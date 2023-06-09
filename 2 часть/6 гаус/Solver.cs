using System.Collections.Generic;
using System.Linq;
using System;
using System.Numerics;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Common;
using System.Threading;

namespace GaussAlgorithm
{
    public class Solver
    {
        public double[][] GetExtendedMatrix(double[][] matrix, double[] freeMembers)
        {
            var mLine = matrix.Length;
            var extendedMatrix = new double[mLine][];
            for (var i = 0; i < mLine; i++)
            {
                var temp = new double[matrix[0].Length + 1];
                matrix[i].CopyTo(temp, 0);
                temp[temp.Length - 1] = freeMembers[i];
                extendedMatrix[i] = temp;
            }
            return extendedMatrix;

        }
        public double[] Solve(double[][] matrix, double[] freeMembers)
        {
            var extendedMatrix = GetExtendedMatrix(matrix, freeMembers);
            extendedMatrix = DeleateZeroLines(extendedMatrix);
            if (extendedMatrix.Length == 0)
                return new double[matrix[0].Length];
            var nColumn = extendedMatrix[0].Length;
            var mLine = extendedMatrix.Length;
            var currRow = 0;
            for (var currColumn = 0; currColumn < nColumn; currColumn++)
            {
                double currMaxInRow;
                int currMaxIndex;
                (currMaxInRow, currMaxIndex) = SearchMaxInRow(extendedMatrix, currColumn);
                if (currMaxInRow < 1e-3)
                    continue;
                var temp = extendedMatrix[currRow];
                extendedMatrix[currRow] = extendedMatrix[currMaxIndex];
                extendedMatrix[currMaxIndex] = temp;

                ReductionToZero(extendedMatrix, currRow, currColumn);
                currRow++;
            }
            extendedMatrix = DeleateZeroLines(extendedMatrix);
            var matrixIsSquare = extendedMatrix[0].Length - 1 == extendedMatrix.Length;
            if (matrixIsSquare)
                return CalculateOneDecisionSystem(extendedMatrix);
            return CalculateInfDecisionSystem(extendedMatrix);
        }

        public void ReductionToZero (double[][] matrix, int row, int column)
        {
            for (var currLine = 0; currLine < matrix.Length; currLine++)
            {
                if (currLine != row)
                {
                    var coef = matrix[currLine][column] / matrix[row][column];
                    for (var c = 0; c < matrix[0].Length; c++)
                    {
                        matrix[currLine][c] -= coef * matrix[row][c];
                    }
                }
            }
        }

        public (double, int) SearchMaxInRow(double[][] matrix, int column)
        {
            if (column < matrix.Length)
            {
                var maxRows = column != 0 ? matrix.Select(row => row.Skip(column).Take(column).First()).ToList() : matrix.Select(row => row[0]).ToList();
                var list = maxRows.Select(x => Math.Abs(x));
                var max = column != 0 ? list.Skip(column).Max() : list.Max();
                var maxIndex = list.ToList().IndexOf(max);
                return (max, maxIndex);
            }
            return (double.MinValue, -1);
        }

        public double[][] DeleateZeroLines(double[][] matrix)
        {
            var noZeroLines = new List<double[]>();
            foreach (var line in matrix)
            {
                var _matrix = line.Take(line.Length - 1);
                var freeMembers = Math.Abs(line.Last());
                if (_matrix.All(x => Math.Abs(x) < 1e-3) && Math.Abs(line.Last()) > 1e-3)
                    throw new NoSolutionException("");
                if (line.All(x => Math.Abs(x) < 1e-3))
                    continue;
                noZeroLines.Add(line);
            }
            return noZeroLines.ToArray();
        }

        private double CalculateValue(double[] line, double[] descision, int lineIndex)
        {
            var j = lineIndex;
            var val = line.Select(x => x * descision[j++]);
            return val.Any(x => x == null) ? 0 : val.Sum(x => x);
        }
    

        private double[] CalculateOneDecisionSystem(double[][] matrix)
        {
            int line = matrix.Length;
            int column = matrix[0].Length;
            var descision = new double[column+20];
            for (int k = line - 1; k >= 0; k--)
            {
                var value = CalculateValue(matrix[k], descision, k);
                descision[k] = (matrix[k][line] - value) / matrix[k][k];
            }
            return descision;
        }

        private double[] CalculateInfDecisionSystem(double[][] matrix)
        {
            int line = matrix.Length;
            int column = matrix[0].Length;
            var descision = new double[column + 20];
            int numberOfFreeDescisions = (column - 1) - line;
            int firstNotZeroLine = 0;
            for (int k = line - 1; k >= 0; k--)
            {
                for (int x = 0; x < column - 1; x++)
                {
                    if (Math.Abs(matrix[k][x]) > 1e-3)
                    {
                        firstNotZeroLine = x;
                        if (numberOfFreeDescisions > 0)
                        {
                            numberOfFreeDescisions -= (column - 1 - x);
                            for (int y = column - 1; y > x; y--)
                            {
                                if (descision[y] == null)
                                    descision[y] = 0;
                            }
                        }
                        break;
                    }
                }
                var value = CalculateValue(matrix[k], descision, firstNotZeroLine + 1);
                descision[firstNotZeroLine] = (matrix[k][column - 1] - value) / matrix[k][firstNotZeroLine];
            }

            return descision;
        }
    }
}