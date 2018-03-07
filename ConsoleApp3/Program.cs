using System;
using System.Collections.Generic;

namespace OnlineSignitureValidation
{
    class Program
    {
        private static string resPath = System.IO.Directory.GetCurrentDirectory() + @"\resources\";
        private static int userId = 1;
        private static int signId = 11;

        public static string ResPath { get { return resPath; } }
        public static string SignId { get { return String.Format("{0}. userm {1}. Signiture", userId, signId); } }

        static void Main(string[] args)
        {
            List<List<List<decimal>>> teacherMatrix = InPut.readSample.GetTeachers(userId);
            List<List<decimal>>  testedMatrix = InPut.readSample.GetDataFromFile(userId, signId);
            
            Testers.DinamicTimeWrapping DTW = Testers.DinamicTimeWrapping.Instance;
            DTW.Teach(teacherMatrix, 1);
            //Console.WriteLine(DTW.Test(teacherMatrix, testedMatrix));

            for (int j = 1; j < 31; j++)
            {
                testedMatrix = InPut.readSample.GetDataFromFile(userId, 10 + j);
                Console.WriteLine((10+j)+":    "+DTW.Test(teacherMatrix, testedMatrix[1], 1));
            }

            Console.ReadKey();
        }
    }
}
