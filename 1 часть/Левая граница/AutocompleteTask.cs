using System;
using System.Collections.Generic;
using NUnit.Framework;

namespace Autocomplete
{
    internal class AutocompleteTask
    {
        static int firstIndex = -1;
        /// <returns>
        /// Возвращает первую фразу словаря, начинающуюся с prefix.
        /// </returns>
        /// <remarks>
        /// Эта функция уже реализована, она заработает, 
        /// как только вы выполните задачу в файле LeftBorderTask
        /// </remarks>
        public static string FindFirstByPrefix(IReadOnlyList<string> phrases, string prefix)
        {
            var index = LeftBorderTask.GetLeftBorderIndex(phrases, prefix, firstIndex, phrases.Count) + 1;
            if (index < phrases.Count && phrases[index].StartsWith(prefix, StringComparison.OrdinalIgnoreCase))
                return phrases[index];
            return null;
        }

        /// <returns>
        /// Возвращает первые в лексикографическом порядке count (или меньше, если их меньше count) 
        /// элементов словаря, начинающихся с prefix.
        /// </returns>
        /// <remarks>Эта функция должна работать за O(log(n) + count)</remarks>
        public static string[] GetTopByPrefix(IReadOnlyList<string> phrases, string prefix, int count)
        {
            var countOfPhrases = GetCountByPrefix(phrases, prefix);
            var leftBorder = LeftBorderTask.GetLeftBorderIndex(phrases, prefix, firstIndex, phrases.Count) + 1;
            countOfPhrases = countOfPhrases > count ? count : countOfPhrases;
            var wordsToShow = new string[countOfPhrases];
            for (int i = 0; i < countOfPhrases; i++)
            {
                var similarWord = phrases[leftBorder + i];
                wordsToShow[i] = similarWord;
            }
            return wordsToShow;
        }

        /// <returns>
        /// Возвращает количество фраз, начинающихся с заданного префикса
        /// </returns>
        public static int GetCountByPrefix(IReadOnlyList<string> phrases, string prefix)
        {
            var rightBorder = RightBorderTask.GetRightBorderIndex(phrases, prefix, firstIndex, phrases.Count);
            var leftBorder = LeftBorderTask.GetLeftBorderIndex(phrases, prefix, firstIndex, phrases.Count);
            var countOfPhrases = rightBorder - leftBorder + firstIndex;
            if (countOfPhrases <= 0)
                return 0;
            return countOfPhrases;
        }
    }
    //тестирование не проходил, оставил по умолчанию
    [TestFixture]
    public class AutocompleteTests
    {
        [Test]
        public void TopByPrefix_IsEmpty_WhenNoPhrases()
        {
            // ...
            //CollectionAssert.IsEmpty(actualTopWords);
        }

        // ...

        [Test]
        public void CountByPrefix_IsTotalCount_WhenEmptyPrefix()
        {
            // ...
            //Assert.AreEqual(expectedCount, actualCount);
        }

        // ...
    }
}
