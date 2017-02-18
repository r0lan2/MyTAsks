//INSTANT C# NOTE: Formerly VB project-level imports:

using System;

namespace BigLamp.Extensions.Double
{
    public static class DoubleExtensions
    {
        /// <summary>
        /// Does regular rounding. Not bankers rounding.
        /// </summary>
        /// <param name="value"></param>
        /// <param name="decimalPrecision"></param>
        /// <returns></returns>
        /// <remarks></remarks>
        public static double Round(this double value, int decimalPrecision)
        {
            return Math.Round(value, decimalPrecision, MidpointRounding.AwayFromZero);
        }
    }
}
