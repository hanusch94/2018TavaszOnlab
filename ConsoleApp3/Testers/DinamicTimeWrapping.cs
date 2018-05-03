using System;
using System.Collections.Generic;
using System.Text;
using StatMathLib;
using System.Linq;

namespace OnlineSignitureVerification.Testers
{
    /// <summary>
    /// Math background: http://seninp.github.io/assets/pubs/senin_dtw_litreview_2008.pdf
    /// </summary>
    class DinamicTimeWrapping : TwoCompareTester<DinamicTimeWrapping>
    {
        private double[,] GlobalCostMatrix;

        protected override double Calculate(List<double> F, List<double> G)
        {
            double normalizer = 0; //StatMathLib.Normalize.AddTheSecond.Average(F, G);
            GlobalCostMatrix = new double[F.Count, G.Count];

            for (int i = 1; i < F.Count; i++)
            {
                GlobalCostMatrix[i,1]=GlobalCostMatrix[i - 1, 1] + Math.Abs(F[i] + normalizer - G[1]);
            }
            for (int j = 1; j < G.Count; j++)
            {
                GlobalCostMatrix[1,j] = GlobalCostMatrix[1, j - 1] + Math.Abs(F[1] + normalizer - G[j]);
            }
            for (int i = 1; i < F.Count; i++)
            {
                for (int j = 1; j < G.Count; j++)
                {
                    GlobalCostMatrix[i, j] = Math.Abs(F[i] + normalizer - G[j] +
                        Math.Min(GlobalCostMatrix[i - 1, j], Math.Min(GlobalCostMatrix[i, j - 1], GlobalCostMatrix[i - 1, j - 1])));
                }
            }

            return GlobalCostMatrix[F.Count-1, G.Count-1];
        }

        protected override double TestMethod(List<List<List<double>>> DataMatrix, List<double> testedMatrix, int ColumnId)
        {
            double ret = 0;
            double[,] localminMax = new double[2, 10];
            double globalMin = -1;
            double globalMax = -1;

            for (int i = 0; i < DataMatrix.Count; i++)
            {
                double a = Calculate(DataMatrix[i][ColumnId], testedMatrix);
                //Console.WriteLine(a);
                TeachedMatrix[10, i] = a;
            }
            for (int i = 0; i < 10; i++)
            {
                localminMax[0, i] = -1;
                localminMax[1, i] = -1;
                for (int j = 0; j < 10; j++)
                {
                    if (i != j)
                    {
                        if (localminMax[0, i] == -1 || localminMax[0, i] > TeachedMatrix[i, j])
                            localminMax[0, i] = TeachedMatrix[i, j];
                        if (localminMax[1, i] == -1 || localminMax[1, i] < TeachedMatrix[i, j])
                            localminMax[1, i] = TeachedMatrix[i, j];
                    }
                }
                if (globalMin == -1 || localminMax[0, i] < globalMin)
                    globalMin = localminMax[0, i];
                if (globalMax == -1 || localminMax[1, i] < globalMax)
                    globalMax = localminMax[1, i];
            }

            for (int i = 0; i < 10; i++)
            {
                //Console.Write("max: {0},  Calc: {1}", localminMax[1, i], TeachedMatrix[10, i]);

                double localMinmaxRange = localminMax[1, i] - localminMax[0, i];

                double a = 0;
                if (inIntervall(localMinmaxRange * 0.3, TeachedMatrix[10, i] - localminMax[0, i], localMinmaxRange * 0.7)) a = -0.05;
                else if (inIntervall(localMinmaxRange * 0.2, TeachedMatrix[10, i] - localminMax[0, i], localMinmaxRange * 0.8)) a = -0.02;
                else if (inIntervall(localMinmaxRange * 0.1, TeachedMatrix[10, i] - localminMax[0, i], localMinmaxRange * 0.9)) a = -0.01;
                else if (inIntervall(localMinmaxRange, TeachedMatrix[10, i] - localminMax[0, i], localMinmaxRange )) a = 0;
                else if (inIntervall(localMinmaxRange * -0.5, TeachedMatrix[10, i] - localminMax[0, i], localMinmaxRange * 1.5)) a = 0.01;
                else if (inIntervall(localMinmaxRange * -1, TeachedMatrix[10, i] - localminMax[0, i], localMinmaxRange * 2)) a = 0.02;
                else if (inIntervall(localMinmaxRange * -2, TeachedMatrix[10, i] - localminMax[0, i], localMinmaxRange * 3)) a = 0.04;
                else if (inIntervall(localMinmaxRange * -3, TeachedMatrix[10, i] - localminMax[0, i], localMinmaxRange * 5)) a = 0.07;
                else if (inIntervall(localMinmaxRange * -5, TeachedMatrix[10, i] - localminMax[0, i], localMinmaxRange * 6)) a = 0.09;
                else if (inIntervall(localMinmaxRange * -6, TeachedMatrix[10, i] - localminMax[0, i], localMinmaxRange * 7)) a = 0.095;
                else a = 0.1;

                ret += a;
                //Console.Write("eredmeny {0}\n", a);
            }

            if (ret < 0) ret = 0;
            if (ret > 1) ret = 1;
            return ret*0.9+0.05;
        }

        private bool inIntervall(double min, double value, double max)
        {
            return min < value && value < max;
        }
    }
}
