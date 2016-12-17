using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace optimization
{
    class TSP
    {
        public List<Points> points = new List<Points>();
        public double CalculateFunction(Transposition x)
        {
            double f = 0;
            for (int i = 0; i < x.points.Count - 1; i++)
            {
                f += Math.Sqrt(Math.Pow(points[x.points[i]].x - points[x.points[i + 1]].x, 2) * Math.Pow(points[x.points[i]].y - points[x.points[i + 1]].y, 2));
            }
            return f;
        }
    }
}
