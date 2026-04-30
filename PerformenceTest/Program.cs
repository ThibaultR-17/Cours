using System.Diagnostics;

Console.WriteLine("Calcul de performance ");

for (int i=0; i<10; i++)
{
    var sw = Stopwatch.StartNew();
    sw.Restart();
    // on éxécute 50 millions de calculs
    object locker = new object();
    double sum = 0;

    Parallel.For<double>(0, 50_000_000,
        () => 0,
        (i, state, local) =>
        {
            local += Math.Sin(i) + Math.Cos(i);
            local += Math.Sqrt(i);
            local += Math.Exp(i % 10) + Math.Log(i);
            local += Math.Pow(i % 100, 3);
            local *= 1.0000001;

            return local;
        },
        local =>
        {
            lock (locker)
            {
                sum += local;
            }
        });
    Console.WriteLine($"Temps de calcul parallele : {sw.ElapsedMilliseconds} ms" + " sum " + sum);
}
