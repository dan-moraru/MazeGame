using Microsoft.VisualStudio.TestTools.UnitTesting;
using Maze;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Moq;

namespace Maze.Tests
{
    [TestClass()]
    public class MapTests
    {
        [TestMethod()]
        [ExpectedException(typeof(ArgumentNullException))]
        public void MapTest()
        {
            Map map = new Map(null);
        }

        [TestMethod()]
        public void CreateMapTest()
        {
            Direction[,] grid = {
                { Direction.E | Direction.S, Direction.W | Direction.S},
                { Direction.N, Direction.N}
            };
            var maze = new Mock<IMapProvider>();
            maze.Setup(m => m.CreateMap()).Returns(grid);
            Map map = new Map(maze.Object);

            map.CreateMap();

            Assert.AreEqual(Block.Empty, map.MapGrid[1,1]);
            Assert.AreEqual(Block.Empty, map.MapGrid[3,3]);
            Assert.AreEqual(5, map.Width);
            Assert.AreEqual(5, map.Height);
        }

        [TestMethod()]
        public void CreateMapTestWithInput()
        {
            Direction[,] grid = {
                { Direction.E | Direction.S, Direction.W | Direction.S},
                { Direction.N, Direction.N}
            };
            var maze = new Mock<IMapProvider>();
            maze.Setup(m => m.CreateMap(It.IsAny<int>(), It.IsAny<int>())).Returns((int width, int height) => grid);
            Map map = new Map(maze.Object);
            int width = 5;
            int height = 5;

            map.CreateMap(width, height);

            Assert.AreEqual(Block.Empty, map.MapGrid[1, 1]);
            Assert.AreEqual(Block.Empty, map.MapGrid[3, 3]);
            Assert.AreEqual(5, map.Width);
            Assert.AreEqual(5, map.Height);
        }

        [TestMethod()]
        [ExpectedException(typeof(NotImplementedException))]
        public void SaveDirectionMapTest()
        {
            var maze = new Mock<IMapProvider>();
            Map m = new Map(maze.Object);
            string testPath = "path";

            m.SaveDirectionMap(testPath);
        }
    }
}