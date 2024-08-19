using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace Maze
{
    public class Player : IPlayer
    {
        public Direction Facing { get; set; }

        public int StartX { get; private set; }

        public int StartY { get; private set; }

        public MapVector Position { get; set; }

        private Block[,] _mapGrid;

        public Player(int y, int x, Block[,] mapGrid)
        {
            this.StartX = x;
            this.StartY = y;
            this.Position = new MapVector(y, x);
            this._mapGrid = mapGrid;
            this.Facing = setupFacing();
        }

        public override string ToString()
        {
            return "Player: [" + this.Position.Y + ", " + this.Position.X + "] Facing: " + this.Facing;
        }

        //Sets the facing of the player to face an empty block at start
        private Direction setupFacing()
        {
            if (_mapGrid[this.Position.Y, this.Position.X + 1] == Block.Empty)
            {
                return Direction.E;
            } else if (_mapGrid[this.Position.Y, this.Position.X - 1] == Block.Empty)
            {
                return Direction.W;
            } else if (_mapGrid[this.Position.Y + 1, this.Position.X] == Block.Empty)
            {
                return Direction.S;
            } else if (_mapGrid[this.Position.Y - 1, this.Position.X] == Block.Empty)
            {
                return Direction.N;
            } else
            {
                return Direction.None;
            }
        }

        //Returns the direction in radians
        public float GetRotation()
        {
            switch (this.Facing)
            {
                case Direction.N:
                    return 0;
                case Direction.S:
                    return (float)Math.PI;
                case Direction.E:
                    return (float)(Math.PI / 2);
                case Direction.W:
                    return (float)((3 * Math.PI) / 2);
                default:
                    return -1;
            }
        }

        //Moves player backward based on current facing
        public void MoveBackward()
        {
            MapVector nextPos = Position - Facing;
            if (_mapGrid[nextPos.Y, nextPos.X] == Block.Empty)
            {
                Position = nextPos;
            }
        }

        //Moves player forward based on current facing
        public void MoveForward()
        {
            MapVector nextPos = Position + Facing;
            if (_mapGrid[nextPos.Y, nextPos.X] == Block.Empty)
            {
                Position = nextPos;
            }
        }

        //Turns player direction to the left
        public void TurnLeft()
        {
            switch (this.Facing)
            {
                case Direction.N:
                    this.Facing = Direction.W;
                    break;
                case Direction.S:
                    this.Facing = Direction.E;
                    break;
                case Direction.E:
                    this.Facing = Direction.N;
                    break;
                case Direction.W:
                    this.Facing = Direction.S;
                    break;
                default:
                    break;
            }
        }

        //Turns player direction to the right
        public void TurnRight()
        {
            switch (this.Facing)
            {
                case Direction.N:
                    this.Facing = Direction.E;
                    break;
                case Direction.S:
                    this.Facing = Direction.W;
                    break;
                case Direction.E:
                    this.Facing = Direction.S;
                    break;
                case Direction.W:
                    this.Facing = Direction.N;
                    break;
                default:
                    break;
            }   
        }
    }
}
