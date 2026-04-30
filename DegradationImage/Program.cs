

using DegradationImage;
using System.Diagnostics;

var sw = Stopwatch.StartNew();
sw.Start();

await Tools.ProcessJSONImages("../../../DataFolder/images.json");
sw.Stop();
Console.WriteLine("single execution (async download) : " + sw.ElapsedMilliseconds + "ms");

sw.Restart();
await ToolsParallelisme.ProcessJSONImages("../../../DataFolder/images.json");
sw.Stop();
Console.WriteLine("parrallelime execution (async download) : " + sw.ElapsedMilliseconds+"ms");