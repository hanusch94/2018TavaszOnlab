using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace StatMathLib.Normalize
{
    public static class AddToTheFirst
    {
        public static double Average(List<double> F, List<double> G)
        {
            double ret = 0;

            double SF = F.Average();
            double SG = G.Average();
            ret = SG / G.Count - SF / F.Count;

            return ret;
        }

        public static double Min(List<double> F, List<double> G)
        {
            return F.Min() - G.Min();
        }
    }
}
