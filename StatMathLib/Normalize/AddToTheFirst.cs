using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace StatMathLib.Normalize
{
    public static class AddToTheFirst
    {
        public static decimal Average(List<decimal> F, List<decimal> G)
        {
            decimal ret = 0;

            decimal SF = F.Average();
            decimal SG = G.Average();
            ret = SG / G.Count - SF / F.Count;

            return ret;
        }

        public static decimal Min(List<decimal> F, List<decimal> G)
        {
            return F.Min() - G.Min();
        }
    }
}
