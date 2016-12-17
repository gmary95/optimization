using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace optimization
{
    class Transposition
    {
        public List<int> points;

        public Transposition(int n)
        {
            Random random = new Random();
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
            int tmp = newTransposition[to];
            newTransposition[to] = newTransposition[from];
            newTransposition[from] = tmp;
            return newTransposition;
        }
        public Transposition CreateRandomTranspositionFromEps(int eps, int count)
        {
            Random rnd = new Random();
            Transposition y = new Transposition(points.Count);
            for (int j = 0; j < eps; j++)
            {
                int from = rnd.Next(count);
                int to = rnd.Next(count);
                if (from != to)
                {
                    y.points = Swap(from, to);
                }
            }
            return y;
        }
    }
}
