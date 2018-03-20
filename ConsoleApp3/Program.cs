using System;
using System.Collections.Generic;

namespace OnlineSignitureVerification
{
    class Program
    {
        private static string resPath = System.IO.Directory.GetCurrentDirectory() + @"\resources\";
        private static int userId = 5;
        private static int signId = 11;

        public static string ResPath { get { return resPath; } }
        public static string SignId { get { return String.Format("{0}. userm {1}. Signiture", userId, signId); } }

        static void Main(string[] args)
        {
            List<List<List<decimal>>> teacherMatrix = InPut.readSample.GetTeachers(userId);
            List<List<decimal>>  testedMatrix = InPut.readSample.GetDataFromFile(userId, signId);
            
            Testers.DinamicTimeWrapping DTW = Testers.DinamicTimeWrapping.Instance;
            Testers.KolmogorovSmirnovTest KST = Testers.KolmogorovSmirnovTest.Instance;

            Testers.SmallTest.Duration D = Testers.SmallTest.Duration.Instance;



            //KST.Teach(teacherMatrix, 1);
            D.Teach(teacherMatrix);
            
            //Console.WriteLine(DTW.Test(teacherMatrix, testedMatrix));

            for (int j = 1; j < 31; j++)
            {
                testedMatrix = InPut.readSample.GetDataFromFile(userId, 10 + j);
                //Console.WriteLine((10+j)+":    "+KST.Test(teacherMatrix, testedMatrix[1], 1));
                Console.WriteLine((10 + j) + ":    " + D.TestMethod(testedMatrix[1]));
            }

            Console.ReadKey();
        }
    }
}
