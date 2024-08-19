using Maze;
using System.Collections.Generic;
using System.Numerics;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Runtime.Intrinsics;

[assembly: InternalsVisibleTo("MazeHuntKillTests")]
namespace MazeHuntKill;
internal class MazeHuntKillV2 : IMapProvider
{
    private Random _random;
    private Direction[,] _directionMap;
    private MapVector _currentPos;
    private List<Direction> _allDirections;

    private HashSet<MapVector> _availableVectors;

    //Public for testing purposes
    public int DefaultSizeValue;

    public MazeHuntKillV2(int? seed = null)
    {
        if (seed != null)
        {
            _random = new Random((int)seed);
        }
        else
        {
            _random = new Random();
        }
        DefaultSizeValue = 10;
        _allDirections = Enum.GetValues(typeof(Direction)).Cast<Direction>()
            .Where(direction => direction != Direction.None).ToList();
    }

    public Direction[,] CreateMap(int height, int width)
    {
        if (height < 0 || width < 0)
        {
            throw new ArgumentException("Width and Height must be positive!");
        }

        _availableVectors = new HashSet<MapVector>();
        for (int y = 0; y < height; y++)
        {
            for (int x = 0; x < width; x++)
            {
                _availableVectors.Add(new MapVector(y, x));
            }
        }

        _directionMap = new Direction[height, width];

        _currentPos = new MapVector(_random.Next(0, height), _random.Next(0, width));

        while (true)
        {
            _currentPos = Walk(_currentPos);
            if (_currentPos == null)
            {
                _currentPos = Hunt();
                if (_currentPos == null)
                {
                    return _directionMap;
                }
            }
        }
    }

    private MapVector Walk(MapVector pos)
    {
        List<Direction> possibleDirs = PossibleDirectionsForWalk(pos);

        if (possibleDirs.Count == 0)
        {
            return null;
        }

        Direction randDir = possibleDirs[_random.Next(possibleDirs.Count)];
        Direction oppositeDir = OppositeDir(randDir);

        _directionMap[pos.Y, pos.X] = _directionMap[pos.Y, pos.X] | randDir;
        MapVector nextPos = pos + randDir;
        _directionMap[nextPos.Y, nextPos.X] = _directionMap[nextPos.Y, nextPos.X] | oppositeDir;

        _availableVectors.Remove(nextPos);

        return nextPos;
    }

    private List<Direction> PossibleDirectionsForWalk(MapVector pos)
    {
        List<Direction> possibleDirs = new List<Direction>();

        foreach (Direction dir in _allDirections)
        {
            MapVector possiblePos = dir + pos;
            if (possiblePos.IsValid && possiblePos.Y < _directionMap.GetLength(0) && possiblePos.X < _directionMap.GetLength(1)
                && _directionMap[possiblePos.Y, possiblePos.X] == Direction.None)
            {
                possibleDirs.Add(dir);
            }
        }
        return possibleDirs;
    }

    private MapVector Hunt()
    {
        foreach (MapVector pos in _availableVectors)
        {
            if (_directionMap[pos.Y, pos.X] == Direction.None && ValidHuntPos(pos))
            {
                return pos;
            }
        }

        return null;
    }

    private bool ValidHuntPos(MapVector pos)
    {
        List<Direction> shuffledDirs = Shuffle(_allDirections);
        foreach (Direction dir in shuffledDirs)
        {
            MapVector possiblePos = dir + pos;
            if (possiblePos.IsValid && possiblePos.Y < _directionMap.GetLength(0) && possiblePos.X < _directionMap.GetLength(1)
                && _directionMap[possiblePos.Y, possiblePos.X] != Direction.None)
            {
                Direction oppositeDir = OppositeDir(dir);
                _directionMap[pos.Y, pos.X] = _directionMap[pos.Y, pos.X] | dir;
                _directionMap[possiblePos.Y, possiblePos.X] = _directionMap[possiblePos.Y, possiblePos.X] | oppositeDir;
                return true;
            }
        }
        return false;
    }

    //Fisher-Yates Shuffle (Modified to return a list of directions)
    private List<Direction> Shuffle(List<Direction> dirs)
    {
        List<Direction> directions = new List<Direction>(dirs);
        int n = directions.Count;
        while (n > 1)
        {
            n--;
            int k = _random.Next(n + 1);
            Direction dir = directions[k];
            directions[k] = directions[n];
            directions[n] = dir;
        }
        return directions;
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
