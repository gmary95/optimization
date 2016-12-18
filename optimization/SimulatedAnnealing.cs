using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace optimization
{
    class SimulatedAnnealing
    {
        TSP tsp;
        int numberOfAttempts;
        int eps, exitConst;
        double alpha;

        public SimulatedAnnealing(TSP tsp, int numberOfAttempts, int eps, double alpha, int exitConst)
        {
            this.tsp = tsp;
            this.numberOfAttempts = numberOfAttempts;
            this.eps = eps;
            this.alpha = alpha;
            this.exitConst = exitConst;
        }

        public Transposition Calculate()
        {
            Transposition x = new Transposition(tsp.points.Count);
            double T = TryTemperature(1,x);
            int accepted = 0;
            int rejected = 0;
            int rejQuasInRaw = 0;
            Random random = new Random();
            bool finished = false;
            do
            {
                Transposition y = x.CreateRandomTranspositionFromEps(eps, tsp.points.Count);
                double dF = tsp.CalculateFunction(y) - tsp.CalculateFunction(x);

                if (random.NextDouble() < Math.Exp(-dF / T))
                {
                    x = y;
                    accepted++;
                }
                else
                {
                    rejected++;
                }

                int quasiEquilibriumReached = QuasiEquilibriumReached(accepted, rejected);
                switch (quasiEquilibriumReached)
                {
                    case 1:
                            accepted = 0;
                            rejected = 0;
                            rejQuasInRaw = 0;
                            T = ApplyPaddingSchedule(T);
                            break;

                    case -1:
                            if (rejQuasInRaw + 1 == exitConst)
                            {
                                finished = true;
                                break;
                            }
                            else
                            {
                                accepted = 0;
                                rejected = 0;
                                rejQuasInRaw++;
                                T = ApplyPaddingSchedule(T);
                                break;
                            }
                    default:
                            break;
                }

            } while (!finished);

            return x;
        }

        private double ApplyPaddingSchedule(double t)
        {
            return t * (1 - alpha);
        }

        private int QuasiEquilibriumReached(long acceptedCounter, long rejectedCounter)
        {
            if (acceptedCounter == eps)
            {
                return 1;
            }
            if (rejectedCounter == 2 * eps)
            {
                return -1;
            }
            return 0;
        }

        private double TryTemperature(double t, Transposition x)
        {
            int acceptedCounter = 0;
            Random random = new Random();

            for (int i = 0; i < numberOfAttempts; i++)
            {
                Transposition y = x.CreateRandomTranspositionFromEps(eps, tsp.points.Count); 
                double dF = tsp.CalculateFunction(y) - tsp.CalculateFunction(x);

                if (random.NextDouble() < Math.Exp(-dF / t))
                {
                    acceptedCounter++;
                }
                double rate = (double)acceptedCounter / numberOfAttempts;

                if (rate >= 0.945 && rate <= 0.955)
                {
                    return t;
                }

                if (rate < 0.945)
                {
                    t = (1 - alpha) / t;
                }
                else
                {
                    t = (1 - alpha) * t;
                }
            }

            return t;
        }
    }
}
