//INSTANT C# NOTE: Formerly VB project-level imports:
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.Xml.Linq;
using BigLamp.Extensions.Double;

public static class IEnumerableExtensions
{



    public static double StdDev(this IEnumerable<double> values)
    {
        double ret = 0;
        int count = values.Count();
        if (count > 1)
        {
            //Compute the Average
            double avg = values.Average();

            //Perform the Sum of (value-avg)^2
            double sum = values.Sum(d => (d - avg) * (d - avg));

            //Put it all together
            ret = Math.Sqrt(sum / count);
        }
        return ret.Round(1);
    }
	

}


