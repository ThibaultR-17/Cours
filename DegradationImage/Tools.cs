using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Processing;
using System.Text.Json.Nodes;

namespace DegradationImage;


internal class Tools
{
    private static readonly HttpClient _httpClient = new();

    public static async Task ProcessJSONImages(string filePath)
    {
        string jsonContent = await File.ReadAllTextAsync(filePath);
        var root = JsonNode.Parse(jsonContent)?.AsObject();
        if (root == null) return;

        string outputDir = Path.GetDirectoryName(Path.GetFullPath(filePath));
        Console.WriteLine($"[START] Processing");

        foreach (var entry in root)
        {
            string name = entry.Key;
            string? url = entry.Value?["url"].ToString();
            if (string.IsNullOrEmpty(url)) continue;

            try
            {
                using var stream = await _httpClient.GetStreamAsync(url);
                using var image = await Image.LoadAsync(stream);

                string targetFolder = Path.Combine(outputDir, name);
                Directory.CreateDirectory(targetFolder);

                await SaveVersion(image, targetFolder, name, "1080", image.Width, image.Height);
                await SaveVersion(image, targetFolder, name, "720", 1280, 720);
                await SaveVersion(image, targetFolder, name, "480", 854, 480);

                Console.WriteLine($"[SUCCESS] {name}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[ERROR] {name}: {ex.Message}");
            }
        }

        Console.WriteLine("[FINISHED] ");
    }

    private static async Task SaveVersion(Image img, string folder, string name, string label, int w, int h)
    {
        using var clone = img.Clone(x => x.Resize(new ResizeOptions
        {
            Size = new Size(w, h),
            Mode = ResizeMode.Max
        }));

        string path = Path.Combine(folder, $"{name}_{label}.webp");
        await clone.SaveAsWebpAsync(path);
    }
}