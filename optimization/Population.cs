using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace optimization
{
    class Population
    {
        TSP tsp;
        public Tour[] tours;

        public Population(TSP tsp, int populationSize, bool initialise)
        {
            this.tsp = tsp;
            tours = new Tour[populationSize];
            if (initialise)
            {
                for (int i = 0; i < PopulationSize(); i++)
                {
                    Tour newTour = new Tour(tsp.points.Count);
                    tours[i] = newTour;
                }
            }
        }

        public Tour GetTour(int index)
        {
            return tours[index];
        }

        public Tour GetFittest()
        {
            Tour fittest = tours[0];
            for (int i = 1; i < PopulationSize(); i++)
            {
                if (tsp.CalculateFunction(fittest) >= tsp.CalculateFunction(GetTour(i)))
                {
                    fittest = GetTour(i);
                }
            }
            return fittest;
        }

        public int PopulationSize()
        {
            return tours.Length;
        }
    }
}
