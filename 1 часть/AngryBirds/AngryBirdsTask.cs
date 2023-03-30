using System;

namespace AngryBirds
{

    namespace AngryBirds
    {
        public static class AngryBirdsTask
        {
            /// <param name="v">Начальная скорость</param>
            /// <param name="distance">Расстояние до цели</param>
            /// <returns>Угол прицеливания в радианах от 0 до Pi/2</returns>
            public static double FindSightAngle(double speed, double distance)
            {
                const G = 9.8;
                return 0.5 * Math.Asin((distance * g) / (speed * speed));
            }
        }
    }