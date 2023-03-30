using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

namespace GravityBalls
{
    public class WorldModel
    {
        public int CursorX;
        public int CursorY;

        public double BallX;
        public double BallY;
        public double BallRadius;
        public double WorldWidth;
        public double WorldHeight;
        public double SpeedY = 300;
        public double SpeedX = 200;
        public double CoefficientOfResistance = 0.2;
        public double ForseOfGravity = 10;

        public void SimulateTimeframe(double dt)
        {
            MoveBall(dt, ref BallY, ref SpeedY, WorldHeight);
            MoveBall(dt, ref BallX, ref SpeedX, WorldWidth);
        }


        public void MoveBall(double dt, ref double ballCoordinate, ref double speed, double border)
        {

            if (speed > 0)
                ballCoordinate = Math.Min(ballCoordinate + speed * dt, border - BallRadius);
            else
                ballCoordinate = Math.Max(ballCoordinate + speed * dt, BallRadius);
            BallRebound(ref ballCoordinate, ref speed, border);
            ForseOfResistanceOn(ref speed);
            GravityForseOn(ref ballCoordinate, ref speed);
            //CursosGiveVelocity(ref speed);
        }
        public void BallRebound(ref double ballCoordinate, ref double speed, double border)
        {
            if (ballCoordinate == border - BallRadius || ballCoordinate == BallRadius)
                speed *= (-1);
        }

        public void GravityForseOn (ref double ballCoordinate, ref double speed)
        {
            if (ballCoordinate == BallY)
                speed += ForseOfGravity;
        }

        public void ForseOfResistanceOn(ref double speed)
        {
            var oppositeSpeedSign = (speed > 0) ? -1 : 1;
            speed += CoefficientOfResistance * oppositeSpeedSign;
        }

        ////Math.Sqrt((CursorX - BallX) * (CursorX - BallX) + (CursorY - BallY) * (CursorY - BallY)); расстояние от курсора до мячика
        //public void CursosGiveVelocity(ref double speed)
        //{
        //    var direction = (speed > 0) ? -1 : 1;
        //    if (CursorX == BallX || CursorY == BallY)
        //    {
        //        speed +=direction * 200 / Math.Sqrt((CursorX - BallX) * (CursorX - BallX) + (CursorY - BallY) * (CursorY - BallY));
        //    }
        //}
    }
}
