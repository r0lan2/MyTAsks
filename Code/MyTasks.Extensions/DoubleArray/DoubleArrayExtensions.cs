//INSTANT C# NOTE: Formerly VB project-level imports:

using System;
using System.Linq;

namespace BigLamp.Extensions.DoubleArray
{
    public static class DoubleArrayExtensions
    {
        /// <summary>
        /// Mean of a given double array
        /// </summary>
        /// <param name="values"></param>
        /// <returns></returns>
        /// <remarks></remarks>
        public static double Mean(this double[] values)
        {
            return values.Average(val => val);
        }

        /// <summary>
        /// Total of a given double array
        /// </summary>
        /// <param name="values"></param>
        /// <returns></returns>
        /// <remarks></remarks>
        public static double Total(this double[] values)
        {
            return values.Sum();
        }

        /// <summary>
        /// Standard Deviation of a given double array
        /// </summary>
        /// <param name="values"></param>
        /// <returns></returns>
        /// <remarks></remarks>
        public static double StandardDev(this double[] values)
        {
            double result = 0;
            double totalSqOfDifference = 0;
            var mean = values.Mean();

            foreach (double d in values)
            {
                totalSqOfDifference += Math.Pow((d - mean), 2);
            }

            if (values.Count() > 0)
            {
                result = Math.Sqrt(totalSqOfDifference / values.Count());
            }

            return result;

        }
        /// <summary>
        /// Coefficient of Variation of a given double array
        /// </summary>
        /// <param name="values"></param>
        /// <returns></returns>
        /// <remarks></remarks>
        public static double CoefficientVariation(this double[] values)
        {
            double result = 0;
            var mean = values.Mean();
            var standardDev = values.StandardDev();

            if (mean > 0)
            {
                result = standardDev * 100 / mean; // case 34521
            }

            return result;
        }
    }
}


