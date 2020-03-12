using System;

namespace SatelliteToolkit.Model
{
    public class Constants
    {
        /// <summary>
        /// Earth's Gravitational Constant (MU) expressed in m3/s2
        /// </summary>
        public static double EarthGravitationalConstant = 3.986005 * Math.Pow(10, 14);

        /// <summary>
        /// Earth's Radius expressed in Kilometers
        /// </summary>
        public static double EarthRadius = 6371;

        /// <summary>
        /// K1 expressed in km2
        /// </summary>
        public static double K1 = 66063.1704;
    }
}
