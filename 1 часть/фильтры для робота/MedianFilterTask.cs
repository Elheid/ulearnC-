using System;
using System.Collections.Generic;

 
namespace Recognizer
{
    internal static class MedianFilterTask
    {
        /* 
* Для борьбы с пиксельным шумом, подобным тому, что на изображении,
* обычно применяют медианный фильтр, в котором цвет каждого пикселя, 
* заменяется на медиану всех цветов в некоторой окрестности пикселя.
* https://en.wikipedia.org/wiki/Median_filter
* 
* Используйте окно размером 3х3 для не граничных пикселей,
* Окно размером 2х2 для угловых и 3х2 или 2х3 для граничных.
*/
        //посмотреть как сделать с try catch try { pixels.Add(image[i, j]); } catch { }
        /*                   var result = new List<List<int>>();
            for (var i = x - 1; i <= x + 1; i++)
            {
                for (var j = y - 1; j <= y + 1; j++)
                {
                    try { result.Add(new List<int> { i, j }); } catch { }
                }
            }
            return result;


        */

        public static double[,] MedianFilter(double[,] original)
        {
            var pixelsInX = original.GetLength(0);
            var pixelsInY = original.GetLength(1);
            var medianFilter = new double[pixelsInX, pixelsInY];

            for (var x = 0; x < pixelsInX; x++)
            {
                for (var y = 0; y < pixelsInY; y++)
                {

                    var listOfBoundaryPixels = new List<double>();
                    FilWithWindowsOfBoundaryPixels(x, y, listOfBoundaryPixels, original);
                    listOfBoundaryPixels.Sort();
                    var medianValue = SearchMedianValue(listOfBoundaryPixels);
                    medianFilter[x, y] = medianValue;
                }
            }
            return medianFilter;
        }

        static void FilWithWindowsOfBoundaryPixels(int x, int y, List<double> listOfBoundaryPixels, double[,] original)
        {
            var leftPixel = x - 1;
            var higherPixel = y - 1;
            var pixelsAroundIncludeOnX = x + 2;
            var pixelsAroundIncludeOnY = y + 2;
            for (var i = leftPixel; i < pixelsAroundIncludeOnX; i++)
                for (var j = higherPixel; j < pixelsAroundIncludeOnY; j++)
                    try
                    {
                        listOfBoundaryPixels.Add(original[i, j]);
                    }
                    catch { }
        }

        static double SearchMedianValue(List<double> listOfBoundaryPixels)
        {
            //нужно и можно ли называть bool вопросом?
            var amountOfBoundaryPixelsIsEven = listOfBoundaryPixels.Count % 2 == 0;
            var firstAverageValue = listOfBoundaryPixels[listOfBoundaryPixels.Count / 2];
            var secondAverageValue = amountOfBoundaryPixelsIsEven ? listOfBoundaryPixels[listOfBoundaryPixels.Count / 2 - 1] : 0;
            var averageValue = ((secondAverageValue + firstAverageValue) / 2);
            if (amountOfBoundaryPixelsIsEven)
                return averageValue;
            else
                return firstAverageValue;
        }
    }
}