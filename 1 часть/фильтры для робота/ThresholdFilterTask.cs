using System.Collections.Generic;

namespace Recognizer
{
    public static class ThresholdFilterTask
    {
        public static double[,] ThresholdFilter(double[,] original, double whitePixelsFraction)
        {
            var pixelsInX = original.GetLength(0);
            var pixelsInY = original.GetLength(1);
            var amountOfPixels = pixelsInX * pixelsInY;
            double thresholdValueT = ((int)(amountOfPixels * whitePixelsFraction));
            thresholdValueT = GainThresholdValueT(thresholdValueT, amountOfPixels, original);

            for (int x = 0; x < pixelsInX; x++)
                for (var y = 0; y < pixelsInY; y++)
                {
                    var filter = original[x, y] >= thresholdValueT ? 1.0 : 0.0;
                    original[x, y] = filter;
                }       
            return original;
        }
        static double GainThresholdValueT(double thresholdValueT, int amountOfPixels, double[,] original)
        {
            var pixelsInX = original.GetLength(0);
            var pixelsInY = original.GetLength(1);
            var thresholdFilter = new List<double>(amountOfPixels);
            for (int x = 0; x < pixelsInX; x++)
                for (var y = 0; y < pixelsInY; y++)
                    thresholdFilter.Add(original[x,y]);
            thresholdFilter.Sort();
            if (thresholdValueT > 0 && thresholdValueT <= thresholdFilter.Count)
                return thresholdFilter[(thresholdFilter.Count - (int)thresholdValueT)];
            else
                return double.MaxValue;
        }
    }
}
