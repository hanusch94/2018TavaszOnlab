using System;
using System.Collections.Generic;
using System.Text;

namespace OnlineSignitureVerification.Testers
{
    abstract class Tester<T>
        where T : class
    {        
        protected virtual double CheckTestOutput(double ret)
        {
            if (ret > 1 || ret < 0)
            {
                //Console.WriteLine("At {0}, the test '{1}' failed", Program.SignId, this.GetType().Name);
                ret = 0.5;
            }
            return ret;
        }
    }
}
