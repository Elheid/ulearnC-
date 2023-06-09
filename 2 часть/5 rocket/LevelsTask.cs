using System;
using System.Collections.Generic;
using System.Reflection.Metadata;
namespace func_rocket
{
    public class LevelsTask
    {
        private static int whiteHoleConst = -140;
        private static int blackHoleConst = 300;
        static readonly Physics standardPhysics = new();
        private static Vector targetVector = new Vector(600, 200);
        private static Vector heavyDir = new Vector(0, 0.9);
        private static Vector upDir = new Vector(0, -1);
        private static Vector upTarget = new Vector(700, 500);
        private static Rocket rocket = new Rocket(new Vector(200, 500), Vector.Zero, -0.5 * Math.PI);

        public static double Distance(Vector x, Vector y) => (x - y).Length;
        public static Vector GetAnomaluLoc() => (targetVector + rocket.Location) / 2;
        public static double GetForceOfHole(double dist, double constant) => constant* dist / (dist* dist + 1);
        public static Vector GetHoleDir(Vector target, Vector vLocation) => (target - vLocation).Normalize();
        public static Vector GetHoleGravity(Vector target, Vector vLoc, int holeConst) => GetGravityVector(GetForceOfHole(Distance(target, vLoc), holeConst),
                GetHoleDir(target, vLoc));

        private static Vector GetGravityVector(double force, Vector direction) => force * direction;

        private static Level GetNewLevel(string title, Vector _targetVector, Gravity gravity)
        {
            return new Level
                (
                title,
                rocket,
                _targetVector,
                gravity,
                standardPhysics
                );
        }

        public static IEnumerable<Level> CreateLevels()
        {
            Func<Vector, Vector, double> distFromDown = (space, vLocation) => space.Y - vLocation.Y;
            Func<double, double> forceUp = (dist) => (300 / (dist + 300.0));
            yield return GetNewLevel("Zero", targetVector, (size, vLoc) => GetGravityVector(1, Vector.Zero));
            yield return GetNewLevel("Heavy", targetVector, (size, vLoc) => GetGravityVector(1, heavyDir));
            yield return GetNewLevel("Up", upTarget, (size, vLoc) => GetGravityVector(forceUp(distFromDown(size, vLoc)), upDir));
            yield return GetNewLevel("WhiteHole", targetVector, (size, vLoc) => GetHoleGravity(targetVector, vLoc, whiteHoleConst));
            yield return GetNewLevel("BlackHole", targetVector, (size, vLoc) => GetHoleGravity(GetAnomaluLoc(), vLoc, blackHoleConst));
            yield return GetNewLevel("BlackAndWhite", targetVector, (size, vLoc) => (GetHoleGravity(targetVector, vLoc, whiteHoleConst) + GetHoleGravity(GetAnomaluLoc(), vLoc, blackHoleConst)) / 2);
        }
    }
}