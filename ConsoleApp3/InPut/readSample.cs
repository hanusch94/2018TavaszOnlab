using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;

namespace OnlineSignitureValidation.InPut
{
    enum ResFileName
    {
        sample,
        task1,
        task2
    }

    static class readSample
    {
        private static ResFileName resFileName = ResFileName.sample;
        public static List<List<List<decimal>>> GetTeachers(int user)
        {
            List<List<List<decimal>>> ret = new List<List<List<decimal>>>();

            for (int i = 1; i <= 10; i++)
            {
                ret.Add(GetDataFromFile(user, i));
            }

            return ret;
        }

        public static List<List<decimal>> GetDataFromFile(int user, int SignId)
        {
            decimal sampling = 5;

            List<List<decimal>> ret = new List<List<decimal>>();
            List<List<decimal>> dotsFromFile = new List<List<decimal>>();

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

                dotsFromFile.Add(new List<decimal>());
                dotsFromFile.Add(GetRowAsdecimal(file));
                decimal norm = dotsFromFile[1][2];
                decimal timeIter = sampling;
                dotsFromFile[1][2] = 0;

                for (int i = 0; i < dotsFromFile[1].Count; i++)
                {
                    ret.Add(new List<decimal>());
                    ret[i].Add(dotsFromFile[1][i]);
                }

                for (int i = 1; i<rows; i++)
                {
                    dotsFromFile.RemoveAt(0);
                    dotsFromFile.Add(GetRowAsdecimal(file));
                    dotsFromFile[1][2] = dotsFromFile[1][2]-norm;

                    decimal distance = dotsFromFile[1][2] - dotsFromFile[0][2];

                    while(timeIter <= dotsFromFile[1][2])
                    {
                        decimal p = (timeIter - dotsFromFile[0][2]) / distance;
                        
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

        private static List<decimal> GetRowAsdecimal(System.IO.StreamReader file)
        {
            List<decimal> ret = new List<decimal>();

            string[] tmp = file.ReadLine().Trim().Split(' ');
            int j = 0;
            foreach (string lm in tmp)
            {
                ret.Add(decimal.Parse(lm));
                j++;
            }

            return ret;
        }
    }
}
