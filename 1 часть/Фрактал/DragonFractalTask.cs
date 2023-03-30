using System;
using System.Drawing;


namespace Fractals
{
    internal static class DragonFractalTask
    {
        enum Sign
        {
            Minus,
            Plus
        }

        static double x;
        static double y;
        public static void DrawDragonFractal(Pixels pixels, int iterationsCount, int seed)
        {
            x = 1;
            y = 0;
            pixels.SetPixel(1, 0);
            var angle = Math.PI / 4;
            var randomNumber = new Random(seed);
            for (int i = 0; i < iterationsCount; i++)
            {
                var coofficientOfShift = randomNumber.Next(2);
                var angleChanging = coofficientOfShift * (Math.PI / 2);
                Paint(pixels, angleChanging + angle, coofficientOfShift);
            }
        }

        static void Paint(Pixels pixels, double angle, int shift)
        {
            var newX = TransformCoordinate(angle,  Sign.Minus) + shift;
            var newY = TransformCoordinate(angle,  Sign.Plus);
            x = newX;
            y = newY;
            pixels.SetPixel(x, y);
        }

        static double TransformCoordinate(double angle, Sign signOfequation)
        {
            if (signOfequation == Sign.Minus)
                return ((x) * Math.Cos(angle) - (y) * Math.Sin(angle)) / Math.Sqrt(2);
            return ((x) * Math.Sin(angle) + (y) * Math.Cos(angle)) / Math.Sqrt(2);
        }
    }
}
