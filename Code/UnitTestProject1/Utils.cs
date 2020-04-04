using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UnitTestProject1
{
    public class DoubleComparer : System.Collections.IComparer
    {
        private readonly double _epsilon;

        public DoubleComparer(double epsilon)
        {
            _epsilon = epsilon;
        }

        public int Compare(object x, object y)
        {
            var a = (double)x;
            var b = (double)y;

            double delta = Math.Abs(a - b);
            if (delta < _epsilon)
            {
                return 0;
            }
            return a.CompareTo(b);
        }
    }
}
