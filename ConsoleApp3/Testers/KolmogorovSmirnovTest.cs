using System;
using System.Collections.Generic;
using System.Text;

namespace OnlineSignitureVerification.Testers
{
    class KolmogorovSmirnovTest : TwoCompareTester<KolmogorovSmirnovTest>
    {
        protected override decimal Calculate(List<decimal> F, List<decimal> G)
        {
            decimal ret = -1;

            foreach( decimal lm in F)
            {
                decimal tmp = Math.Abs(getDistValue(F, lm)-getDistValue(G, lm));
                if (tmp > ret)
                    ret = tmp;
            }

            return ret;
        }

        protected override double TestMethod(List<List<List<decimal>>> DataMatrix, List<decimal> testedMatrix, int ColumnId)
        {
            double ret = 0;
            decimal[,] localminMax = new decimal[2, 10];
            decimal globalMin = -1;
            decimal globalMax = -1;

            for (int i = 0; i < DataMatrix.Count; i++)
            {
                decimal a = Calculate(DataMatrix[i][ColumnId], testedMatrix);
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

                double a = 0;
                if (2 * globalMax < TeachedMatrix[10, i])
                    a = -0.1;
                else if (2 * localminMax[1, i] < TeachedMatrix[10, i])
                    a = 0;
                else if (localminMax[1, i] < TeachedMatrix[10, i])
                    a = 0.02;
                else if (localminMax[1, i] + localminMax[0, i] < TeachedMatrix[10, i] * 2)
                    a = 0.06;
                else
                    a = 0.1;

                ret += a;
                //Console.Write("eredmeny {0}\n", a);
            }

            if (ret < 0)
                ret = 0;
            return ret;
        }

        private decimal getDistValue(List<decimal> F, decimal x)
        {
            int less = 0;
            foreach( decimal lm in F){
                if (lm < x) less++;
            }

            return less/F.Count;
        }
    }
}
