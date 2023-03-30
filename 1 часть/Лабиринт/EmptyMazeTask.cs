using System.Net.NetworkInformation;

namespace Mazes
{
    public static class EmptyMazeTask
    {
        const int placeOccupiedByWalls = 2;
        public static void MoveOut(Robot robot, int width, int height)
        {
            var stepsForWidth = width - placeOccupiedByWalls - robot.X;
            var stepsForHeight = height - placeOccupiedByWalls - robot.Y;
            MoveTo(robot, stepsForWidth, Direction.Right);
            MoveTo(robot, stepsForHeight, Direction.Down);
        }

        static void MoveTo(Robot robot, int stepCount, Direction dir)
        {
           for (var i = 0; i < stepCount; i++)
                robot.MoveTo(dir);
        }
    }
}