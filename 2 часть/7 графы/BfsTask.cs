using DynamicData;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Dungeon;

public class BfsTask
{
	public static IEnumerable<SinglyLinkedList<Point>> FindPaths(Map map, Point start, Point[] chests)
    {
        var chestsSet = new HashSet<Point>();
        foreach (var chest in chests)
            chestsSet.Add(chest);
        var visitedPoints = new HashSet<Point>();
        var queueOfPaths = new Queue<SinglyLinkedList<Point>>();
        visitedPoints.Add(start);
        queueOfPaths.Enqueue(new SinglyLinkedList<Point>(start, null));
        while (queueOfPaths.Count > 0) 
        { 
            var pathOfPoints = queueOfPaths.Dequeue();
            var point = pathOfPoints.Value;
            if (!map.InBounds(point)) continue;
            if (map.Dungeon[point.X, point.Y] == 0) continue;
            if (chestsSet.Contains(point)) yield return pathOfPoints;
            var nodes = Walker.PossibleDirections
            .Select(newPoint => point + newPoint);
            AddPointsToPath(pathOfPoints, visitedPoints, queueOfPaths);
        }
    }
    private static void AddPointsToPath(SinglyLinkedList<Point> pathOfPoints, HashSet<Point> visited, Queue<SinglyLinkedList<Point>> queueOfPaths)
    {
        var nodes = Walker.PossibleDirections
            .Select(newPoint => pathOfPoints.Value + newPoint);
        foreach (var nextPoint in nodes)
        {
            if (!visited.Contains(nextPoint))
            {
                queueOfPaths.Enqueue(new SinglyLinkedList<Point>(nextPoint, pathOfPoints));
                visited.Add(nextPoint);
            }
        }
    }
}