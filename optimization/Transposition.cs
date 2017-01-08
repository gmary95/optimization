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
        public Transposition CreateRandomTranspositionFromEps()
        {
            Random rnd = new Random();
            Transposition y = new Transposition(points.Count);
            int x1 = 0, x2 = 0;
            while (x1 == x2)
            {
                x1 = (int)(points.Count * rnd.NextDouble());
                x2 = (int)(points.Count * rnd.NextDouble());
            }
            y.points = Swap(x1, x2);
            return y;
        }
    }
}
