
using System;

internal class Program
{
    [STAThreadAttribute]
    private static void Main(string[] args)
    {
        using var game = new MazeGame.MazeGame();
        game.Run();
    }
}