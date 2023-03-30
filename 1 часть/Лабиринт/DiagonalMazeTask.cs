using System;
namespace Mazes
{
    public static class DiagonalMazeTask
    {
        const int PlaceOccupiedByWalls = 2;
        public static void MoveOut(Robot robot, int width, int height)
        {
            var widthWithOutWall = width - PlaceOccupiedByWalls;
            var heightWithOutWall = height - PlaceOccupiedByWalls;
            var rightBorder = widthWithOutWall - 1;
            var downBorder = heightWithOutWall - 1;
            var stepCountWidth = rightBorder / (heightWithOutWall);
            var stepCountHeight = downBorder / widthWithOutWall;
            if (width > height)
                DiagonalMove(robot, stepCountWidth, downBorder, Direction.Right, Direction.Down);
            else
                DiagonalMove(robot, stepCountHeight, rightBorder, Direction.Down, Direction.Right);
        }
        static void MoveTo(Robot robot, int stepCount, Direction dir)
        {
            for (var i = 0; i < stepCount; i++)
                robot.MoveTo(dir);
        }
        public static void DiagonalMove(Robot robot, int stepCount, int border, Direction firstDir, Direction secondDir)
        {
            for (int i = 0; i <= border; ++i)
            {
                MoveTo(robot, stepCount, firstDir);
                if (i != border)
                    MoveTo(robot, 1, secondDir);
            }
        }
    }
}
