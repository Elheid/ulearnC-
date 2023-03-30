using System;

using System;

namespace Billiards
{
    public static class BilliardsTask
    {
        /// <summary>
		/// <param name="BounceWall">Находит угол отскока шарика от стены</param>//Это пояснение самого метода, его не надо?
        /// </summary>
        /// <param name="directionRadians">Угол направления движения шара</param>
        /// <param name="wallInclinationRadians">Угол</param>
        /// <returns></returns>
        public static double BounceWall(double directionRadians, double wallInclinationRadians)
        {
            return 2 * wallInclinationRadians - directionRadians;
        }
  
    }
}
 
