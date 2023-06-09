using System.Collections.Generic;

namespace yield;
public static class QueueExtension2
{
    static double summOfQueuePoints;
    public static double EnqueueAndReturnSumm(this Queue<double> queue, double cellValue, int windowWidth)
    {
        queue.Enqueue(cellValue);
        summOfQueuePoints += cellValue;
        if (queue.Count > windowWidth)
            summOfQueuePoints -= queue.Dequeue();
        return summOfQueuePoints;
    }
}

public static class MovingAverageTask
{
    public static IEnumerable<DataPoint> MovingAverage(this IEnumerable<DataPoint> data, int windowWidth)
	{
		var queueOfCells = new Queue<double>();
		foreach(var windowCell in data)
		{
            var cellValue = windowCell.OriginalY;
            var summOfQueuePoints = queueOfCells.EnqueueAndReturnSumm(cellValue, windowWidth);
            var averageValue = summOfQueuePoints / queueOfCells.Count;
            yield return windowCell.WithAvgSmoothedY(averageValue);
        }
	}
}