using System;
using System.Collections.Generic;
using System.Text;

namespace OnlineSignitureVerification.Testers
{
    abstract class TwoCompareTester<T> : Tester<T>
        where T: class
    {
        protected double[,] TeachedMatrix = new double[11, 10];

        public virtual void Teach(double[,] TeachMatrix)
        {
            TeachedMatrix = TeachMatrix;
        }

        public virtual double[,] Teach(List<List<List<double>>> DataMatrix, int ColumnId)
        {
            double SUM = 0;
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
            //for (int i = 0; i < 10; i++)
            //{
            //    for (int j = 0; j < 10; j++) ;
            //        Console.WriteLine(TeachedMatrix[i, j]);
            //    Console.WriteLine();
            //}


            //Console.WriteLine(SUM);

            return TeachedMatrix;
        }

        public virtual double Test(List<List<List<double>>> DataMatrix, List<double> testedMatrix, int ColumnId)
        {
            double ret = TestMethod(DataMatrix, testedMatrix, ColumnId);
            return CheckTestOutput(ret);
        }

        //TODO: Test
        protected abstract double TestMethod(List<List<List<double>>> DataMatrix, List<double> testedMatrix, int ColumnId);

        //TODO: CalculateDistance, GetDistance 
        protected abstract double Calculate(List<double> F, List<double> G);
    }
}
