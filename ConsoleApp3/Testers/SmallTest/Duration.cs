using System;
using System.Collections.Generic;
using System.Text;

namespace OnlineSignitureVerification.Testers.SmallTest
{
    class Duration : Tester<Duration>
    {
        int[] teached = new int[10];

        public void Teach(List<List<List<double>>> DataMatrix)
        {
            for (int i = 0; i < DataMatrix.Count; i++)
            {
                teached[i] = DataMatrix[i][0].Count;
            }
            //for (int i = 0; i < 10; i++)
            //{
            //    Console.WriteLine(teached[i]);
            //}
            //Console.WriteLine();
        }

        public double TestMethod(List<List<double>> s1)
        {
            double ret = -1;
            List<double> column = s1[0];  ///mivel ugyanolyan mintavétel, bármely oszlop elemszáma = duration

            int max = 0;
            foreach (int lm in teached) if (lm > max) max = lm;

            if (column.Count <= max * 0.75) ret = 0.75;
            else if (column.Count <= max *0.9) ret = 0.65;
            else if (column.Count <= max) ret = 0.5;
            else if (column.Count < max*1.05) ret = 0.47;
            else if (column.Count < max * 1.3) ret = 0.3;
            else if (column.Count < max * 1.6) ret = 0.2;
            else ret = 0.1;

            return ret;
        }

        public int calculate(List<List<double>> s1, List<List<double>> s2)
        {
            return Math.Abs(s1[0].Count - s1[0].Count);
        }
    }
}
