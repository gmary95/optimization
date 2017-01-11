using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace optimization
{
    class HillClimbingCalculator
    {
        TSP tsp;
        int numberOfAttempts;
        public List<Tour> bestTours;

        public HillClimbingCalculator(TSP tsp, int numberOfAttempts)
        {
            this.tsp = tsp;
            this.numberOfAttempts = numberOfAttempts;
            bestTours = new List<Tour>();
        }
        public Tour Calculate()
        {
            Tour x = new Tour(tsp.points.Count);
            bestTours.Add(x);
            bool isFound = false;
            do
            {
                isFound = false;
                for (int i = 0; i < numberOfAttempts; i++)
                {
                    Tour y = x.CreateRandomTranspositionFromEps();
                    if (tsp.CalculateFunction(y) < tsp.CalculateFunction(x))
                    {
                        x = y;
                        bestTours.Add(x);
                        isFound = true;
                        break;
                    }
                }
            } while (isFound);
            tsp.Draw(x);
            return x;
        }
    }
}
