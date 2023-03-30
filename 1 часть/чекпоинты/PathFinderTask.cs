using System;
using System.Drawing;

namespace RoutePlanning
{
    public static class PathFinderTask
    {
        static double minDistance;
        public static int[] FindBestCheckpointsOrder(Point[] checkpoints)
        {
            var firstPoint = 1;
            var sizeOfWay = checkpoints.Length;
            var bestWay = new int[sizeOfWay];
            var wayToMove = new int[sizeOfWay];
            minDistance = double.MaxValue;
            return SearchBesthWay(checkpoints, bestWay, wayToMove, firstPoint, sizeOfWay);
        }

        public static int[] SearchBesthWay(Point[] checkpoints, int[] bestWay,
                int[] wayToMove, int point, int sizeOfWay)
        {
            var nextPoint = point + 1;
            var distanceOfWay = checkpoints.GetPathLength(wayToMove);
            var endOfWay = point == wayToMove.Length;
            var lessLongWayExist = distanceOfWay < minDistance;
            if (endOfWay && lessLongWayExist)
            {
                ChangeBestWayToNew(checkpoints, bestWay, wayToMove, sizeOfWay);
                return bestWay;
            }

            for (var i = 0; i < sizeOfWay; i++)
            {
                var index = Array.IndexOf(wayToMove, i, 0, point);
                var positionAlreadyExistInWay = index != -1;
                if (positionAlreadyExistInWay)
                    continue;
                wayToMove[point] = i;
                SearchBesthWay(checkpoints, bestWay, wayToMove, nextPoint, sizeOfWay);
            }
            return bestWay;
        }

        public static void ChangeBestWayToNew(Point[] checkpoints, int[] bestWay, int[] wayToMove, int sizeOfWay)
        {
            var distanceOfWay = checkpoints.GetPathLength(wayToMove);
            minDistance = distanceOfWay;
            Array.Copy(wayToMove, bestWay, sizeOfWay);
        }
    }
}
