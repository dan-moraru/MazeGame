using MazeHuntKill;
namespace MazeHuntKillTests;

[TestClass]
public class MazeHuntKillTests
{
    [TestMethod]
    public void CreateMapWithInput()
    {
        int seed = 1;
        int width = 2;
        int height = 2;
        MazeHuntKill.MazeHuntKill mhk = new MazeHuntKill.MazeHuntKill(seed); //5x5 looks like: E|

        Maze.Direction[,] directionMap = mhk.CreateMap(height, width);

        Assert.AreEqual(Maze.Direction.E, directionMap[0, 0]);
        Assert.AreEqual(Maze.Direction.W | Maze.Direction.S, directionMap[0, 1]);
        Assert.AreEqual(Maze.Direction.E, directionMap[1, 0]);
        Assert.AreEqual(Maze.Direction.W | Maze.Direction.N, directionMap[1, 1]);
    }

    [TestMethod]
    public void CreateMapWithInputV2()
    {
        int seed = 1;
        int width = 2;
        int height = 2;
        MazeHuntKillV2 mhk = new MazeHuntKillV2(seed); //5x5 looks like: E|

        Maze.Direction[,] directionMap = mhk.CreateMap(height, width);

        Assert.AreEqual(Maze.Direction.E, directionMap[0, 0]);
        Assert.AreEqual(Maze.Direction.W | Maze.Direction.S, directionMap[0, 1]);
        Assert.AreEqual(Maze.Direction.E, directionMap[1, 0]);
        Assert.AreEqual(Maze.Direction.W | Maze.Direction.N, directionMap[1, 1]);
    }

    [TestMethod]
    public void CreateMapWithoutInput()
    {
        int seed = 2;
        MazeHuntKill.MazeHuntKill mhk = new MazeHuntKill.MazeHuntKill(seed); //5x5 looks like:W 

        mhk.DefaultSizeValue = 2;
        Maze.Direction[,] directionMap = mhk.CreateMap();

        Assert.AreEqual(Maze.Direction.S | Maze.Direction.E, directionMap[0, 0]);
        Assert.AreEqual(Maze.Direction.S | Maze.Direction.W, directionMap[0, 1]);
        Assert.AreEqual(Maze.Direction.N, directionMap[1, 0]);
        Assert.AreEqual(Maze.Direction.N, directionMap[1, 1]);
    }

    [TestMethod]
    public void CreateMapWithoutInputV2()
    {
        int seed = 1;
        MazeHuntKillV2 mhk = new MazeHuntKillV2(seed); //5x5 looks like:E

        mhk.DefaultSizeValue = 2;
        Maze.Direction[,] directionMap = mhk.CreateMap();

        Assert.AreEqual(Maze.Direction.E, directionMap[0, 0]);
        Assert.AreEqual(Maze.Direction.W | Maze.Direction.S, directionMap[0, 1]);
        Assert.AreEqual(Maze.Direction.E, directionMap[1, 0]);
        Assert.AreEqual(Maze.Direction.W | Maze.Direction.N, directionMap[1, 1]);
    }

    [TestMethod()]
    [ExpectedException(typeof(ArgumentException))]
    public void CreateMapWithInvalidInputs()
    {
        int seed = 3;
        int width = -2;
        int height = 2;
        MazeHuntKill.MazeHuntKill mhk = new MazeHuntKill.MazeHuntKill(seed);

        Maze.Direction[,] directionMap = mhk.CreateMap(height, width);
    }
}