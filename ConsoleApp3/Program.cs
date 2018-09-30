using System;
using System.Collections.Generic;
using System.IO;
using OnlineSignitureVerification.Testers.SmallTest;
using OnlineSignitureVerification.Testers;
using StatMathLib;

namespace OnlineSignitureVerification
{
    public enum ResFileName
    {
        sample,
        task1,
        task2
    }
    
    class Program
    {
        public static ResFileName resFileName = ResFileName.task1;

        private static string resPath = Directory.GetCurrentDirectory() + @"\resources\";
        private static int userId = 5;
        private static int signId = 40;

        public static string ResPath { get { return resPath; } }
        public static string SignId { get { return String.Format("{0}. userm {1}. Signiture", userId, signId); } }

        static void Main(string[] args)
        {
            //weightRun();
            MainRun(args);
        }

        static void weightRun()
        {
            DinamicTimeWrapping DTW = new DinamicTimeWrapping();
            KolmogorovSmirnovTest KST = new KolmogorovSmirnovTest();
            Duration D = new Duration();

            double[,,] dtwRes = new double[3, 1600, 40];
            double[,,] kstRes = new double[3, 1600, 40];
            double[,] durRes = new double[1600, 40];

            for(int user = 1; user<= 40; user++)
            {
                for(int signId = 1; signId <= 20; signId++)
                {
                    List<List<double>> sign = InPut.readSample.GetDataFromFile(user, signId);
                    for(int signId2 = 1; signId2 <= 40; signId2++)
                    {
                        List<List<double>> sign2 = InPut.readSample.GetDataFromFile(user, signId2);
                        durRes[user * 40 + signId, signId2] = D.calculate(sign, sign2);
                        for(int column=0; column<2; column++)
                        {
                            kstRes[user * 40 + signId, signId2, column] = KST.Calculate(sign[column], sign2[column]);
                            dtwRes[user * 40 + signId, signId2, column] = DTW.Calculate(sign[column], sign2[column]);
                        }
                    }
                }
            }
            

            //TODO: for lm in user, sign
                // for i in user, sign
                    //count value for KST, DTW, Dur, PenUp
           //TODO: PLOT
        }

        static void MainRun(string[] args)
        {
            double FINAL_maxbad = 0;
            double FINAL_mingood = 1;

            int userCount = 40;

            double falsePozitive = 0;
            double falseNegative = 0;
            double kstFP = 0;
            double kstFN = 0;
            double dtwFP = 0;
            double dtwFN = 0;
            double durFP = 0;
            double durFN = 0;

            List<List<List<double>>> teacherMatrix;
            List<List<double>> testedMatrix;

            DinamicTimeWrapping DTW = new DinamicTimeWrapping();
            KolmogorovSmirnovTest KST = new KolmogorovSmirnovTest();

            Duration D = new Duration();


            for (int i = 1; i <= userCount; i++)
            {
                teacherMatrix = InPut.readSample.GetTeachers(userId);

                KST.Teach(teacherMatrix, 2);
                DTW.Teach(teacherMatrix, 2);
                D.Teach(teacherMatrix);
                for (int j = 1; j < 31; j++)
                {
                    double kstRET = 0.5;
                    double dtwRET = 0.5;
                    double durRET = 0.5;
                    double bayes = 0.5;

                    testedMatrix = InPut.readSample.GetDataFromFile(userId, 10 + j);

                    kstRET = KST.Test(teacherMatrix, testedMatrix[2], 2);
                    dtwRET = DTW.Test(teacherMatrix, testedMatrix[2], 2);
                    durRET = D.TestMethod(testedMatrix);

                    //KST.Teach(kstTMatrix2);
                    //DTW.Teach(dtwTMatrix2);

                    //kstRET = StatMathLib.LikelihoodFusion.BayesTrap(KST.Test(teacherMatrix, testedMatrix[2], 2), kstRET);
                    //dtwRET = StatMathLib.LikelihoodFusion.BayesTrap(DTW.Test(teacherMatrix, testedMatrix[2], 2), dtwRET);

                    bayes = LikelihoodFusion.BayesTrap(kstRET, LikelihoodFusion.BayesTrap(dtwRET, durRET));

                    if (j <= 10)
                    {
                        if (FINAL_mingood > bayes) FINAL_mingood = bayes;

                        if (kstRET < 0.5) kstFN++;
                        if (dtwRET < 0.5) dtwFN++;
                        if (durRET < 0.5) durFN++;
                        if (bayes < 0.5) falseNegative++;
                    }
                    else
                    {
                        if (FINAL_maxbad < bayes) FINAL_maxbad = bayes;

                        if (kstRET >= 0.5) kstFP++;
                        if (dtwRET >= 0.5) dtwFP++;
                        if (durRET >= 0.5) durFP++;
                        if (bayes >= 0.5) falsePozitive++;
                    }

                    Console.WriteLine("U:" + i + " S:" + j + "   KST: " + kstRET + "  DTW: " + dtwRET + "  Dur: " + durRET + "   FINAL: " + bayes);
                    if (j == 10) Console.WriteLine();
                }

                Console.WriteLine("\n");
            }

            Console.WriteLine("KST FN: " + kstFN / (10 * userCount));
            Console.WriteLine("KST FP: " + kstFP / (20 * userCount));
            Console.WriteLine("KST sum:  " + (2 * kstFN + kstFP) / (40 * userCount) + "\n");

            Console.WriteLine("DTW FN: " + dtwFN / (10 * userCount));
            Console.WriteLine("DTW FP: " + dtwFP / (20 * userCount));
            Console.WriteLine("DTW sum:  " + (2 * dtwFN + dtwFP) / (40 * userCount) + "\n");

            Console.WriteLine("KST FN: " + durFN / (10 * userCount));
            Console.WriteLine("KST FP: " + durFP / (20 * userCount));
            Console.WriteLine("KST sum:  " + (2 * durFN + durFP) / (40 * userCount) + "\n");

            Console.WriteLine("____BAYES_____");
            Console.WriteLine("FN: " + falseNegative / (10 * userCount));
            Console.WriteLine("FP: " + falsePozitive / (20 * userCount));
            Console.WriteLine("sum:  " + (2 * falseNegative + falsePozitive) / (40 * userCount) + "\n");

            Console.WriteLine("________________________________________________");
            Console.WriteLine("FINAL_mingood" + FINAL_mingood);
            Console.WriteLine("FINAL_maxbad" + FINAL_maxbad);

            Console.ReadKey();
        }

        private bool halfChance(double min, double value, double max)
        {
            return 0.45 < value && value < 55;
        }
    }
}
