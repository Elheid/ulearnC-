using System;
using System.Collections;
using System.Collections.Generic;

//public class TwoLinkedItem<T>
//{
//    public TwoLinkedItem(T value)
//    {
//        Value = value;
//    }
//    public T Value { get; set; }
//    public TwoLinkedItem<T> Previous { get; set; }
//    public TwoLinkedItem<T> Next { get; set; }
//}
//public class TwoLinkedList<T> : IEnumerable
//{
//    TwoLinkedItem<T> head;
//    TwoLinkedItem<T> tail;
//    public bool IsEmpty { get { return head == null; } }
//    public int Count { get { return count; } }
//    int count;
//    public void Add(T value)
//    {
//        var item = new TwoLinkedItem<T>(value);
//        if (IsEmpty)
//            head = item;
//        else
//        {
//            tail.Next = item;
//            item.Previous = tail;
//        }
//        tail = item;
//        count++;
//    }

//    public void AddFirst(T value)
//    {
//        var item = new TwoLinkedItem<T>(value);
//        item.Next = head;
//        if (IsEmpty)
//            tail = item;
//        else
//            head.Previous = item;
//        head = item;
//        count++;
//    }

//    public void RemoveLast(T value)
//    {
//        if (IsEmpty) throw new InvalidOperationException();
//        var itemToRemove = tail;
//        if (value.Equals(tail.Value))
//            head = itemToRemove;
//        tail = null;
//    }

//    public void RemoveFirst(T value)
//    {
//        var itemToRemove = head;
//        if (IsEmpty) throw new InvalidOperationException();
//        if (value.Equals(head.Value))
//            head.Next = itemToRemove;
//        head = null;
//    }

//    IEnumerator IEnumerable.GetEnumerator()
//    {
//        return ((IEnumerable)this).GetEnumerator();
//    }
//}

namespace LimitedSizeStack;
public class LimitedSizeStack<T>
{
    LinkedList<T> stack = new LinkedList<T>();

    private int MaxSizeOfList;

    public LimitedSizeStack(int undoLimit)
    {
        MaxSizeOfList = undoLimit;
    }

    public void Push(T item)
    {
        if (stack.Count == null || MaxSizeOfList == 0) return;
        if (stack.Count == MaxSizeOfList)
            stack.Remove(stack.First);
        stack.AddLast(item);
    }

    public T Pop()
    {
        var itemToRemove = stack.Last.Value;
        stack.Remove(itemToRemove);
        return itemToRemove;
    }

    public int Count { get { return stack.Count; } }
}