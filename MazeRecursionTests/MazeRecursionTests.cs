using MazeRecursion;
namespace MazeRecursionTests;

[TestClass]
public class MazeRecursionTests
{
    [TestMethod]
    public void CreateMapWithInput()
    {
        int seed = 1;
        int width = 2;
        int height = 2;
        MazeRecursion.MazeRecursion mz = new MazeRecursion.MazeRecursion(seed); //5x5 looks like: E|

        Maze.Direction[,] directionMap = mz.CreateMap(height, width);

        Assert.AreEqual(Maze.Direction.E, directionMap[0,0]);
        Assert.AreEqual(Maze.Direction.W | Maze.Direction.S, directionMap[0,1]);
        Assert.AreEqual(Maze.Direction.E, directionMap[1, 0]);
        Assert.AreEqual(Maze.Direction.W | Maze.Direction.N, directionMap[1, 1]);
    }

    [TestMethod]
    public void CreateMapWithoutInput()
    {
        int seed = 2;
        MazeRecursion.MazeRecursion mz = new MazeRecursion.MazeRecursion(seed); //5x5 looks like:M
        
        mz.DefaultSizeValue = 2;
        Maze.Direction[,] directionMap = mz.CreateMap();

        Assert.AreEqual(Maze.Direction.S, directionMap[0, 0]);
        Assert.AreEqual(Maze.Direction.S, directionMap[0, 1]);
        Assert.AreEqual(Maze.Direction.N | Maze.Direction.E, directionMap[1, 0]);
        Assert.AreEqual(Maze.Direction.N | Maze.Direction.W, directionMap[1, 1]);
    }

    [TestMethod()]
    [ExpectedException(typeof(ArgumentException))]
    public void CreateMapWithInvalidInputs()
    {
        int seed = 3;
        int width = 2;
        int height = -2;
        MazeRecursion.MazeRecursion mz = new MazeRecursion.MazeRecursion(seed);

        Maze.Direction[,] directionMap = mz.CreateMap(height, width);
    }
}