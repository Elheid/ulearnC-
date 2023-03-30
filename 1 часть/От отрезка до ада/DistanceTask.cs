using System;

namespace DistanceTask
{
	public static class DistanceTask
	{
		// Расстояние от точки O (x, y) до отрезка AB с координатами A(ax, ay), B(bx, by)
		public static double GetDistanceToSegment(double ax, double ay, double bx, double by, double x, double y)
		{
            var segmentAO = CalculateDistanceOfPoints(ax, x, ay, y);
            var segmentBO = CalculateDistanceOfPoints(bx, x, by, y);
            var segmentAB = CalculateDistanceOfPoints(ax, bx, ay, by);
            var semiPerimetr = (segmentAO + segmentBO + segmentAB) / 2;
            // если точка лежит на отрезке
            if ((ax == x && ay == y) || (bx == x && by == y))
                return 0;
            // если отрезок - точка
            else if (segmentAB == 0) 
                return segmentAO;
            // условия, когда минимальное растояние можно провести из одного из концов отрезка
            else if ((CheckObtuseAngle(segmentBO, segmentAB, segmentAO) || (CheckObtuseAngle(segmentAO, segmentAB, segmentBO))))
                return Math.Min(segmentBO, segmentAO);
            else
            return 2 * Math.Sqrt(semiPerimetr * (semiPerimetr - segmentAB) * (semiPerimetr - segmentBO) * (semiPerimetr - segmentAO)) / segmentAB;
        }
        public static double CalculateDistanceOfPoints(double x1, double x2, double y1, double y2)
        {
            return Math.Sqrt(Math.Pow((x2 - x1),2) + Math.Pow((y2 - y1),2));
        }
        public static bool CheckObtuseAngle(double trinagleSideA, double trinagleSideB, double trinagleSideC)
        {
            return ((trinagleSideA * trinagleSideA + trinagleSideB * trinagleSideB < trinagleSideC * trinagleSideC));
        }

    }
}