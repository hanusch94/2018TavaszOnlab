using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;

namespace OnlineSignitureVerification.InPut
{
    enum ResFileName
    {
        sample,
        task1,
        task2
    }

    static class readSample
    {
        private static ResFileName resFileName = ResFileName.task1;
        public static List<List<List<double>>> GetTeachers(int user)
        {
            List<List<List<double>>> ret = new List<List<List<double>>>();

            for (int i = 1; i <= 10; i++)
            {
                ret.Add(GetDataFromFile(user, i));
            }

            return ret;
        }

        public static List<List<double>> GetDataFromFile(int user, int SignId)
        {
            double sampling = 5;

            List<List<double>> ret = new List<List<double>>();
            List<List<double>> dotsFromFile = new List<List<double>>();

            string path;

            switch (resFileName)
            {
                case ResFileName.sample:
                    path = String.Format("{0}USER{1}_{2}.txt", Program.ResPath, user, SignId);
                    break;
                case ResFileName.task1: 
                    path = String.Format("{0}U{1}S{2}.txt", Program.ResPath, user, SignId);
                    break;
                case ResFileName.task2:
                    path = String.Format("{0}U{1}S{2}.txt", Program.ResPath, user, SignId);
                    break;
                default:
                    throw new NotImplementedException("Resource file's name logic");
            }

            System.IO.StreamReader file = new System.IO.StreamReader(path);
            try
            {
                int rows = Int32.Parse(file.ReadLine().Trim());

                dotsFromFile.Add(new List<double>());
                dotsFromFile.Add(GetRowAsdouble(file));
                double norm = dotsFromFile[1][2];
                double timeIter = sampling;
                dotsFromFile[1][2] = 0;

                for (int i = 0; i < dotsFromFile[1].Count; i++)
                {
                    ret.Add(new List<double>());
                    ret[i].Add(dotsFromFile[1][i]);
                }

                for (int i = 1; i<rows; i++)
                {
                    dotsFromFile.RemoveAt(0);
                    dotsFromFile.Add(GetRowAsdouble(file));
                    dotsFromFile[1][2] = dotsFromFile[1][2]-norm;

                    double distance = dotsFromFile[1][2] - dotsFromFile[0][2];

                    while(timeIter <= dotsFromFile[1][2])
                    {
                        double p = (timeIter - dotsFromFile[0][2]) / distance;
                        
                        for (int j = 0; j < ret.Count; j++)
                        {
                            if (j == 2)
                                ret[j].Add(timeIter);
                            else
                            {
                                if (p == 1)
                                    ret[j].Add(dotsFromFile[1][j]);
                                else
                                    ret[j].Add(p * dotsFromFile[0][j] + (1 - p) * dotsFromFile[1][j]);
                            }
                        }

                        timeIter += sampling;
                    }
                    bool stop = true;
                }
            }
            finally { file.Close(); }

            return ret;
        }

        private static List<double> GetRowAsdouble(System.IO.StreamReader file)
        {
            List<double> ret = new List<double>();

            string[] tmp = file.ReadLine().Trim().Split(' ');
            int j = 0;
            foreach (string lm in tmp)
            {
                ret.Add(double.Parse(lm));
                j++;
            }

            return ret;
        }
    }
}
