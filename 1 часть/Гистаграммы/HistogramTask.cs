using System;
using System.Linq;
using System.Xml.Linq;

namespace Names
{
    internal static class HistogramTask
    {
        const int UnknownBirthDate = 1;
        const int NumberOfDays = 31;
        public static HistogramData GetBirthsPerDayHistogram(NameData[] names, string name)
        {
            string[] days = new string[NumberOfDays];
            var birthCount = new double[NumberOfDays];
            CountNumberOfDays(days);
            CheckAmountOfBirthsByName(birthCount, name, names);

            return new HistogramData("Рождаемость по дням", days, birthCount);
        }

        static void CheckAmountOfBirthsByName(double[] birthCount,string name, NameData[] namesList)
        {
            foreach (var human in namesList)
            {
                if ((human.Name != name) || (human.BirthDate.Day == UnknownBirthDate))
                    continue;
                else
                    birthCount[human.BirthDate.Day - UnknownBirthDate]++;
            }
        }

        static void CountNumberOfDays(string[] days)
        {
            for (var i = 0; i < NumberOfDays; i++)
            {
                var dayInRow = i + 1;
                days[i] = (dayInRow).ToString();
            }
        }
    }
}