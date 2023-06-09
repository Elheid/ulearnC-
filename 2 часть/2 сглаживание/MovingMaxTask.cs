using Avalonia.Controls;
using Avalonia.Controls;
using Newtonsoft.Json.Linq;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;


namespace yield;

public static class QueueExtension
{
    public static void EnqueueAndChangeList(this Queue<double> queue,
                double cellValue, int windowWidth, LinkedList<double> list)
    {
        queue.Enqueue(cellValue);
        if (queue.Count > windowWidth && queue.Dequeue() >= list.First.Value)
        {
            list.RemoveFirst();
        }
    }
}

public static class LinkedListExtension
{
    public static void PushPossibleMax(this LinkedList<double> list, double value)
    {
        if (list.Count == 0)
            list.AddFirst(value);
        else
        {
            while (list.Count > 0 && value > list.Last.Value)
                list.RemoveLast();
            list.AddLast(value);
        }
    }
}

public static class MovingMaxTask
{
    public static IEnumerable<DataPoint> MovingMax(this IEnumerable<DataPoint> data, int windowWidth)
    {
        var queueOfCells = new Queue<double>();
        var listOfMax = new LinkedList<double>();
        foreach (var windowCell in data)
        {
            var cellValue = windowCell.OriginalY;
            queueOfCells.EnqueueAndChangeList(cellValue, windowWidth, listOfMax);
            listOfMax.PushPossibleMax(cellValue);
            var maxValue = windowCell.WithMaxY(listOfMax.First.Value);
            yield return maxValue;
        }
    }
}
