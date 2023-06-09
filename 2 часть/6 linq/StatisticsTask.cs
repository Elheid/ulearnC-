using System;
using System.Collections.Generic;
using System.Linq;

namespace linq_slideviews;

public class StatisticsTask
{
	public static double GetMedianTimePerSlide(List<VisitRecord> visits, SlideType slideType)
	{
        var timePerSlide = visits.OrderBy(visit => visit.DateTime)
                .GroupBy(visit => visit.UserId)
                .SelectMany(visits => visits.Bigrams())
                .Where(bigram => bigram.Item1.SlideType == slideType)
                .Select(visits => visits.Second.DateTime.Subtract(visits.First.DateTime).TotalMinutes)
                .Where(minutes => minutes >= 1 && minutes <= 120); 
        return (visits.Count == 0 || timePerSlide.Count() == 0 ) ? 0 : timePerSlide.Median();
    }
}