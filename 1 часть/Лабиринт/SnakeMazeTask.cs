using Mazes;
using System;
using System.Runtime.CompilerServices;
using System.Security.Cryptography.X509Certificates;

namespace Mazes
{
    public static class SnakeMazeTask
    {
        const int placeOccupiedByWalls = 2;
        public static void MoveOut(Robot robot, int width, int height)
        {
            var stepsForWidth = width - placeOccupiedByWalls - robot.X;
            var bottomOfMaze = height - placeOccupiedByWalls;
            var stepsForHeight = 2;
            while (!robot.Finished)
            {
                MoveTo(robot, stepsForWidth, Direction.Right);
                MoveTo(robot, stepsForHeight, Direction.Down);
                MoveTo(robot, stepsForWidth, Direction.Left);
                if (robot.Y != bottomOfMaze)
                {
                        MoveTo(robot, stepsForHeight, Direction.Down);
                }
            }
        }

        static void MoveTo(Robot robot, int stepCountWidth, Direction dir)
        {
            for (var i = 0; i < stepCountWidth; i++)
            {
                robot.MoveTo(dir);
            }
        }
    }
}
