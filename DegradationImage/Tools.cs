using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Formats.Webp;
using SixLabors.ImageSharp.Processing;
using System.Net.Http.Json;
using System.Text.Json.Nodes;

public record ImageData(string Name, Image Image);

namespace DegradationImage
{
    internal class Tools
    {
        private static readonly HttpClient _httpClient = new HttpClient();

        public static async Task ProcessJSONImages(string filePath)
        {
            Console.WriteLine($"[START] Reading JSON from: {filePath}");

            string? jsonDirectory = Path.GetDirectoryName(Path.GetFullPath(filePath));
            if (jsonDirectory == null) return;

            string jsonContent = await File.ReadAllTextAsync(filePath);
            var root = JsonNode.Parse(jsonContent)?.AsObject();
            if (root == null)
            {
                Console.WriteLine("[ERROR] JSON content is null or invalid.");
                return;
            }

            Console.WriteLine($"[INFO] Found {root.Count} entries. Starting downloads...");

            var downloadTasks = root.Select(async values =>
            {
                string name = values.Key;
                string? url = values.Value?["url"]?.ToString();
                if (string.IsNullOrEmpty(url)) return null;

                try
                {
                    Console.WriteLine($"[DOWNLOAD] Starting: {name}");
                    byte[] imageBytes = await _httpClient.GetByteArrayAsync(url);
                    var image = Image.Load(new MemoryStream(imageBytes));
                    Console.WriteLine($"[DOWNLOAD] Success: {name} ({image.Width}x{image.Height})");
                    return new ImageData(name, image);
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"[DOWNLOAD ERROR] {name}: {ex.Message}");
                    return null;
                }
            });

            var results = await Task.WhenAll(downloadTasks);
            var toDegrade = results.Where(r => r != null).Cast<ImageData>().ToList();

            Console.WriteLine($"[INFO] Downloaded {toDegrade.Count} images. starting degradation...");

            var tasks = toDegrade.Select(async item =>
            {
                try
                {
                    string baseName = item.Name;
                    string targetFolder = Path.Combine(jsonDirectory, baseName);
                    Directory.CreateDirectory(targetFolder);

                    Console.WriteLine($"[PROCESS] Processing: {baseName} -> {targetFolder}");

                    string path1080 = Path.Combine(targetFolder, $"{baseName}_1080.webp");
                    Task saveOriginal = item.Image.SaveAsWebpAsync(path1080);
                    Console.WriteLine($"[SAVE] Original queued: {baseName}_1080.webp");

                    var resolutions = new[]
                    {
                        new { Label = "720", Width = 1280, Height = 720 },
                        new { Label = "480", Width = 854, Height = 480 }
                    };

                    foreach (var res in resolutions)
                    {
                        using (Image clone = item.Image.Clone(ctx => ctx.Resize(res.Width, res.Height)))
                        {
                            string pathRes = Path.Combine(targetFolder, $"{baseName}_{res.Label}.webp");
                            Console.WriteLine($"[RESIZE] {baseName} -> {res.Label}p...");
                            await clone.SaveAsWebpAsync(pathRes);
                            Console.WriteLine($"[SAVE] Done: {baseName}_{res.Label}.webp");
                        }
                    }

                    await saveOriginal;
                    Console.WriteLine($"[COMPLETE] All versions saved for: {baseName}");
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"[PROCESS ERROR] Failed to process {item.Name}: {ex.Message}");
                }
                finally
                {
                    item.Image.Dispose();
                }
            });

            await Task.WhenAll(tasks);
            Console.WriteLine("\n[FINISHED] All images processed successfully.");
        }

    }


}