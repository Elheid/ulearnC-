using System;
using System.Drawing;
using NUnit.Framework;

namespace Manipulation
{
    public static class AnglesToCoordinatesTask
    {
        /// <summary>
        /// По значению углов суставов возвращает массив координат суставов
        /// в порядке new []{elbow, wrist, palmEnd}
        /// </summary>
        static float xCoordinate;
        static float yCoordinate;
        public static PointF[] GetJointPositions(double shoulder, double elbow, double wrist)
        {
            xCoordinate = 0;
            yCoordinate = 0;
            var upperArm = Manipulator.UpperArm;
            var forearm = Manipulator.Forearm;
            var palm = Manipulator.Palm;    

            var angleOfForearmAndX = elbow + shoulder - Math.PI; 
            var angleOfPalmAndX = wrist + angleOfForearmAndX - Math.PI;
            var elbowPos = ChangeCoordinate( upperArm,shoulder);
            var wristPos = ChangeCoordinate(forearm, angleOfForearmAndX);
            var palmEndPos = ChangeCoordinate(palm, angleOfPalmAndX);
            return new PointF[]
            {
                elbowPos,
                wristPos,
                palmEndPos
            };
        }

        public static PointF ChangeCoordinate(float arm, double angle)
        {
            xCoordinate += arm * (float)Math.Cos(angle);
            yCoordinate += arm * (float)Math.Sin(angle);
            return new PointF(xCoordinate, yCoordinate);
        }
    }

    [TestFixture]
    public class AnglesToCoordinatesTask_Tests
    {
        [TestCase(Math.PI / 2, Math.PI / 2, Math.PI, Manipulator.Forearm + Manipulator.Palm, Manipulator.UpperArm)]
        [TestCase(Math.PI / 2, Math.PI / 2, Math.PI, Manipulator.Forearm + Manipulator.Palm, Manipulator.UpperArm)]
        [TestCase(Math.PI / 2, Math.PI / 2, Math.PI, Manipulator.Forearm + Manipulator.Palm, Manipulator.UpperArm)]
        [TestCase(Math.PI / 2, Math.PI / 2, Math.PI, Manipulator.Forearm + Manipulator.Palm, Manipulator.UpperArm)]

        public void TestGetJointPositions(double shoulder, double elbow, double wrist, double palmEndX, double palmEndY)
        {
            var joints = AnglesToCoordinatesTask.GetJointPositions(shoulder, elbow, wrist);
            Assert.AreEqual(palmEndX, joints[2].X, 1e-5, "palm endX");
            Assert.AreEqual(palmEndY, joints[2].Y, 1e-5, "palm endY");
        }
    }
}