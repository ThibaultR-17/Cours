using System.Text.Json.Nodes;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Formats.Webp;
using SixLabors.ImageSharp.Processing;

namespace DegradationImage
{
    internal class ToolsParallelisme
    {
        private static readonly HttpClient _httpClient = new HttpClient();

        public static async Task ProcessJSONImages(string filePath)
        {
            Console.WriteLine($"[START] Reading JSON from: {filePath}");

            string? jsonDirectory = Path.GetDirectoryName(Path.GetFullPath(filePath));
            if (jsonDirectory == null) return;

            string jsonContent = await File.ReadAllTextAsync(filePath);
            var root = JsonNode.Parse(jsonContent)?.AsObject();
            if (root == null) return;

            // --- PHASE 1: STANDARD ASYNC DOWNLOAD (Sequential Start) ---
            Console.WriteLine($"[INFO] Starting downloads...");
            var downloadTasks = root.Select(async values =>
            {
                string name = values.Key;
                string? url = values.Value?["url"]?.ToString();
                if (string.IsNullOrEmpty(url)) return null;

                try
                {
                    byte[] imageBytes = await _httpClient.GetByteArrayAsync(url);
                    var image = Image.Load(new MemoryStream(imageBytes));
                    Console.WriteLine($"[DOWNLOAD] Success: {name}");
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

            // --- PHASE 2: TRUE PARALLELISM FOR PROCESSING ---
            Console.WriteLine($"[INFO] Starting Parallel Degradation on {toDegrade.Count} images...");

            // We limit parallelism here to prevent 100% CPU usage or RAM exhaustion
            // Environment.ProcessorCount is a good safe default for CPU-bound tasks
            var parallelOptions = new ParallelOptions
            {
                MaxDegreeOfParallelism = Environment.ProcessorCount
            };

            await Parallel.ForEachAsync(toDegrade, parallelOptions, async (item, ct) =>
            {
                try
                {
                    string baseName = item.Name;
                    string targetFolder = Path.Combine(jsonDirectory, baseName);
                    Directory.CreateDirectory(targetFolder);

                    Console.WriteLine($"[PARALLEL THREAD] Processing {baseName}...");

                    // 1. Save Original 1080p
                    string path1080 = Path.Combine(targetFolder, $"{baseName}_1080.webp");
                    Task saveOriginal = item.Image.SaveAsWebpAsync(path1080, ct);

                    var resolutions = new[]
                    {
                        new { Label = "720", Width = 1280, Height = 720 },
                        new { Label = "480", Width = 854, Height = 480 }
                    };

                    // 2. Process other resolutions
                    foreach (var res in resolutions)
                    {
                        using (Image clone = item.Image.Clone(ctx => ctx.Resize(res.Width, res.Height)))
                        {
                            string pathRes = Path.Combine(targetFolder, $"{baseName}_{res.Label}.webp");
                            await clone.SaveAsWebpAsync(pathRes, ct);
                        }
                    }

                    await saveOriginal;
                    Console.WriteLine($"[COMPLETE] Saved all versions for: {baseName}");
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"[ERROR] Failed to process {item.Name}: {ex.Message}");
                }
                finally
                {
                    // Essential for memory management in parallel loops
                    item.Image.Dispose();
                }
            });

            Console.WriteLine("\n[FINISHED] Processed all images.");
        }
    }
}