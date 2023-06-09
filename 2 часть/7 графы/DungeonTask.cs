using System;
using System.Collections.Generic;
using System.Linq;

namespace Dungeon;

public class DungeonTask
{
	public static MoveDirection[] FindShortestPath(Map map)
	{
		var start = map.InitialPosition;
		var exit = map.Exit;
        var exits = new Point[] { exit };
        var chests = map.Chests;

        var firstTry = BfsTask.FindPaths(map, start, exits).FirstOrDefault();
        if (CheckBorderCases(chests, firstTry) != null)
            return CheckBorderCases(chests, firstTry);

        var fromStart = BfsTask.FindPaths(map, start, chests);
        var fromExit = BfsTask.FindPaths(map, exit, chests).DefaultIfEmpty();
        var strightToExit = firstTry.ToList();
        strightToExit.Reverse();

        if (fromStart.Count() == 0)
            return ReturnPathInDirections(strightToExit);

        var fromStartToExit = fromStart.Join(fromExit, toFinish => toFinish.Value, fromStart => fromStart.Value, 
            (toFinish, fromStart) => Tuple.Create(fromStart.ToList(),toFinish.ToList()));
        var finalPath = fromStartToExit.OrderBy(paths => paths.Item1.Count + paths.Item2.Count).First();
        finalPath.Item2.Reverse();
        finalPath.Item2.AddRange(finalPath.Item1.Skip(1));
        return ReturnPathInDirections(finalPath.Item2);

    }

    public static MoveDirection ToDirection(Point point, Point prevPoint)
	{
        var dx = prevPoint.X - point.X;
        var dy = prevPoint.Y - point.Y;
        if (dx != 0) 
            return dx > 0 ? MoveDirection.Right: MoveDirection.Left;
        return dy > 0 ? MoveDirection.Down : MoveDirection.Up;
    }

    public static MoveDirection[] ReturnPathInDirections(List<Point> list)
    {
        return list.Zip(list.Skip(1), ToDirection).ToArray();
    }

    public static MoveDirection[]? CheckBorderCases(Point[] chests, SinglyLinkedList<Point> firstTry)
    {
        if (firstTry == null)
            return new MoveDirection[0];

        var strightToExit = firstTry.ToList();
        strightToExit.Reverse();
        if (chests.Any(chest => firstTry.Contains(chest)))
            return strightToExit.Zip(strightToExit.Skip(1), ToDirection).ToArray();
        return null;
    }
}