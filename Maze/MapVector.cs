using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace Maze
{
    public class MapVector : IMapVector
    {
        public bool IsValid => X >= 0 && Y >= 0;

        public int X { get; private set; }

        public int Y { get; private set; }

        public MapVector(int y, int x) 
        {
            this.Y = y;
            this.X = x;
        }

        public override string ToString()
        {
            return "[" + this.Y + ", " + this.X + "]";
        }

        //Checks if position is positive and inside the map
        public bool InsideBoundary(int width, int height)
        {
            if (((X > 0 && X < width-1) && (Y > 0 && Y < height-1)) && IsValid)
            {
                return true;
            }
            return false;
        }

        //Calculates magnitude of position from origin
        public double Magnitude()
        {
            return Math.Sqrt(Math.Pow(this.Y, 2) + (Math.Pow(this.X, 2)));
        }

        //Operator overloading
        public static MapVector operator +(MapVector v1, MapVector v2)
        {
            return new MapVector((v1.Y+v2.Y),(v1.X+v2.X));
        }

        public static MapVector operator -(MapVector v1, MapVector v2)
        {
            return new MapVector((v1.Y - v2.Y), (v1.X - v2.X));
        }

        public static MapVector operator *(MapVector v1, int constant)
        {
            return new MapVector(v1.Y*constant, v1.X*constant);
        }

        public override bool Equals(Object mpv)
        {
            if (mpv is MapVector other)
            {
                return this.X == other.X && this.Y == other.Y;
            }
            return false;
        }

        //Casting overloading
        public static implicit operator MapVector(Direction type)
        {
            switch (type)
            {
                case Direction.N:
                    return new MapVector(-1, 0);
                case Direction.S:
                    return new MapVector(1, 0);
                case Direction.E:
                    return new MapVector(0, 1);
                case Direction.W:
                    return new MapVector(0, -1);
                default:
                    return new MapVector(0,0);
            }
        }
    }
}
