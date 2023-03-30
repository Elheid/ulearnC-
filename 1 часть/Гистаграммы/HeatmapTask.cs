using System;
using System.Security.Cryptography;
using System.Xml.Linq;

namespace Names
{
    internal static class HeatmapTask
    {
        const int NumberOfDays = 30;
        const int NumberOfMonths = 12;
        const int FirstNumberOfDays = 2;
        const int FirstNumberOfMonths = 1;
        public static HeatmapData GetBirthsPerDateHeatmap(NameData[] names)
        {
            var days = new string[NumberOfDays];
            var months = new string[NumberOfMonths];
            var heatMap = new double[NumberOfDays, NumberOfMonths];

            FillingOutHeatMap(names, heatMap);
            CountAmountInTimeInterval(days, NumberOfDays, FirstNumberOfDays);
            CountAmountInTimeInterval(months, NumberOfMonths, FirstNumberOfMonths);

            return new HeatmapData("Тепловая карта рождаемости по дням", heatMap, days, months);
        }
        static void CountAmountInTimeInterval(string[] timeInterval, int amountOfPeriodsInInterval, int firstNumberOfTimeInterval)
        {
            for (var i = 0; i < amountOfPeriodsInInterval; i++)
            {
                var numberInTimeInterval = firstNumberOfTimeInterval + i;
                timeInterval[i] = (numberInTimeInterval).ToString();
            }
        }
        static void FillingOutHeatMap(NameData[] names, double[,] heatMap)
        {
            foreach (var human in names)
            {
                if (human.BirthDate.Day != 1)
                {
                    var amountOfDays = human.BirthDate.Day - FirstNumberOfDays;
                    var amountOfMonths = human.BirthDate.Month - FirstNumberOfMonths;
                    heatMap[amountOfDays, amountOfMonths]++;
                }
            }
        }
    }
}