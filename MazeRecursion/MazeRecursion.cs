using Maze;
using System;
using System.Runtime.CompilerServices;

[assembly: InternalsVisibleTo("MazeRecursionTests")]
namespace MazeRecursion;
internal class MazeRecursion : IMapProvider
{
    private Random _random;
    private Direction[,] _directionMap;
    private List<MapVector> _previousVectors;

    //Public for testing purposes
    public int DefaultSizeValue;

    public MazeRecursion(int? seed=null)
    {
        if (seed != null)
        {
            _random = new Random((int)seed);
        } else
        {
            _random = new Random();
        }
        DefaultSizeValue = 10;
    }

    public Direction[,] CreateMap(int height, int width)
    {
        if (height < 0 || width < 0)
        {
            throw new ArgumentException("Width and Height must be positive!");
        }

        _previousVectors = new List<MapVector>();

        _directionMap = new Direction[height, width];

        MapVector startingVector = new MapVector(0,0);

        Walking(startingVector, _directionMap);

        return _directionMap;
    }

    private void Walking(MapVector vector, Direction[,] directionMap)
    {
        List<Direction> allDirections = Enum.GetValues(typeof(Direction)).Cast<Direction>()
            .Where(direction => direction != Direction.None).ToList();
        Shuffle(allDirections);

        foreach (Direction dir in allDirections)
        {
            Direction oppositeDir = OppositeDir(dir);
            MapVector nextPos = vector + dir;
            
            if (!_previousVectors.Contains(nextPos) && nextPos.IsValid && nextPos.Y < directionMap.GetLength(0) && nextPos.X < directionMap.GetLength(1)
                && directionMap[nextPos.Y, nextPos.X] == Direction.None)
            {
                directionMap[vector.Y, vector.X] = directionMap[vector.Y, vector.X] | dir;
                directionMap[nextPos.Y, nextPos.X] = directionMap[nextPos.Y, nextPos.X] | oppositeDir;
                if (!_previousVectors.Contains(vector))
                {
                    _previousVectors.Add(vector);
                }
                Walking(nextPos, directionMap);
            }
        }
    }

    //Fisher-Yates Shuffle
    private void Shuffle<Direction>(List<Direction> dirs)
    {
        int n = dirs.Count;
        while (n > 1)
        {
            n--;
            int k = _random.Next(n + 1);
            Direction dir = dirs[k];
            dirs[k] = dirs[n];
            dirs[n] = dir;
        }
    }

    private Direction OppositeDir(Direction d)
    {
        switch (d)
        {
            case Direction.N:
                return Direction.S;
            case Direction.S:
                return Direction.N;
            case Direction.E:
                return Direction.W;
            case Direction.W:
                return Direction.E;
            default:
                return Direction.None;
        }
    }

    public Direction[,] CreateMap()
    {
        return CreateMap(DefaultSizeValue, DefaultSizeValue);
    }
}
