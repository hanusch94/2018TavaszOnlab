using System;
using System.Collections.Generic;
using System.Text;

namespace OnlineSignitureVerification.Testers
{
    abstract class TwoCompareTester<T> : Tester<T>
        where T: class
    {
        protected decimal[,] TeachedMatrix = new decimal[11, 10];

        public virtual void Teach(List<List<List<decimal>>> DataMatrix, int ColumnId)
        {
            decimal SUM = 0;
            for (int i = 0; i < DataMatrix.Count; i++)
            {
                TeachedMatrix[i, i] = -1;
                for (int j = i + 1; j < DataMatrix.Count; j++)
                {
                    TeachedMatrix[i, j] = Calculate(DataMatrix[i][ColumnId], DataMatrix[j][ColumnId]);
                    TeachedMatrix[j, i] = TeachedMatrix[i, j];
                    SUM += TeachedMatrix[i, j];
                }
            }
            for (int i = 0; i < 10; i++)
            {
                for (int j = 0; j < 10; j++)
                    Console.WriteLine(TeachedMatrix[i, j]);
                Console.WriteLine();
            }


            //Console.WriteLine(SUM);
        }

        public virtual double Test(List<List<List<decimal>>> DataMatrix, List<decimal> testedMatrix, int ColumnId)
        {
            double ret = TestMethod(DataMatrix, testedMatrix, ColumnId);
            return CheckTestOutput(ret);
        }

        protected abstract double TestMethod(List<List<List<decimal>>> DataMatrix, List<decimal> testedMatrix, int ColumnId);

        protected abstract decimal Calculate(List<decimal> F, List<decimal> G);
    }
}
