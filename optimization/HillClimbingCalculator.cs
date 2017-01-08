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
        int eps;

        public HillClimbingCalculator(TSP tsp, int numberOfAttempts, int eps)
        {
            this.tsp = tsp;
            this.numberOfAttempts = numberOfAttempts;
            this.eps = eps;
        }
        public Tour Calculate()
        {
            Tour x = new Tour(tsp.points.Count);
            tsp.Draw(x);
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
                        isFound = true;
                    }
                }         
            } while (!isFound);
            return x;
        }
    }
}
