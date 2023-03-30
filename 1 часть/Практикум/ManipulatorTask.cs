using System;
using NUnit.Framework;

namespace Manipulation
{
    public static class ManipulatorTask
    {
        public static double[] MoveManipulatorTo(double x, double y, double alpha)
        {
            var palm = Manipulator.Palm;
            var forearm = Manipulator.Forearm;
            var upperArm = Manipulator.UpperArm;
            var xWrist = x + palm * Math.Cos(Math.PI - alpha);
            var yWrist = y + palm * Math.Sin(Math.PI - alpha);
            var lengthShouldeToWrist = Math.Sqrt(Math.Pow(xWrist, 2) + Math.Pow(yWrist, 2));
            var elbow = TriangleTask.GetABAngle(upperArm, forearm, lengthShouldeToWrist);
            var shoulder = TriangleTask.GetABAngle(upperArm, lengthShouldeToWrist, forearm)
                                    + Math.Atan2(yWrist, xWrist);
            var wrist = -alpha - shoulder - elbow;
            if (double.IsNaN(wrist))
                return new[] { double.NaN, double.NaN, double.NaN };
            return new[] { shoulder, elbow, wrist };
        }
    }

    [TestFixture]
    public class ManipulatorTask_Tests
    {
        private static readonly Random rnd = new Random();

        [Test]
        public void TestMoveManipulatorTo()
        {

        }
    }
}