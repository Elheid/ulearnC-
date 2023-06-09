using System;
using System.Collections.Generic;
using System.Drawing;
using System.Globalization;
using System.Linq;

namespace linq_slideviews;

public class ParsingTask
{
    public static IDictionary<int, SlideRecord> ParseSlideRecords(IEnumerable<string> lines)
    {
        SlideType slide;
        return lines
                .Select(words => words.Split(";"))
                .Where(phrase => phrase.Length == 3 && IsThisNumber(phrase[0]) && !IsThisNumber(phrase[1]) && !IsThisNumber(phrase[2]) && IsThisSlideType(phrase[1]))
                .Select(phrase => ReturnSlideRecord(phrase))
                .ToDictionary(dict => dict.SlideId, value => value);
    }

    public static SlideRecord ReturnSlideRecord(string[] str)
    {
        return new SlideRecord(int.Parse(str[0]), GiveSlideType(ToCorrectStr(str[1])), str[2]);
    }

    public static bool IsThisSlideType(string str)
    {
        SlideType slide;
        return SlideType.TryParse(ToCorrectStr(str), out slide);
    }

    public static string ToCorrectStr(string str)
    {
        string first = "";
        for(var i=0; i<str.Length;i++)
        {
            if (i == 0)
                first += Char.ToUpper(str[0]);
            else
                first += Char.ToLower(str[i]);
        }
        return first.ToString();
    }

    public static SlideType GiveSlideType(string str)
    {
        SlideType slide;
        SlideType.TryParse(str, out slide);
        return slide;
    }

    public static bool IsThisNumber(string str)
    {
        int numForCheck;
        return int.TryParse(str, out numForCheck);
    }

    public static bool IsThisCorrectData(string str)
    {
        DateTime dataForCheck;
        return DateTime.TryParse(str, out dataForCheck);
    }

    public static IEnumerable<VisitRecord> ParseVisitRecords(
        IEnumerable<string> lines, IDictionary<int, SlideRecord> slides)
    {
        var titleString = "UserId;SlideId;Date;Time";
        return lines.Where(words => words != titleString).Select(words => ReturnVisitRecord(words, slides));
    }

    public static VisitRecord ReturnVisitRecord(string visitStr, IDictionary<int, SlideRecord> slides)
    {
        var str = visitStr.Split(";");
        if (!(str.Length == 4 && (IsThisNumber(str[0])) && (IsThisNumber(str[1])) && IsThisCorrectData(str[2]) && IsThisCorrectData(str[3]) && slides.ContainsKey(int.Parse(str[1]))))
            throw new FormatException($"Wrong line [{visitStr}]");
        var userId = int.Parse(str[0]);
        var slideId = int.Parse(str[1]);
        return new VisitRecord(userId,
            slideId, ReturnCorrectDataTime(str[2], str[3]),
            slides[slideId].SlideType);
    }

    public static DateTime ReturnCorrectDataTime(string date, string time)
    {
        var dateString = $"{date} {time}";
        return DateTime.Parse(dateString);
    }
}