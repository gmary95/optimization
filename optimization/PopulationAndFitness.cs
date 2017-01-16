using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace optimization
{
    class PopulationAndFitness
    {
        public double fitness;
        public Tour tour;

        public PopulationAndFitness(TSP tsp, Tour tour)
        {
            this.tour = tour;
            fitness = tsp.CalculateFunction(tour);
        }
    }
}
