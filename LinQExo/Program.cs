using LinQExo;
using System.Text.Json.Nodes;

bool finish = false;

// selection et Validation
Console.WriteLine("File Path");
string path = Console.ReadLine();
if (!File.Exists(path))
{
    Console.WriteLine("Erreur : Le fichier n'existe pas.");
    return 1;
}

// identification du type
string extension = Path.GetExtension(path).ToLower();
List<JsonNode> dataSet = new List<JsonNode>();

// chargement 
if (extension == ".json")
{
    dataSet = ToolsLinQExo.LoadJson(path);
}
else if (extension == ".xml")
{
    dataSet = ToolsLinQExo.LoadXml(path);
}
else
{
    Console.WriteLine("Format inconnu. Utilisez .json ou .xml");
    return 1;
}

//initial preview
ToolsLinQExo.AfficherDonnees(dataSet);

//loop : 
// --> choose search, sort, export
// --> show results 
//copie to fallback
List<JsonNode> results = new List<JsonNode>(dataSet);

while (!finish)
{
    Console.WriteLine("\nActions: cherche (c), trie (t), export (e), reset (r), quitter (q)");
    var choice = Console.ReadLine()?.ToLower();

    if (choice == "c")
    {
        Console.WriteLine("Entrez le terme à rechercher :");
        string term = Console.ReadLine();

        if (!string.IsNullOrEmpty(term))
        {
            results = results.Where(n => ToolsLinQExo.SearchDeep(n, term)).ToList();

            Console.WriteLine($"Résultats : {results.Count()}");
            ToolsLinQExo.AfficherDonnees(results);
        }
    }
    else if (choice == "t")
    {
        Console.WriteLine("Nom de la colonne pour le tri :");
        var col = Console.ReadLine();

        dataSet = dataSet.OrderBy(n => {
            var val = n[col]?.ToString();
            if (val == null) return (object)"";

            if (double.TryParse(val, System.Globalization.CultureInfo.InvariantCulture, out double num))
                return num;
            return val;
        }).ToList();

        ToolsLinQExo.AfficherDonnees(dataSet);
    }
    else if (choice == "r")
    {
        results = new List<JsonNode>(dataSet);
        Console.WriteLine("Données réinitialisées.");
        ToolsLinQExo.AfficherDonnees(results);
    }
    else if (choice == "e")
    {
        Console.WriteLine("Format (json/xml) :");
        var format = Console.ReadLine().ToLower();
        Console.WriteLine("Nom du fichier :");
        var fileName = Console.ReadLine();

        string exportPath = fileName + "." + format;

        if (format == "json") ToolsLinQExo.ExportToJson(results, exportPath);
        else if (format == "xml") ToolsLinQExo.ExportToXml(results, exportPath);

        Console.WriteLine($"Export réussi : {exportPath}");
    }
    else if (choice == "q") finish = true;
}

return 0;