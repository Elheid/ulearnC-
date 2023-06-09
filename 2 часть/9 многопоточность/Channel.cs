using DynamicData;
using System;
using System.Collections.Generic;
using System.Linq;

namespace rocket_bot;

public class Channel<T> where T : class
{
	private List<T> list = new List<T> ();
	private int count;
    /// <summary>
    /// Возвращает элемент по индексу или null, если такого элемента нет.
    /// При присвоении удаляет все элементы после.
    /// Если индекс в точности равен размеру коллекции, работает как Append.
    /// </summary>
    public T this[int index]
	{
		get
		{
			if (index < 0 || index >= Count) return null;
			return list.ElementAt(index);
		}
		set
		{
            if (index < 0 || index > Count) throw new IndexOutOfRangeException();
			if (index == Count) lock (list) { list.Add(value); count++; }
			else
			{
                lock (list)
				{
                    list[index] = value;
                    list.RemoveRange(index+1, Count-index-1);
					count = index+1;
                }
            }
        }
	}

	/// <summary>
	/// Возвращает последний элемент или null, если такого элемента нет
	/// </summary>
	public T LastItem()
	{
		lock(list) 
		{
			if (Count == 0 || list[list.Count - 1] == null) return null;
            return list[list.Count - 1];
        }
	}

	/// <summary>
	/// Добавляет item в конец только если lastItem является последним элементом
	/// </summary>
	public void AppendIfLastItemIsUnchanged(T item, T knownLastItem)
	{
		lock (list)
		{ 
			if (knownLastItem == LastItem())
			{
                list.Add(item);
				count++;
            }
		}
	}

	/// <summary>
	/// Возвращает количество элементов в коллекции
	/// </summary>
	public int Count
	{
		get
		{
			return count;
		}
	}
}