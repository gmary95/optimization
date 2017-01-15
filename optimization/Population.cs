using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static optimization.GA;

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
        public LehaPairTour GetFittestLeha()
        {
            SortFittest();

            return new LehaPairTour(tours[0], tours[1]);
        }

        public Tour GetFittest()
        {
            SortFittest();

            return tours[0];
        }

        public void SortFittest()
        {
            Array.Sort(tours, (x, y) =>
            {
                double d = (tsp.CalculateFunction(x) - tsp.CalculateFunction(y));
                return d < 0 ? -1 : d == 0 ? 0 : 1;
            });
        }

        public void ReplaceWorst(Tour tour)
        {
            Tour worst = tours[0];
            int index = 0;
            for (int i = 1; i < PopulationSize(); i++)
            {
                if (tsp.CalculateFunction(worst) <= tsp.CalculateFunction(GetTour(i)))
                {
                    worst = GetTour(i);
                    index = i;
                }
            }
            if (tsp.CalculateFunction(worst) >= tsp.CalculateFunction(tour))
            {
                tours[index] = tour;
            }
        }

        public int PopulationSize()
        {
            return tours.Length;
        }
    }
}
