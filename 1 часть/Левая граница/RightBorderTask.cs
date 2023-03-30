using NUnit.Framework.Constraints;
using System;
using System.Collections.Generic;


namespace Autocomplete
{
    public class RightBorderTask
    {
        /// <returns>
        /// Возвращает индекс правой границы. 
        /// То есть индекс минимального элемента, который не начинается с prefix и большего prefix.
        /// Если такого нет, то возвращает items.Length
        /// </returns>
        /// <remarks>
        /// Функция должна быть НЕ рекурсивной
        /// и работать за O(log(items.Length)*L), где L — ограничение сверху на длину фразы
        /// </remarks>
        public static int GetRightBorderIndex(IReadOnlyList<string> phrases, string prefix, int left, int right)
        {
            if (phrases.Count == 0) 
                return right;
            
            while (left < right)
            {
                int mid = left + (right - left) / 2;

                if (string.Compare(prefix, phrases[mid], StringComparison.OrdinalIgnoreCase) >= 0
                    || phrases[mid].StartsWith(prefix, StringComparison.OrdinalIgnoreCase))
                    left = mid + 1;
                else
                    right = mid - 1;
            }

            if (string.Compare(prefix, phrases[right], StringComparison.OrdinalIgnoreCase) >= 0
                         || phrases[left].StartsWith(prefix, StringComparison.OrdinalIgnoreCase))
                return right + 1;
            else
                return right;
        }
    }
}
/*
var left = 0;
var right = array.Length - 1;
while (left < right)
{
    var middle = (right + left) / 2;
    if (element <= array[middle])
        right = middle;
    else left = middle + 1;
}
if (array[right] == element)
    return right;
return -1;
}

            while (right - left > 1)
            {
                var middle = left + (right - left) / 2;
                if (string.Compare(prefix, phrases[middle], StringComparison.OrdinalIgnoreCase) >= 0
                   || phrases[middle].StartsWith(prefix))
                    left = middle;
                else right = middle;
            }
            return right;
        }
    }
}

            if (phrases.Count == 0) 
                return right;
            while (right - left > 1)
            {
                var middle = left + (right - left) / 2;
                if (string.Compare(prefix, phrases[middle], StringComparison.OrdinalIgnoreCase) >= 0
                    || phrases[middle].StartsWith(prefix, StringComparison.OrdinalIgnoreCase))
                    right = middle - 1;
                left = middle + 1;
            }
            if (string.Compare(prefix, phrases[right], StringComparison.OrdinalIgnoreCase) >= 0
                    || phrases[right].StartsWith(prefix, StringComparison.OrdinalIgnoreCase))
                return right + 1;
            return right;
*/