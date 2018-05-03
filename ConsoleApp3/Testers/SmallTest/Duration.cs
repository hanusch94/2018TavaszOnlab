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
            for (int i = 0; i < 10; i++)
            {
                //Console.WriteLine(teached[i]);
            }
            //Console.WriteLine();
        }

        public double TestMethod(List<double> testedMatrix)
        {
            double ret = -1;

            int max = 0;
            foreach (int lm in teached) if (lm > max) max = lm;

            if (testedMatrix.Count <= max * 0.75) ret = 0.60;
            else if (testedMatrix.Count <= max *0.9) ret = 0.56;
            else if (testedMatrix.Count <= max) ret = 0.5;
            else if (testedMatrix.Count < max*1.05) ret = 0.47;
            else if (testedMatrix.Count < max * 1.3) ret = 0.3;
            else if (testedMatrix.Count < max * 1.6) ret = 0.2;
            else ret = 0.1;

            return ret;
        }
    }
}
