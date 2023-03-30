using GeometryTasks;
using System;
using System.Numerics;

namespace GeometryTasks
{
    public class Segment
    {
        public Vector Begin;
        public Vector End;

        public double GetLength()
        {
            return Geometry.GetLength(this);
        }

        public bool Contains(Vector vector)
        {
            return Geometry.IsVectorInSegment(vector, this);
        }

    }
    public class Vector
    {
        public double X;
        public double Y;

        public double GetLength()
        {
            return Geometry.GetLength(this);
        }

        public Vector Add(Vector vector)
        {
            return Geometry.Add(this, vector);
        }

        public bool Belongs(Segment segment)
        {
            return Geometry.IsVectorInSegment(this, segment);
        }

    }
    public class Geometry
    {
        public static double GetLength(Vector vector)
        {
            return Math.Sqrt((vector.X * vector.X) + (vector.Y * vector.Y));
        }

        public static Vector Add(Vector firstVector, Vector secondVector)
        {
            var sumVector = new Vector();
            {
                sumVector.X = firstVector.X + secondVector.X;
                sumVector.Y = firstVector.Y + secondVector.Y;
            }
            return sumVector;
        }

        public static double GetLength(Segment segment)
        {
            var lengthX = (segment.End.X - segment.Begin.X);
            var lengthY = (segment.End.Y - segment.Begin.Y);    
            return Math.Sqrt(lengthX * lengthX + lengthY * lengthY);
        }
        public static bool IsVectorInSegment(Vector vectorX, Segment segmentAB)
        {
            //var segmentAX = new Segment();
            //{
            //    segmentAX.Begin.X = segmentAB.Begin.X;
            //    segmentAX.Begin.X = segmentAB.End.X;
            //    segmentAX.End.X = vectorX.X;
            //    segmentAX.End.Y = vectorX.Y;
            //}
            //var segmentXB = new Segment();
            //{
            //    segmentXB.Begin.X = vectorX.X;
            //    segmentXB.Begin.Y = vectorX.Y;
            //    segmentAX.End.X = segmentAB.End.X;
            //    segmentAX.End.Y = segmentAB.End.Y;
            //}
            //var ABLength = GetLength(segmentAB);
            //var AXLength = GetLength(segmentAX);
            //var XBLength = GetLength(segmentXB);
            //if ((AXLength + XBLength) == ABLength)
            //    return true;
            //return
            var vectorBeetwenSegX = vectorX.X >= segmentAB.Begin.X && vectorX.X <= segmentAB.End.X;
            var vectorVeetwenSegY = vectorX.Y >= segmentAB.Begin.Y && vectorX.Y <= segmentAB.End.Y;
            var xVectorBeetwenBeginAndEndSeg = (vectorX.X - segmentAB.Begin.X) * (segmentAB.End.Y - segmentAB.Begin.Y);
            var yVectorBeetwenBeginAndEndSeg = (vectorX.Y - segmentAB.Begin.Y) * (segmentAB.End.X - segmentAB.Begin.X);
            var AXCollinearAB = xVectorBeetwenBeginAndEndSeg - yVectorBeetwenBeginAndEndSeg == 0;
            var vectorOnSegBegin = (segmentAB.Begin.X == vectorX.X) && (segmentAB.Begin.Y == vectorX.Y);
            var vectorOnSegEnd = (segmentAB.End.X == vectorX.X) && (segmentAB.End.Y == vectorX.Y);
            var isVectorOnBeginOrEndOfSegment = vectorOnSegBegin || vectorOnSegEnd;
            if (isVectorOnBeginOrEndOfSegment)
                return true;
            return AXCollinearAB && vectorBeetwenSegX && vectorVeetwenSegY;
        }
    }
}
namespace ReadOnlyVectorTask
{
    public class ReadOnlyVector
    {
        public readonly double X;
        public readonly double Y;
        public ReadOnlyVector(double x, double y)
        {
            X = x;
            Y = y;
        }
        public ReadOnlyVector Add(ReadOnlyVector other)
        {
            return new ReadOnlyVector 
            (
                X + other.X,
                Y + other.Y
            );
        }

        public ReadOnlyVector WithX(double x)
        {
            return new ReadOnlyVector(x, Y);
        }

        public ReadOnlyVector WithY(double y)
        {
            return new ReadOnlyVector(X, y);
        }
    }
}
                    