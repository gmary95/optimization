using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;

namespace optimization
{
    class HillClimbingCalculator
    {
        TSP tsp;
        int numberOfAttempts;
        public List<Tour> bestTours;

        public HillClimbingCalculator(TSP tsp, int numberOfAttempts)
        {
            this.tsp = tsp;
            this.numberOfAttempts = numberOfAttempts;
            bestTours = new List<Tour>();
        }
        public Tour CalculateFirst()
        {
            Form1 form = (Form1)Application.OpenForms[0];
            Chart chart1 = form.chart1;
            chart1.Series.Clear();
            chart1.Series.Add("line");
            chart1.Series[0].ChartType = SeriesChartType.FastLine;
            Tour x = new Tour(tsp.points.Count);
            //bestTours.Add(x);
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
                        //bestTours.Add(x);
                        chart1.Series[0].Points.AddY(tsp.CalculateFunction(x));
                        isFound = true;
                        break;
                    }
                }
            } while (isFound);
            tsp.Draw(x);
            Console.WriteLine();
            return x;
        }

        public Tour CalculateBest()
        {
            Form1 form = (Form1)Application.OpenForms[0];
            Chart chart1 = form.chart1;
            Tour x = new Tour(tsp.points.Count);
            //bestTours.Add(x);
            bool isFound = false;
            do
            {
                isFound = false;
                Tour y = new Tour(tsp.points.Count);
                Tour min = new Tour(tsp.points.Count);
                min.points = new List<int>(x.points);
                for (int tourPos1 = 0; tourPos1 < x.points.Count - 1; tourPos1++)
                {
                    for (int tourPos2 = tourPos1 + 1; tourPos2 < x.points.Count; tourPos2++)
                    {
                        y.points = x.Swap(tourPos1, tourPos2);
                        if (tsp.CalculateFunction(y) < tsp.CalculateFunction(min))
                        {
                            min.points = y.points;
                        }
                    }
                }
                if (tsp.CalculateFunction(min) < tsp.CalculateFunction(x))
                {
                    x = min;
                    //bestTours.Add(x);
                    chart1.Series[0].Points.AddY(tsp.CalculateFunction(x));
                    isFound = true;
                }
            } while (isFound);
            tsp.Draw(x);
            Console.WriteLine();
            return x;
        }
    }
}
