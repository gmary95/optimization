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
        public PopulationAndFitness[] tours;

        public Population(TSP tsp, int populationSize, bool initialise)
        {
            this.tsp = tsp;
            tours = new PopulationAndFitness[populationSize];
            if (initialise)
            {
                for (int i = 0; i < PopulationSize(); i++)
                {
                    Tour newTour = new Tour(tsp.points.Count);
                    tours[i] = new PopulationAndFitness(tsp, newTour);
                }
            }
        }

        public PopulationAndFitness GetTour(int index)
        {
            return tours[index];
        }
        public ParentPairTour GetFittestLeha()
        {
            SortFittest();

            return new ParentPairTour(tours[0], tours[1]);
        }

        public PopulationAndFitness GetFittest()
        {
            SortFittest();
            return tours[0];
        }

        public void SortFittest()
        {
            Array.Sort(tours, (x, y) =>
            {
                double d = (x.fitness - y.fitness);
                return d < 0 ? -1 : d == 0 ? 0 : 1;
            });
        }

        public void ReplaceWorst(PopulationAndFitness tour)
        {
            SortFittest();
            if (tours[tours.Length - 1].fitness >= tour.fitness)
            {
                tours[tours.Length - 1] = tour;
            }
        }

        public int PopulationSize()
        {
            return tours.Length;
        }
    }
}
