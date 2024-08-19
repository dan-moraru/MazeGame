using Microsoft.VisualStudio.TestTools.UnitTesting;
using Maze;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Moq;
using System.Reflection.Emit;

namespace Maze.Tests
{
    [TestClass()]
    public class PlayerTests
    {
        [TestMethod()]
        public void PlayerTest()
        {
            Block[,] mapGrid = {
                { Block.Solid, Block.Solid, Block.Solid },
                { Block.Solid, Block.Empty, Block.Solid },
                { Block.Solid, Block.Solid, Block.Solid }
            };

            Player p = new Player(1,1, mapGrid);
            Block result = mapGrid[p.Position.Y, p.Position.X];

            Assert.AreEqual(Block.Empty, result);
        }

        [TestMethod()]
        public void GetRotationTest()
        {
            Block[,] mapGrid = {
                { Block.Solid, Block.Solid, Block.Solid },
                { Block.Solid, Block.Empty, Block.Solid },
                { Block.Solid, Block.Solid, Block.Solid }
            };

            Player p = new Player(1, 1, mapGrid);
            p.Facing = Direction.N;
            float result = p.GetRotation();

            Player p2 = new Player(1, 1, mapGrid);
            p2.Facing = Direction.S;
            float result2 = p2.GetRotation();

            Player p3 = new Player(1, 1, mapGrid);
            p3.Facing = Direction.E;
            float result3 = p3.GetRotation();

            Player p4 = new Player(1, 1, mapGrid);
            p4.Facing = Direction.W;
            float result4 = p4.GetRotation();

            Player p5 = new Player(1, 1, mapGrid);
            p5.Facing = Direction.None;
            float result5 = p5.GetRotation();

            Assert.AreEqual(0, result);
            Assert.AreEqual((float)Math.PI, result2);
            Assert.AreEqual((float)(Math.PI / 2), result3);
            Assert.AreEqual((float)((3 * Math.PI) / 2), result4);
            Assert.AreEqual(-1, result5);
        }

        [TestMethod()]
        public void MoveBackwardTest()
        {
            Block[,] mapGrid = {
                { Block.Solid, Block.Solid, Block.Solid },
                { Block.Solid, Block.Empty, Block.Solid },
                { Block.Solid, Block.Empty, Block.Solid },
                { Block.Solid, Block.Solid, Block.Solid}
            };

            Player p = new Player(1, 1, mapGrid);
            p.Facing = Direction.N;
            p.MoveBackward();
            MapVector result = p.Position;

            Assert.AreEqual(2, result.Y);
            Assert.AreEqual(1, result.X);
        }

        [TestMethod()]
        public void MoveForwardTest()
        {
            Block[,] mapGrid = {
                { Block.Solid, Block.Solid, Block.Solid },
                { Block.Solid, Block.Empty, Block.Solid },
                { Block.Solid, Block.Empty, Block.Solid },
                { Block.Solid, Block.Solid, Block.Solid }
            };

            Player p = new Player(2, 1, mapGrid);
            p.Facing = Direction.N;
            p.MoveForward();
            MapVector result = p.Position;

            Assert.AreEqual(1, result.Y);
            Assert.AreEqual(1, result.X);
        }

        [TestMethod()]
        public void TurnLeftTest()
        {
            Block[,] mapGrid = {
                { Block.Solid, Block.Solid, Block.Solid, Block.Solid },
                { Block.Solid, Block.Empty, Block.Empty, Block.Solid },
                { Block.Solid, Block.Empty, Block.Solid, Block.Solid },
                { Block.Solid, Block.Solid, Block.Solid, Block.Solid }
            };

            Player p = new Player(1, 1, mapGrid);
            p.Facing = Direction.E;
            p.TurnLeft();
            Direction result = p.Facing;

            Player p2 = new Player(1, 1, mapGrid);
            p2.Facing = Direction.W;
            p2.TurnLeft();
            Direction result2 = p2.Facing;

            Player p3 = new Player(1, 1, mapGrid);
            p3.Facing = Direction.N;
            p3.TurnLeft();
            Direction result3 = p3.Facing;

            Player p4 = new Player(1, 1, mapGrid);
            p4.Facing = Direction.S;
            p4.TurnLeft();
            Direction result4 = p4.Facing;

            Assert.AreEqual(Direction.N, result);
            Assert.AreEqual(Direction.S, result2);
            Assert.AreEqual(Direction.W, result3);
            Assert.AreEqual(Direction.E, result4);
        }

        [TestMethod()]
        public void TurnRightTest()
        {
            Block[,] mapGrid = {
                { Block.Solid, Block.Solid, Block.Solid, Block.Solid },
                { Block.Solid, Block.Empty, Block.Empty, Block.Solid },
                { Block.Solid, Block.Empty, Block.Solid, Block.Solid },
                { Block.Solid, Block.Solid, Block.Solid, Block.Solid }
            };

            Player p = new Player(1, 2, mapGrid);
            p.Facing = Direction.E;
            p.TurnRight();
            Direction result = p.Facing;

            Player p2 = new Player(1, 2, mapGrid);
            p2.Facing = Direction.W;
            p2.TurnRight();
            Direction result2 = p2.Facing;

            Player p3 = new Player(1, 2, mapGrid);
            p3.Facing = Direction.N;
            p3.TurnRight();
            Direction result3 = p3.Facing;

            Player p4 = new Player(1, 2, mapGrid);
            p4.Facing = Direction.S;
            p4.TurnRight();
            Direction result4 = p4.Facing;

            Assert.AreEqual(Direction.S, result);
            Assert.AreEqual(Direction.N, result2);
            Assert.AreEqual(Direction.E, result3);
            Assert.AreEqual(Direction.W, result4);
        }
    }
}