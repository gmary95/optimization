using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace optimization
{
    class GA
    {
        private double mutationRate = 0.9;
        private int tournamentSize = 4;

        TSP tsp;
        static Random random = new Random();

        public GA(TSP tsp)
        {
            this.tsp = tsp;
        }

        public Population EvolvePopulation(Population population)
        {
            Population newPopulation = new Population(tsp, population.PopulationSize(), false);

                newPopulation.tours[0] = population.GetFittest();

            for (int i = 1; i < newPopulation.PopulationSize(); i++)
            {
                Tour parent1 = TournamentSelection(tsp, population);
                Tour parent2 = TournamentSelection(tsp, population);
                Tour child = Crossover(parent1, parent2);
                newPopulation.tours[i] = child;
            }

            for (int i = 1; i < newPopulation.PopulationSize(); i++)
            {
                Mutate(newPopulation.GetTour(i));
            }

            return newPopulation;
        }

        public Tour Crossover(Tour parent1, Tour parent2)
        {
            Tour child = new Tour(tsp.points.Count, -1);

            int startPos = (int)(random.NextDouble() * parent1.points.Count);
            int endPos = (int)(random.NextDouble() * parent1.points.Count);

            for (int i = 0; i < child.points.Count; i++)
            {
                if (startPos < endPos && i > startPos && i < endPos)
                {
                    child.points[i] = parent1.points[i];
                } 
                else if (startPos > endPos)
                {
                    if (!(i < startPos && i > endPos))
                    {
                        child.points[i] = parent1.points[i];
                    }
                }
            }

            for (int i = 0; i < parent2.points.Count; i++)
            {
                if (!child.points.Contains(parent2.points[i]))
                {
                    for (int ii = 0; ii < child.points.Count; ii++)
                    {
                        if (child.points[ii] == -1)
                        {
                            child.points[ii] = parent2.points[i];
                            break;
                        }
                    }
                }
            }
            return child;
        }

        private void Mutate(Tour tour)
        {
            for (int tourPos1 = 0; tourPos1 < tour.points.Count; tourPos1++)
            {
                if (random.NextDouble() < mutationRate)
                {
                    int tourPos2 = (int)(tour.points.Count * random.NextDouble());
                    tour.Swap(tourPos1, tourPos2);
                }
            }
        }

        private Tour TournamentSelection(TSP tsp, Population population)
        {
            Population tournament = new Population(tsp, tournamentSize, false);
            for (int i = 0; i < tournamentSize; i++)
            {
                int randomId = (int)(random.NextDouble() * population.PopulationSize());
                tournament.tours[i] = population.GetTour(randomId);
            }
            Tour fittest = tournament.GetFittest();
            return fittest;
        }
    }
}
