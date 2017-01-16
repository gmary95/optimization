using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;

namespace optimization
{
    class SimulatedAnnealing
    {
        TSP tsp;
        int numberOfAttempts;
        int exitConst;
        double alpha;
        public List<Tour> bestTours;
        static Random random = new Random();
        [System.Runtime.InteropServices.DllImport("kernel32.dll")]
        private static extern bool AllocConsole();

        public SimulatedAnnealing(TSP tsp, int numberOfAttempts, double alpha, int exitConst)
        {
            this.tsp = tsp;
            this.numberOfAttempts = numberOfAttempts;
            this.alpha = alpha;
            this.exitConst = exitConst;
            bestTours = new List<Tour>();
            //AllocConsole();
        }

        public Tour Calculate()
        {
            Form1 form = (Form1)Application.OpenForms[0];
            Chart chart1 = form.chart1;
            Tour x = new Tour(tsp.points.Count);
            //bestTours.Add(x);
            double T = 1000;// TryTemperature(1,x);
            int accepted = 0;
            int rejected = 0;
            int rejQuasInRaw = 0;
            bool finished = false;
            do
            {
                Tour y = x.CreateRandomTranspositionFromEps();
                double dF = tsp.CalculateFunction(y) - tsp.CalculateFunction(x);

                if (random.NextDouble() < Math.Exp(-dF / T))
                {
                    x = y;
                    //Console.Clear();
                    //for (int j = 0; j < x.points.Count; j++)
                    //{
                    //    Console.Write(x.points[j].ToString());
                    //}
                    //Console.WriteLine();
                    //bestTours.Add(x);

                    chart1.Series[0].Points.AddY(tsp.CalculateFunction(x));
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
            tsp.Draw(x);
            return x;
        }

        private double ApplyPaddingSchedule(double t)
        {
            return t * (1 - alpha);
        }

        private int QuasiEquilibriumReached(long acceptedCounter, long rejectedCounter)
        {
            double nEps = (int)((tsp.points.Count * (tsp.points.Count - 1)) / 2);
            if (acceptedCounter == nEps)
            {
                return 1;
            }
            if (rejectedCounter == 2 * nEps)
            {
                return -1;
            }
            return 0;
        }

        private double TryTemperature(double t, Tour x)
        {
            int acceptedCounter = 0;

            for (int i = 0; i < numberOfAttempts; i++)
            {
                Tour y = x.CreateRandomTranspositionFromEps(); 
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
                    t = t / (1 - alpha);
                }
                else
                {
                    t = t * (1 - alpha);
                }
            }

            return t;
        }
    }
}
