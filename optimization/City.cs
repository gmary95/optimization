using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace optimization
{
    class City
    {
        public double x { get; }
        public double y { get; }

        public City(double x, double y)
        {
            this.x = x;
            this.y = y;
        }

        public string toString()
        {
            return x.ToString() + ", " + y.ToString();
        }

    }
}
