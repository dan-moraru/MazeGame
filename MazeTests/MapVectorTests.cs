using Microsoft.VisualStudio.TestTools.UnitTesting;
using Maze;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Numerics;

namespace Maze.Tests
{
    [TestClass()]
    public class MapVectorTests
    {
        [TestMethod()]
        public void MapVectorTest()
        {
            MapVector vector = new MapVector(2, 2);
            Assert.IsTrue(vector.IsValid);
        }

        [TestMethod()]
        public void MapVectorTestInvalid()
        {
            MapVector vector = new MapVector(-2,2);
            MapVector vector2 = new MapVector(2,-2);
            Assert.IsFalse(vector.IsValid);
            Assert.IsFalse(vector2.IsValid);
        }

        [TestMethod()]
        public void InsideBoundaryTest()
        {
            int width = 5;
            int height = 5;
            MapVector vector = new MapVector(2,2);

            bool inside = vector.InsideBoundary(width, height);

            Assert.IsTrue(inside);
        }

        [TestMethod()]
        public void InsideBoundaryTestNegative()
        {
            int width = 5;
            int height = 5;
            MapVector vector = new MapVector(-2, 2);

            bool inside = vector.InsideBoundary(width, height);

            Assert.IsFalse(inside);
        }

        [TestMethod()]
        public void MagnitudeTest()
        {
            int x = 5;
            int y = 5;
            MapVector vector = new MapVector(x, y);

            double magnitude = vector.Magnitude();

            Assert.AreEqual(Math.Sqrt(50), magnitude);
        }

        [TestMethod()]
        public void MapVectorPlusOperatorTest()
        {
            MapVector vector1 = new MapVector(4, 1);
            MapVector vector2 = new MapVector(4, 2);

            MapVector result = vector1 + vector2;

            Assert.AreEqual(3, result.X);
            Assert.AreEqual(8, result.Y);
        }

        [TestMethod()]
        public void MapVectorMinusOperatorTest()
        {
            MapVector vector1 = new MapVector(4, 1);
            MapVector vector2 = new MapVector(4, 2);

            MapVector result = vector2 - vector1;

            Assert.AreEqual(1, result.X);
            Assert.AreEqual(0, result.Y);
        }

        [TestMethod()]
        public void MapVectorMultiplierOperatorTest()
        {
            MapVector vector1 = new MapVector(4, 1);
            int constant = 2;

            MapVector result = vector1 * constant;

            Assert.AreEqual(2, result.X);
            Assert.AreEqual(8, result.Y);
        }

        [TestMethod()]
        public void MapVectorEqualsOperatorTest()
        {
            MapVector vector1 = new MapVector(4, 1);
            MapVector vector2 = new MapVector(4, 1);

            bool result = vector1.Equals(vector2);

            Assert.IsTrue(result);
        }

        [TestMethod()]
        public void MapVectorNotEqualsOperatorTest()
        {
            MapVector vector1 = new MapVector(1, 1);
            MapVector vector2 = new MapVector(4, 1);

            bool result = vector1.Equals(vector2);

            Assert.IsFalse(result);
        }

        [TestMethod()]
        public void MapVectorDirectionCastingTest()
        {
            MapVector vector1 = Direction.N;
            MapVector vector2 = Direction.E;
            MapVector vector3 = Direction.W;
            MapVector vector4 = Direction.S;
            MapVector vector5 = Direction.None;

            Assert.AreEqual(-1, vector1.Y);
            Assert.AreEqual(0, vector1.X);

            Assert.AreEqual(0, vector2.Y);
            Assert.AreEqual(1, vector2.X);

            Assert.AreEqual(0, vector3.Y);
            Assert.AreEqual(-1, vector3.X);

            Assert.AreEqual(1, vector4.Y);
            Assert.AreEqual(0, vector4.X);

            Assert.AreEqual(0, vector5.Y);
            Assert.AreEqual(0, vector5.X);
        }
    }
}