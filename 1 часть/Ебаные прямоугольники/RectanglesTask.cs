using System;

namespace Rectangles
{
	public static class RectanglesTask
	{
		public static bool AreIntersected(Rectangle r1, Rectangle r2)
		{
			var leftMatchesOrOverRight = r1.Left <= r2.Right;
			var rigntMatchesOrBelowLeft = r1.Right >= r2.Left;
			var bottomMatchesOrBelowTop = r1.Bottom >= r2.Top;
			var topMatchesOrOverBottom = r1.Top <= r2.Bottom;
            return leftMatchesOrOverRight && rigntMatchesOrBelowLeft && bottomMatchesOrBelowTop && topMatchesOrOverBottom;
        }

		public static int IntersectionSquare(Rectangle r1, Rectangle r2)
		{
			var areaOfCross = (Math.Min(r1.Right, r2.Right) - Math.Max(r1.Left, r2.Left)) * (Math.Min(r1.Bottom, r2.Bottom) - Math.Max(r1.Top, r2.Top));
            return AreIntersected(r1, r2) ? (areaOfCross) : 0;
        }

		public static int IndexOfInnerRectangle(Rectangle r1, Rectangle r2)
		{
			var leftMatchesOrBelow = r1.Left >= r2.Left;
            var leftMatchesOrOver = r1.Left <= r2.Left;
			var rigntMatchesOrBelow = r1.Right >= r2.Right;
            var rigntMatchesOrOver = r1.Right <= r2.Right;
            var bottomMatchesOrBelow = r1.Bottom >= r2.Bottom;
            var bottomMatchesOrOver = r1.Bottom <= r2.Bottom;
            var topMatchesOrOver = r1.Top <= r2.Top;
            var topMatchesOrBellow = r1.Top >= r2.Top;
            if (leftMatchesOrBelow && rigntMatchesOrOver && topMatchesOrBellow && bottomMatchesOrOver)
                return 0;
            else if (leftMatchesOrOver && rigntMatchesOrBelow && topMatchesOrOver && bottomMatchesOrBelow)
                return 1;
            else
                return -1;
        }
	}
}