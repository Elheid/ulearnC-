using System;
using System.Collections.Generic;


namespace Recognizer
{
    internal static class SobelFilterTask
    {
        public static double[,] SobelFilter(double[,] original, double[,] sx)
        {
            var pixelsInX = original.GetLength(0);
            var pixelsInY = original.GetLength(1);
            var valuesInX = sx.GetLength(0);
            var valuesInY = sx.GetLength(1);
            var sobelFilter = new double[pixelsInX, pixelsInY];

            var radiusOfSearchX = valuesInX / 2;
            var radiusOfSearchY = valuesInY / 2;
            var xBorder = pixelsInX - radiusOfSearchX;
            var yBorder = pixelsInY - radiusOfSearchY;
            for (int x = radiusOfSearchX; x < xBorder; x++)
                for (int y = radiusOfSearchX; y < yBorder; y++)
                {
                    var sy = new double[valuesInY, valuesInX];
                    TransposeMatrix(valuesInX, valuesInY, sx, sy);
                    var matrixOfBoundaryValues = new double[valuesInX, valuesInX];
                    matrixOfBoundaryValues = FilWithBoundaryValues(x, y, matrixOfBoundaryValues,
                        original, radiusOfSearchY);
                    var gx = MultiplyMatrix(matrixOfBoundaryValues, sx, valuesInX);
                    var gy = MultiplyMatrix(matrixOfBoundaryValues, sy, valuesInX);
                    sobelFilter[x, y] = Math.Sqrt(gx * gx + gy * gy);
                }
            return sobelFilter;
        }

        static void TransposeMatrix(int valuesInX, int valuesInY,
                    double[,] matrixOriginal, double[,] matrixWitchTranspose)
        {
            for (int i = 0; i < valuesInX; i++)
            {
                for (int j = 0; j < valuesInY; j++)
                {
                    matrixWitchTranspose[i, j] = matrixOriginal[j, i];
                }
            }
        }

        static double MultiplyMatrix(double[,] firstMatrix, double[,] secondMatrix, int valuesInX)
        {
            var resultOfMultiplication = 0.0;
            for (int i = 0; i < valuesInX; i++)
            {
                for (int j = 0; j < valuesInX; j++)
                {
                    resultOfMultiplication += firstMatrix[i, j] * secondMatrix[j, i];
                }
            }
            return resultOfMultiplication;
        }

        static double[,] FilWithBoundaryValues(int x, int y, double[,] matrixOfBoundaryValues,
                double[,] original, int radiusOfSearch)
        {
            var leftValue = x - radiusOfSearch;
            var higherValue = y - radiusOfSearch;
            var valuesAroundIncludeOnX = x + radiusOfSearch;
            var valuesAroundIncludeOnY = y + radiusOfSearch;
            for (int i = leftValue; i <= valuesAroundIncludeOnX; i++)
            {
                for (int j = higherValue; j <= valuesAroundIncludeOnY; j++)
                {
                    try
                    {
                        matrixOfBoundaryValues[i - leftValue, j - higherValue] = original[i, j];
                    }
                    catch { };
                }
            }
            return matrixOfBoundaryValues;
        }
    }
}