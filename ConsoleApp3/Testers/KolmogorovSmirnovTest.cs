using System;
using System.Collections.Generic;
using System.Text;

namespace OnlineSignitureVerification.Testers
{
    class KolmogorovSmirnovTest : TwoCompareTester<KolmogorovSmirnovTest>
    {
        protected override double Calculate(List<double> F, List<double> G)
        {
            double ret = -1;

            foreach( double lm in F)
            {
                double tmp = Math.Abs(getDistValue(F, lm)-getDistValue(G, lm));
                //Console.WriteLine(tmp);
                if (tmp > ret)
                    ret = tmp;
            }

            return ret;
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

                //if (4 * localminMax[1, i] < TeachedMatrix[10, i]) a = 0.015;
                //else if (4 * localminMax[1, i] < TeachedMatrix[10, i]) a = 0.03;
                //else if (3 * localminMax[1, i] < TeachedMatrix[10, i]) a = 0.09;
                //else if (2 * globalMax < TeachedMatrix[10, i]) a = 0.1;

                double a = 0;
                double localMinmaxRange = localminMax[1, i] - localminMax[0, i];
                if (Program.resFileName == ResFileName.task1)
                {
                    if (4 * localminMax[1, i] < TeachedMatrix[10, i]) a = 0.015;
                    else if (3 * localminMax[1, i] < TeachedMatrix[10, i]) a = 0.09;
                    else if (2 * globalMax < TeachedMatrix[10, i]) a = 0.1;
                }
                else
                {
                    if (2 * localMinmaxRange < TeachedMatrix[10, i] - localminMax[0, i]) a = 0.005;
                    else if (localMinmaxRange < TeachedMatrix[10, i] - localminMax[0, i]) a = 0.01;
                    else if (0.5 * localMinmaxRange < TeachedMatrix[10, i] - localminMax[0, i]) a = 0.035;
                    else if (0.4 * localMinmaxRange < TeachedMatrix[10, i] - localminMax[0, i]) a = 0.05;
                    else if (0.3 * localMinmaxRange < TeachedMatrix[10, i] - localminMax[0, i]) a = 0.07;
                    else if (0 < TeachedMatrix[10, i] - localminMax[0, i]) a = 0.08;
                    else if (-0.5 * localMinmaxRange < TeachedMatrix[10, i] - localminMax[0, i]) a = 0.09;
                    else a = 1;
                }

                ret += a;
                //Console.Write("eredmeny {0}\n", a);
            }

            if (ret < 0) ret = 0;
            if (ret > 1) ret = 1;
            ret = ret * 0.5 + 0.25;
            return ret;
        }

        private double getDistValue(List<double> F, double x)
        {
            double less = 0;
            foreach( double lm in F){
                if (lm < x) less++;
            }

            return less/F.Count;
        }
    }
}
