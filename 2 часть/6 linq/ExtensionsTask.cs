using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;

namespace linq_slideviews;

public static class ExtensionsTask
{
	/// <summary>
	/// Медиана списка из нечетного количества элементов — это серединный элемент списка после сортировки.
	/// Медиана списка из четного количества элементов — это среднее арифметическое 
    /// двух серединных элементов списка после сортировки.
	/// </summary>
	/// <exception cref="InvalidOperationException">Если последовательность не содержит элементов</exception>
	public static double Median(this IEnumerable<double> items)
	{
		if (items.Count() == 0) throw new InvalidOperationException();
		var listOfAllItems = items.ToList();
		listOfAllItems.Sort();
		if (listOfAllItems.Count() % 2 == 0)
			return (listOfAllItems[listOfAllItems.Count / 2] + listOfAllItems[listOfAllItems.Count / 2-1]) / 2;
		return listOfAllItems[listOfAllItems.Count / 2];

    }

    /// <returns>
    /// Возвращает последовательность, состоящую из пар соседних элементов.
    /// Например, по последовательности {1,2,3} метод должен вернуть две пары: (1,2) и (2,3).
    /// </returns>
	public static IEnumerable<(T First, T Second)> Bigrams<T>(this IEnumerable<T> items) 
	{
        T prevItem = default(T);
        var firstTry = true;
        var result = items.Select(item =>
        {
            var res = new Tuple<T, T>(prevItem, item);
            prevItem = item;          
            return (res.Item1, res.Item2);
        });
        foreach (var res in result)
        {
            if (firstTry)
            {
                firstTry = false;
                continue;
            }
            yield return res;
        }
    }
}