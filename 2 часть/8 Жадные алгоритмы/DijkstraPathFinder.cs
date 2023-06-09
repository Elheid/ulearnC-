using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Avalonia.Controls.Primitives;
using Greedy.Architecture;


namespace Greedy
{
    public class DijkstraData
    {
        public Point Previous { get; set; }
        public int Price { get; set; }
    }
    public class DijkstraPathFinder
    {
        public IEnumerable<PathWithCost> GetPathsByDijkstra(State state, Point start, IEnumerable<Point> targets)
        {

            var chestSet = new HashSet<Point>();
            var notVisitedPoints = new HashSet<Point>();
            var visitedPoints = new HashSet<Point>();
            var data = new DijkstraData() { Previous = new Point(-1, -1), Price = 0 };
            var track = new Dictionary<Point, DijkstraData>();
            foreach (var target in targets)
            {
                chestSet.Add(target);
            }
            track.Add(start, data);
            notVisitedPoints.Add(start);

            while (true)
            {
                var toOpen = new Point(-1, -1);
                var bestPrice = int.MaxValue;
                foreach (var point in notVisitedPoints)
                    if (track.ContainsKey(point) && track[point].Price < bestPrice)
                    {
                        bestPrice = track[point].Price;
                        toOpen = point;
                    }
                if (toOpen == new Point(-1, -1)) yield break;
                if (chestSet.Contains(toOpen))
                {
                    chestSet.Remove(toOpen);
                    yield return GetPath(track, toOpen);
                }
                RecordToTrack(state, toOpen, track, visitedPoints, notVisitedPoints);
            }
        }

        void RecordToTrack(State state, Point toOpen, Dictionary<Point, DijkstraData> track, HashSet<Point> visitedPoints, HashSet<Point> notVisitedPoints)
        {
            var nextPoints = GetNextPoints(state, toOpen);
            foreach (var nextPoint in nextPoints)
            {
                var currentPrice = track[toOpen].Price + state.CellCost[nextPoint.X, nextPoint.Y];
                if (!track.ContainsKey(nextPoint) || track[nextPoint].Price > currentPrice)
                {
                    track[nextPoint] = new DijkstraData { Previous = toOpen, Price = currentPrice };
                }
                if (!visitedPoints.Contains(nextPoint))
                    notVisitedPoints.Add(nextPoint);
            }
            notVisitedPoints.Remove(toOpen);
            visitedPoints.Add(toOpen);
        }

        PathWithCost GetPath(Dictionary<Point, DijkstraData> track, Point point)
        {
            var result = new List<Point>();
            var currPoint = point;
            while (currPoint != new Point(-1, -1))
            {
                result.Add(currPoint);
                currPoint = track[currPoint].Previous;
            }
            result.Reverse();
            var path = new PathWithCost(track[point].Price, result.ToArray());
            return path;
        }

        static List<Point> GetNextPoints(State state, Point currPoint)
        {
            var result = new List<Point>();
            for (var dx = -1; dx <= 1; dx++)
            {
                for (var dy = -1; dy <= 1; dy++)
                {
                    var nextPoint = new Point(currPoint.X + dx, currPoint.Y + dy);
                    if (dx != 0 && dy != 0) continue;
                    if (!state.InsideMap(nextPoint)) continue;
                    if (state.IsWallAt(nextPoint)) continue;
                    result.Add(nextPoint);
                }
            }
            return result;
        }
    }
}