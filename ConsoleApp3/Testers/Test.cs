using System;
using System.Collections.Generic;
using System.Text;

namespace OnlineSignitureValidation.Testers
{
    abstract class Tester<T>
        where T : class
    {
        private static readonly Lazy<T> sInstance = new Lazy<T>(() => CreateInstanceOfT());

        public static T Instance { get { return sInstance.Value; } }

        private static T CreateInstanceOfT()
        {
            return Activator.CreateInstance(typeof(T), true) as T;
        }

        public abstract void Teach(List<List<List<decimal>>> teacherMatrix, int ColumnId);

        public virtual double Test(List<List<List<decimal>>> teacherMatrix, List<decimal> testedMatrix, int ColumnId)
        {
            double ret=TestMethod(teacherMatrix, testedMatrix, ColumnId);

            if (ret > 1 || ret < 0)
            {
                Console.WriteLine("At {0}, the test '{1}' failed", Program.SignId, this.GetType().Name);
                ret = 0.5;
            }
            return ret;
        }

        protected abstract double TestMethod(List<List<List<decimal>>> teacherMatrix, List<decimal> testedMatrix, int ColumnId);
    }
}
