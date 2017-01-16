using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace optimization
{
    class GA
    {
        public class LehaPair
        {
            public int x;
            public int y;
            public LehaPair(int x, int y)
            {
                this.x = x;
                this.y = y;
            }
        }

        public class ParentPairTour
        {
            public PopulationAndFitness x;
            public PopulationAndFitness y;
            public ParentPairTour(PopulationAndFitness x, PopulationAndFitness y)
            {
                this.x = x;
                this.y = y;
            }
        }


        private double mutationRate = 0.3;
        private int tournamentSize = 4;

        TSP tsp;
        static Random random = new Random();
        List<int> rankMap;
        public Population population;

        public GA(TSP tsp, Population population)
        {
            this.tsp = tsp;
            this.population = population;
            this.rankMap = new List<int>(population.PopulationSize());
        }

        public void EvolvePopulation()
        {
            int sumOfRank = CalcSumOfRank(population.PopulationSize());
            ParentPairTour parent = parentSelection(population, sumOfRank);
            Tour child = Crossover(parent.x, parent.y);
            Mutate(child);
            population.ReplaceWorst(new PopulationAndFitness(tsp, child));
        }

        public Tour Crossover(PopulationAndFitness parent1, PopulationAndFitness parent2)
        {
            Tour child = new Tour(tsp.points.Count, -1);


            Dictionary<int, NeighboardCityes> cities = createEdgeRecombinationMatrix(parent1.tour, parent2.tour);
            child.points[0] = (cities.Keys.Select(x => new LehaPair(x, cities[x].neignboardCity.Count)).OrderBy(x => x.y).ToList())[0].x;

            for (int i = 1; i < parent1.tour.points.Count; i++)
            {
                int xBB;
                if (cities[child.points[i - 1]].neignboardCity.Count != 0)
                {
                    List<int> neighboards = cities[child.points[i - 1]].FindNextCity();
                    List<int> xEE = neighboards
                        .GroupBy(x => cities[x].neignboardCity.Count)
                        .OrderBy(x => x.Key)
                        .ToList()[0]
                        .ToList();

                    xBB = xEE[random.Next(xEE.Count)];
                }
                else
                {
                    xBB = cities[child.points[i - 1]].RandomNodeNotInChild(child);
                }

                child.points[i] = xBB;

                for (int j = 0; j < cities.Count; j++)
                {
                    cities[j].DeleteCity(child.points[i - 1]);
                }
            }
            return child;
        }

        private void Mutate(Tour tour)
        {
            if (random.NextDouble() < mutationRate)
            {
                tour = tour.CreateRandomTranspositionFromEps();
            }
        }

        private ParentPairTour TournamentSelection(TSP tsp, Population population)
        {
            Population tournament = new Population(tsp, tournamentSize, false);
            for (int i = 0; i < tournamentSize; i++)
            {
                int randomId = (int)(random.NextDouble() * population.PopulationSize());
                tournament.tours[i] = population.GetTour(randomId);
            }
           
            return tournament.GetFittestLeha();
        }
        private ParentPairTour parentSelection(Population population, int sumOfRank)
        {

            List<PopulationAndFitness> sortedTour = population.tours.OrderBy(x => x.fitness).ToList();

            int topIndex1 = getTopIndex(sumOfRank);
            int topIndex2 = getTopIndex(sumOfRank, topIndex1);

            PopulationAndFitness first = population.GetTour(topIndex1);
            PopulationAndFitness second = population.GetTour(topIndex2);

            return new ParentPairTour(first, second);
        }
        private int getTopIndex(int sumOfRank, int disallowedIndex)
        {
            int topIndex;

            do
            {
                topIndex = getTopIndex(sumOfRank);
            } while (topIndex == disallowedIndex);

            return topIndex;
        }

        private int getTopIndex(int sumOfRank)
        {
            return indexNodeByRank(random.Next(sumOfRank));
        }

        private int indexNodeByRank(int rank)
        {
            for (int i = 0; i < rankMap.Count; i++)
            {
                if (rank < rankMap[i])
                {
                    return i;
                }
            }
            return -1;
        }

        private int CalcSumOfRank(int populationSize)
        {
            int sumOfRank;

            if (populationSize == 1)
            {
                sumOfRank = populationSize;
            }
            else
            {
                sumOfRank = populationSize + CalcSumOfRank(populationSize - 1);
            }

            rankMap.Add(sumOfRank);

            return sumOfRank;
        }

        private Dictionary<int, NeighboardCityes> createEdgeRecombinationMatrix(Tour parent1, Tour parent2)
        {
            Dictionary<int, NeighboardCityes> edgeRecombinationMatrix = new Dictionary<int, NeighboardCityes>();
            for (int i = 0; i < parent1.points.Count; i++)
            {
                if (i == parent1.points.Count - 1)
                {
                    NeighboardCityes cities = new NeighboardCityes();
                    cities.neignboardCity.Add(new NeighboardCity(parent1.points[i - 1], false));
                    cities.neignboardCity.Add(new NeighboardCity(parent1.points[0], false));
                    edgeRecombinationMatrix.Add(parent1.points[i], cities);
                }
                else
                {
                    if (i == 0)
                    {
                        NeighboardCityes cities = new NeighboardCityes();
                        cities.neignboardCity.Add(new NeighboardCity(parent1.points[parent1.points.Count - 1], false));
                        cities.neignboardCity.Add(new NeighboardCity(parent1.points[i + 1], false));
                        edgeRecombinationMatrix.Add(parent1.points[i], cities);
                    }
                    else
                    {
                        NeighboardCityes cities = new NeighboardCityes();
                        cities.neignboardCity.Add(new NeighboardCity(parent1.points[i - 1], false));
                        cities.neignboardCity.Add(new NeighboardCity(parent1.points[i + 1], false));
                        edgeRecombinationMatrix.Add(parent1.points[i], cities);
                    }
                }
            }

            for (int i = 0; i < parent2.points.Count; i++)
            {
                if (i == parent2.points.Count - 1)
                {
                    NeighboardCityes cities = new NeighboardCityes();
                    if (edgeRecombinationMatrix[parent2.points[i]].isContains(parent2.points[i - 1]))
                    {
                        edgeRecombinationMatrix[parent2.points[i]].neignboardCity[edgeRecombinationMatrix[parent2.points[i]].FindCity(parent2.points[i - 1])].isRepeat = true;
                    }
                    else
                    {
                        edgeRecombinationMatrix[parent2.points[i]].neignboardCity.Add(new NeighboardCity(parent2.points[i - 1], false));
                    }
                    if (edgeRecombinationMatrix[parent2.points[i]].isContains(parent2.points[0]))
                    {
                        edgeRecombinationMatrix[parent2.points[i]].neignboardCity[edgeRecombinationMatrix[parent2.points[i]].FindCity(parent2.points[0])].isRepeat = true;
                    }
                    else
                    {
                        edgeRecombinationMatrix[parent2.points[i]].neignboardCity.Add(new NeighboardCity(parent2.points[0], false));

                    }
                }
                else
                {
                    if (i == 0)
                    {
                        NeighboardCityes cities = new NeighboardCityes();
                        if (edgeRecombinationMatrix[parent2.points[i]].isContains(parent2.points[parent2.points.Count - 1]))
                        {
                            edgeRecombinationMatrix[parent2.points[i]].neignboardCity[edgeRecombinationMatrix[parent2.points[i]].FindCity(parent2.points[parent2.points.Count - 1])].isRepeat = true;
                        }
                        else
                        {
                            edgeRecombinationMatrix[parent2.points[i]].neignboardCity.Add(new NeighboardCity(parent2.points[parent2.points.Count - 1], false));
                        }
                        if (edgeRecombinationMatrix[parent2.points[i]].isContains(parent2.points[i + 1]))
                        {
                            edgeRecombinationMatrix[parent2.points[i]].neignboardCity[edgeRecombinationMatrix[parent2.points[i]].FindCity(parent2.points[i + 1])].isRepeat = true;
                        }
                        else
                        {
                            edgeRecombinationMatrix[parent2.points[i]].neignboardCity.Add(new NeighboardCity(parent2.points[i + 1], false));

                        }
                    }
                    else
                    {
                        NeighboardCityes cities = new NeighboardCityes();
                        if (edgeRecombinationMatrix[parent2.points[i]].isContains(parent2.points[i - 1]))
                        {
                            edgeRecombinationMatrix[parent2.points[i]].neignboardCity[edgeRecombinationMatrix[parent2.points[i]].FindCity(parent2.points[i - 1])].isRepeat = true;
                        }
                        else
                        {
                            edgeRecombinationMatrix[parent2.points[i]].neignboardCity.Add(new NeighboardCity(parent2.points[i - 1], false));
                        }
                        if (edgeRecombinationMatrix[parent2.points[i]].isContains(parent2.points[i + 1]))
                        {
                            edgeRecombinationMatrix[parent2.points[i]].neignboardCity[edgeRecombinationMatrix[parent2.points[i]].FindCity(parent2.points[i + 1])].isRepeat = true;
                        }
                        else
                        {
                            edgeRecombinationMatrix[parent2.points[i]].neignboardCity.Add(new NeighboardCity(parent2.points[i + 1], false));

                        }
                    }
                }
            }
            return edgeRecombinationMatrix;
        }
    }
}
