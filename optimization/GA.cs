using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace optimization
{
    class GA
    {
        private double mutationRate = 0.3;
        private int tournamentSize = 4;

        TSP tsp;
        static Random random = new Random();
        List<int> rankMap;

        public GA(TSP tsp)
        {
            this.tsp = tsp;
        }

        public void EvolvePopulation(Population population)
        {
            this.rankMap = new List<int>(population.PopulationSize());
            int sumOfRank = CalcSumOfRank(population.PopulationSize());
            LehaPairTour parent = parentSelection(population, sumOfRank);
            Tour child = Crossover(parent.x, parent.y);
            Mutate(child);
            population.ReplaceWorst(child);
        }

        public Tour Crossover(Tour parent1, Tour parent2)
        {
            Tour child = new Tour(tsp.points.Count, -1);


            Dictionary<int, NeighboardCityes> cities = createEdgeRecombinationMatrix(parent1, parent2);
            child.points[0] = (cities.Keys.Select(x => new LehaPair(x, cities[x].neignboardCity.Count)).OrderBy(x => x.y).ToList())[0].x;

            for (int i = 1; i < parent1.points.Count; i++)
            {
                List<int> neighboards = cities[child.points[i - 1]].FindNextCity();
                int xBB;
                try
                {
                    List<int> xEE =
                        neighboards
                        .GroupBy(x => cities[x].neignboardCity.Count)
                        .OrderBy(x => x.Key)
                        .ToList()[0]
                        .ToList();

                     xBB = xEE[random.Next(xEE.Count)];
                }
                catch (Exception)
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

        public class LehaPairTour
        {
            public Tour x;
            public Tour y;
            public LehaPairTour(Tour x, Tour y)
            {
                this.x = x;
                this.y = y;
            }
        }

        private void Mutate(Tour tour)
        {
            if (random.NextDouble() < mutationRate)
            {
                tour = tour.CreateRandomTranspositionFromEps();
            }
        }

        private LehaPairTour TournamentSelection(TSP tsp, Population population)
        {
            Population tournament = new Population(tsp, tournamentSize, false);
            for (int i = 0; i < tournamentSize; i++)
            {
                int randomId = (int)(random.NextDouble() * population.PopulationSize());
                tournament.tours[i] = population.GetTour(randomId);
            }
           
            return tournament.GetFittestLeha();
        }
        private LehaPairTour parentSelection(Population population, int sumOfRank)
        {

            List<Tour> sortedTour = population.tours.OrderBy(x => tsp.CalculateFunction(x)).ToList();

            int topIndex1 = getTopIndex(sumOfRank);
            int topIndex2 = getTopIndex(sumOfRank, topIndex1);

            Tour first = population.GetTour(topIndex1);
            Tour second = population.GetTour(topIndex2);

            return new LehaPairTour(first, second);
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
                            edgeRecombinationMatrix[parent2.points[i]].neignboardCity.Add(new NeighboardCity(parent2.points[i + 1], edgeRecombinationMatrix[parent2.points[i]].isContains(parent2.points[i + 1])));

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
                            edgeRecombinationMatrix[parent2.points[i]].neignboardCity.Add(new NeighboardCity(parent2.points[i - 1], edgeRecombinationMatrix[parent2.points[i]].isContains(parent2.points[i - 1])));
                        }
                        if (edgeRecombinationMatrix[parent2.points[i]].isContains(parent2.points[i + 1]))
                        {
                            edgeRecombinationMatrix[parent2.points[i]].neignboardCity[edgeRecombinationMatrix[parent2.points[i]].FindCity(parent2.points[i + 1])].isRepeat = true;
                        }
                        else
                        {
                            edgeRecombinationMatrix[parent2.points[i]].neignboardCity.Add(new NeighboardCity(parent2.points[i + 1], edgeRecombinationMatrix[parent2.points[i]].isContains(parent2.points[i + 1])));

                        }
                    }
                }
            }
            return edgeRecombinationMatrix;
        }
    }
}
