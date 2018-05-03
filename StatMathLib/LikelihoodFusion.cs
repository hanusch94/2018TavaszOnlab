using System;

namespace StatMathLib
{
    public static class LikelihoodFusion
    {
        /// <summary>
        /// It implement's the Bayes Trap. It is for merge two likelihood into one. The parameters, and the returnvalue are in 0:1 intervall
        /// </summary>
        public static double BayesTrap(double a, double b)
        {
            if (b > 1 || b < 0) throw new ArgumentOutOfRangeException("a");
            if (a > 1 || a < 0) throw new ArgumentOutOfRangeException("b");

            return (a * b) / (a * b + (1 - a) * (1 - b));
        }

        /// <summary>
        /// It implement's the Bayes Trap. It is for merge two likelihood into one. The parameters, and the returnvalue are in 0:1 intervall
        /// </summary>
        public static float BayesTrap(float a, float b)
        {
            if (b > 1 || b < 0) throw new ArgumentOutOfRangeException("a");
            if (a > 1 || a < 0) throw new ArgumentOutOfRangeException("b");

            return (a * b) / (a * b + (1 - a) * (1 - b));
        }
    }
}
