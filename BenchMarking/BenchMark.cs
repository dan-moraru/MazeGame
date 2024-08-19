using System.Diagnostics;
using static System.Net.Mime.MediaTypeNames;
using System.Text;
using MazeRecursion;
using Maze;
using MazeHuntKill;
using System;
using System.Drawing;

public class BenchMark
{
    public static void Main(String[] args)
    {
        // Init algorithm classes
        IMapProvider recursionAlgorithm = MazeRecursionFactory.GetProvider();
        IMapProvider recursionAlgorithmV2 = MazeRecursionFactory.GetProvider(true);
        IMapProvider huntKillAlgorithm = MazeHuntKillFactory.GetProvider();
        IMapProvider huntKillAlgorithmV2 = MazeHuntKillFactory.GetProvider(true);

        // Init stopwatch
        Stopwatch timer = new Stopwatch();

        // Create Path for CSV file
        string path = Path.Combine("..", "..", "..", "mazeGraphs.csv");

        // Create string list to save each line
        List<string> lines = new List<string>();

        // Test out Recursion algorithm first
        Console.WriteLine("Square mazes using Recursion Algorithm. Going up to 90");
        for (int size = 2; size < 91; size++)
        {
            TimeSpan timeElapsed = TimeIt(timer, () => recursionAlgorithm.CreateMap(size, size));
            lines.Add(size + ":" + timeElapsed.TotalMilliseconds);
            Console.WriteLine("[" + size + "," + size + "]:" + timeElapsed.TotalMilliseconds);
        }

        lines.Add(":");
        Console.WriteLine("");

        Console.WriteLine("Square mazes using RecursionV2 Algorithm. Going up to 90");
        for (int size = 2; size < 91; size++)
        {
            TimeSpan timeElapsed = TimeIt(timer, () => recursionAlgorithmV2.CreateMap(size, size));
            lines.Add(size + ":" + timeElapsed.TotalMilliseconds);
            Console.WriteLine("[" + size + "," + size + "]:" + timeElapsed.TotalMilliseconds);
        }

        lines.Add(":");
        Console.WriteLine("");

        // Test out HuntKill algorithm second
        Console.WriteLine("Square mazes using HuntKill Algorithm. Going up to 200");
        for (int size = 2; size < 201; size++)
        {
            TimeSpan timeElapsed = TimeIt(timer, () => huntKillAlgorithm.CreateMap(size, size));
            lines.Add(size + ":" + timeElapsed.TotalMilliseconds);
            Console.WriteLine("[" + size + "," + size + "]:" + timeElapsed.TotalMilliseconds);
        }

        lines.Add(":");
        Console.WriteLine("");

        // Test out HuntKillV2 algorithm third
        Console.WriteLine("Square mazes using HuntKillV2 Algorithm. Going up to 200");
        for (int size = 2; size < 201; size++)
        {
            TimeSpan timeElapsed = TimeIt(timer, () => huntKillAlgorithmV2.CreateMap(size, size));
            lines.Add(size + ":" + timeElapsed.TotalMilliseconds);
            Console.WriteLine("[" + size + "," + size + "]:" + timeElapsed.TotalMilliseconds);
        }

        // Write to file
        WriteToFile(path, lines);
        Console.WriteLine("Data has been written to a CSV file");
    }

    public static void WriteToFile(string path, List<string> data)
    {
        try
        {
            using (StreamWriter writer = (File.Exists(path)) ? File.AppendText(path) : File.CreateText(path))
            {
                foreach (string line in data)
                {
                    string[] parts = line.Split(':');

                    if (parts.Length >= 2)
                    {
                        writer.WriteLine($"{parts[0].Trim()}, {parts[1].Trim()}");
                    }
                }
            }
        }
        catch (IOException e)
        {
            Console.WriteLine($"An error occurred: {e.Message}");
        }
    }

    public static TimeSpan TimeIt(Stopwatch timer, Action bench)
    {
        timer.Reset();
        timer.Start();
        bench.Invoke();
        timer.Stop();
        return timer.Elapsed;
    }
}
