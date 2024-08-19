using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Reflection.Metadata.Ecma335;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Maze
{
    public class Map : IMap
    {
        public int Height { get; private set; }

        public bool IsGameFinished => Player.Position.Equals(Goal);

        public Block[,] MapGrid { get; private set; }

        public IPlayer Player { get; private set; }

        public int Width { get; private set; }

        public MapVector Goal { get; private set; }

        private IMapProvider Imp { get;  }

        public Map(IMapProvider imp)
        {
            if (imp == null)
            {
                throw new ArgumentNullException("Map provider can't be null");
            }
            this.Imp = imp;
        }

        public void CreateMap()
        {
            var directionMap = Imp.CreateMap();

            //Size of Map
            this.Width = 2 + directionMap.GetLength(1) + (directionMap.GetLength(1) - 1);
            this.Height = 2 + directionMap.GetLength(0) + (directionMap.GetLength(0) - 1);

            MapGrid = new Block[this.Height, this.Width];

            //MapGrid
            CreateMapGrid(directionMap);

            //Player
            createPlayer();

            //Goal
            createGoal();
        }

        private void CreateMapGrid(Direction[,] directionMap)
        {
            for (int y = 0; y < directionMap.GetLength(0); y++)
            {
                for (int x = 0; x < directionMap.GetLength(1); x++)
                {
                    MapGrid[2 * y + 1, 2 * x + 1] = Block.Empty;

                    if ((directionMap[y, x] & Direction.E) > 0)
                    {
                        MapGrid[2 * y + 1, 2 * x + 1 + 1] = Block.Empty;
                    }

                    if ((directionMap[y, x] & Direction.W) > 0)
                    {
                        MapGrid[2 * y + 1, 2 * x + 1] = Block.Empty;
                    }

                    if ((directionMap[y, x] & Direction.S) > 0)
                    {
                        MapGrid[2 * y + 1 + 1, 2 * x + 1] = Block.Empty;
                    }

                    if ((directionMap[y, x] & Direction.N) > 0)
                    {
                        MapGrid[2 * y + 1, 2 * x + 1] = Block.Empty;
                    }
                }
            }
        }

        //Creates a random position for player
        private void createPlayer()
        {
            while (true)
            {
                Random r = new Random();
                int x = r.Next(1, Width);
                int y = r.Next(1, Height);
                MapVector pStart = new MapVector(y, x);
                if (pStart.InsideBoundary(Width, Height) && MapGrid[y, x] == Block.Empty)
                {
                    Player = new Player(y, x, this.MapGrid);
                    break;
                }
            }
        }

        //Creates the furthest goal position from the player
        private void createGoal()
        {
            double previousMagnitude = 0;
            for (int y = 0; y < Height; y++)
            {
                for (int x = 0; x < Width; x++)
                {
                    MapVector current = new MapVector(y, x);
                    if (!(MapGrid[y, x] == Block.Empty && !(Player.Position.Equals(current))))
                    {
                        continue;
                    }

                    if (MapGrid[y, x + 1] == Block.Solid && MapGrid[y, x - 1] == Block.Solid)
                    {
                        if (!(MapGrid[y + 1, x] == Block.Solid || MapGrid[y - 1, x] == Block.Solid))
                        {
                            continue;
                        }
                        
                        if (checkGoal(current, ref previousMagnitude))
                        {
                            continue;
                        }
                    }

                    if (MapGrid[y + 1, x] == Block.Solid && MapGrid[y - 1, x] == Block.Solid)
                    {
                        if (!(MapGrid[y, x + 1] == Block.Solid || MapGrid[y, x - 1] == Block.Solid))
                        {
                            continue;
                        }

                        if (checkGoal(current, ref previousMagnitude))
                        {
                            continue;
                        }
                    }
                }
            }
        }

        //Filters out previous magnitudes to ensure the furthest goal position
        private bool checkGoal(MapVector current, ref double previousMagnitude)
        {
            MapVector pgVector = Player.Position - current;
            double pgMagnitude = pgVector.Magnitude();
            if (pgMagnitude < previousMagnitude)
            {
                return true;
            }
            previousMagnitude = pgMagnitude;
            Goal = current;
            return false;
        }

        public void CreateMap(int width, int height)
        {
            // Size of Map
            this.Width = width;
            this.Height = height;

            // Reverse size find for direction map
            int directionWidth = (this.Width - 1) / 2;
            int directionHeight = (this.Height - 1) / 2;

            var directionMap = Imp.CreateMap(directionHeight, directionWidth);

            MapGrid = new Block[this.Height, this.Width];

            //MapGrid
            CreateMapGrid(directionMap);

            //Player
            createPlayer();

            //Goal
            createGoal();
        }

        public void SaveDirectionMap(string path)
        {
            throw new NotImplementedException();
        }
    }
}
