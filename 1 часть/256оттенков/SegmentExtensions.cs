using System.Collections.Generic;
using GeometryTasks;
using System.Drawing;

namespace GeometryPainting
{
    public static class SegmentExtensions
    {
        static Dictionary<Segment, Color> coloursDictionary = new Dictionary<Segment, Color>();
        public static void SetColor(this Segment segment, Color colour)
        {
            if (!coloursDictionary.ContainsKey(segment))
                coloursDictionary.Add(segment, colour);
            else
                coloursDictionary[segment] = colour;
        }

        public static Color GetColor(this Segment segment)
        {
            var dictionaryEmptyOrKeyNotFound = coloursDictionary.Count == 0 && coloursDictionary.ContainsKey(segment);
            return (dictionaryEmptyOrKeyNotFound ? Color.Black : coloursDictionary[segment]);
        }
    }
}

