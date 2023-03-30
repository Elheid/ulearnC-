using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Text;

namespace RefactorMe
{
    class Drawing
    {
        static float x, y;
        static Graphics canvas;

        public static void Initialize(Graphics newCanvas)
        {
            canvas = newCanvas;
            canvas.SmoothingMode = SmoothingMode.None;
            canvas.Clear(Color.Black);
        }

        public static void SetPosition(float x0, float y0)
        {
            x = x0;
            y = y0;
        }

        public static void DrawLine(double length, double angle)
        {
            var x1 = (float)(x + length * Math.Cos(angle));
            var y1 = (float)(y + length * Math.Sin(angle));
            canvas.DrawLine(Pens.Yellow, x, y, x1, y1);
            x = x1;
            y = y1;
        }

        public static void Shift(double length, double angle)
        {
            x = (float)(x + length * Math.Cos(angle));
            y = (float)(y + length * Math.Sin(angle));
        }
    }

    public class ImpossibleSquare
    {
        const float LengthEdge = 0.375f;
        const float ThickEdge = 0.04f;
        public static void Draw(int weight, int height, double turnAngle, Graphics canvas)
        {
            // turnAngle пока не используется, но будет использоваться в будущем
            Drawing.Initialize(canvas);

            var sideSize = Math.Min(weight, height);

            var x0=SetPosition(weight, sideSize);
            var y0= SetPosition(height, sideSize);

            Drawing.SetPosition(x0, y0);

            DrawSquare(sideSize);
        }
        static void DrawSide(double sideSize, double variable)
        {
            Drawing.DrawLine(sideSize * LengthEdge, variable);
            Drawing.DrawLine(sideSize * ThickEdge * Math.Sqrt(2), variable + Math.PI / 4);
            Drawing.DrawLine(sideSize * LengthEdge, variable + Math.PI);
            Drawing.DrawLine(sideSize * LengthEdge - sideSize * ThickEdge, variable + Math.PI / 2);

            Drawing.Shift(sideSize * ThickEdge, variable + -Math.PI);
            Drawing.Shift(sideSize * ThickEdge * Math.Sqrt(2), variable + 3 * Math.PI / 4);
        }
        static float SetPosition(int quantity, double sideSize)
        {
            var diagonal = Math.Sqrt(2) * (sideSize * LengthEdge + sideSize * ThickEdge) / 2;
            return (float)(diagonal * Math.Sin(Math.PI / 4 + Math.PI)) + quantity / 2f;
        }

        static void DrawSquare(double sideSize)
        {
            DrawSide(sideSize, 0);
            DrawSide(sideSize, -Math.PI / 2);
            DrawSide(sideSize, Math.PI);
            DrawSide(sideSize, Math.PI / 2);
        }
    }
}