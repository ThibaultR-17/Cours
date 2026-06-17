using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Processing;
using System.Text.Json.Nodes;

namespace DegradationImage;

internal class ToolsParallelisme
{
    private static readonly HttpClient _httpClient = new();

    public static async Task ProcessJSONImages(string filePath)
    {
        var jsonContent = await File.ReadAllTextAsync(filePath);
        var root = JsonNode.Parse(jsonContent)?.AsObject() ?? throw new Exception("Invalid JSON");

        string outputDir = Path.GetDirectoryName(Path.GetFullPath(filePath)) ?? "";
        //Attention ici, on prend tout. Peut etre diminué le max si besoin
        var options = new ParallelOptions { MaxDegreeOfParallelism = Environment.ProcessorCount };

        await Parallel.ForEachAsync(root, options, async (entry, ct) =>
        {
            string name = entry.Key;
            string? url = entry.Value?["url"]?.ToString();
            if (string.IsNullOrEmpty(url)) return;

            try
            {
                using var stream = await _httpClient.GetStreamAsync(url, ct);
                using var image = await Image.LoadAsync(stream, ct);

                string targetFolder = Path.Combine(outputDir, name);
                Directory.CreateDirectory(targetFolder);
                var configs = new[] {
                    (Label: "1080", W: image.Width, H: image.Height),
                    (Label: "720",  W: 1280,        H: 720),
                    (Label: "480",  W: 854,         H: 480)
                };

                foreach (var config in configs)
                {
                    using var clone = image.Clone(x => x.Resize(new ResizeOptions
                    {
                        Size = new Size(config.W, config.H),
                        Mode = ResizeMode.Max
                    }));

                    string path = Path.Combine(targetFolder, $"{name}_{config.Label}.webp");
                    await clone.SaveAsWebpAsync(path, ct);
                }

                Console.WriteLine($"[DONE] {name}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[ERROR] {name}: {ex.Message}");
            }
        });
    }
}