using System.Text.Json;
using System.Text.Json.Nodes;
using System.Xml.Linq;

namespace LinQExo
{
    internal class ToolsLinQExo
    {
        public static string RootName { get; set; } = "Root";
        public static string EntryName { get; set; } = "Item";

        public static List<JsonNode> LoadJson(string filePath)
        {
            var root = JsonNode.Parse(File.ReadAllText(filePath));
            return root is JsonArray array
                ? array.Select(n => n.DeepClone()).ToList()
                : new List<JsonNode> { root.DeepClone() };
        }

        public static List<JsonNode> LoadXml(string filePath)
        {
            var doc = XDocument.Load(filePath);
            var root = doc.Root;

            RootName = root.Name.LocalName;
            EntryName = root.Elements().FirstOrDefault()?.Name.LocalName ?? "Item";

            return root.Elements().Select(el => XmlToJsonNode(el)).ToList();
        }

        private static JsonNode XmlToJsonNode(XElement el)
        {
            if (!el.Elements().Any()) return JsonValue.Create(el.Value);

            var obj = new JsonObject();
            foreach (var child in el.Elements())
            {
                obj[child.Name.LocalName] = XmlToJsonNode(child);
            }
            return obj;
        }

        public static bool SearchDeep(JsonNode node, string term)
        {
            if (node == null) return false;

            if (node is JsonValue val)
                return val.ToString().Contains(term, StringComparison.OrdinalIgnoreCase);

            if (node is JsonObject obj)
                return obj.Any(kvp => SearchDeep(kvp.Value, term));

            if (node is JsonArray arr)
                return arr.Any(item => SearchDeep(item, term));

            return false;
        }

        public static void AfficherDonnees(IEnumerable<JsonNode> data)
        {
            var options = new JsonSerializerOptions { WriteIndented = true };
            foreach (var node in data)
            {
                Console.WriteLine(node.ToJsonString(options));
                Console.WriteLine(new string('-', 20));
            }
            Console.WriteLine($"\nTotal : {data.Count()}");
        }

        public static void ExportToJson(IEnumerable<JsonNode> data, string filePath)
        {
            var array = new JsonArray();
            foreach (var node in data) array.Add(node.DeepClone());

            File.WriteAllText(filePath, array.ToJsonString(new JsonSerializerOptions { WriteIndented = true }));
        }

        public static void ExportToXml(IEnumerable<JsonNode> data, string filePath)
        {
            var xml = new XElement(RootName,
                data.Select(node => JsonToXml(node, EntryName))
            );
            xml.Save(filePath);
        }

        private static XElement JsonToXml(JsonNode node, string name)
        {
            if (node is JsonObject obj)
            {
                return new XElement(name, obj.Select(kvp => JsonToXml(kvp.Value, kvp.Key)));
            }
            if (node is JsonArray arr)
            {
                return new XElement(name, arr.Select(item => JsonToXml(item, "Value")));
            }

            return new XElement(name, node.ToString());
        }
    }
}