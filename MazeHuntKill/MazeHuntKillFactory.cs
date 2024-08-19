using Maze;

namespace MazeHuntKill
{
    public static class MazeHuntKillFactory
    {
        public static IMapProvider GetProvider(bool improved = false)
        {
            if (improved)
            {
                return new MazeHuntKillV2();
            }
            return new MazeHuntKill();
        }
    }
}
