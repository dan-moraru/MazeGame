using Maze;

namespace MazeRecursion
{
    public static class MazeRecursionFactory
    {
        public static IMapProvider GetProvider(bool improved = false)
        {
            if (improved)
            {
                return new MazeRecursionV2();
            }
            return new MazeRecursion();
        }
    }
}
