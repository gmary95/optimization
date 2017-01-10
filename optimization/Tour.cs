using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace optimization
{
    class Tour
    {
        public List<int> points;
        static Random random = new Random();

        public Tour(int n, int elem)
        {
            points = new List<int>();
            for (int i = 0; i < n; i++)
            {
                points.Add(elem);
            }
        }

        public Tour(int n)
        {
            points = new List<int>();
            List<int> digit = new List<int>();
            for (int i = 0; i < n; i++)
            {
                digit.Add(i);
            }
            while (points.Count < n)
            {
                int i = random.Next(digit.Count);
                points.Add(digit[i]);
                digit.RemoveAt(i);
            }
        }

        public List<int> Swap(int from, int to)
        {
            List<int> newTransposition = new List<int>(points);
            /*int tmp = newTransposition[to];
            newTransposition[to] = newTransposition[from];
            newTransposition[from] = tmp;*/
            int size = points.Count;
            for (int c = 0; c <= from - 1; ++c)
            {
                newTransposition[c] = points[c];
            }
            int dec = 0;
            for (int c = from; c <= to; ++c)
            {
                newTransposition[c] = points[to - dec];
                dec++;
            }
            for (int c = to + 1; c < size; ++c)
            {
                newTransposition[c] = points[c];
            }
            return newTransposition;
        }

        public Tour CreateRandomTranspositionFromEps(int eps)
        {
            Tour y = new Tour(points.Count);
            int x1 = 0, x2 = 0;
            while (x1 == x2)
            {
                x1 = (int)(points.Count * random.NextDouble());
                x2 = (int)(points.Count * random.NextDouble());
            }
            int count = 0;
            count = (int)(eps * random.NextDouble());
            for (int i = 0; i < count; i++)
            {
                y.points = Swap(x1, x2);
            }
            return y;
        }
    }
}
