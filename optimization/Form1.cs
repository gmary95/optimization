using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;

namespace optimization
{
    public partial class Form1 : Form
    {
        TSP tsp;
        List<Tour> sameAnswer;
        List<City> city = new List<City>();

        public Form1()
        {
            InitializeComponent();
            label5.Text = "";
            label8.Text = "";
        }

        private void openToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void HillClim_Click(object sender, EventArgs e)
        {

        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (comboBox1.Text == "Hill Climbing(best)")
            {
                label2.Visible = false;
                label6.Visible = false;
                label7.Visible = false;
                numericUpDown1.Visible = false;
                numericUpDown4.Visible = false;
                textBox1.Visible = false;
            }
            if (comboBox1.Text == "Hill Climbing(first)")
            {
                label2.Visible = true;
                label6.Visible = false;
                label7.Visible = false;
                numericUpDown1.Visible = true;
                numericUpDown4.Visible = false;
                textBox1.Visible = false;
            }
            if (comboBox1.Text == "Simulated Annealing")
            {
                label2.Visible = true;
                label6.Visible = true;
                label7.Visible = true;
                label7.Text = "Number For Exit";
                numericUpDown1.Visible = true;
                numericUpDown4.Visible = true;
                textBox1.Visible = true;
            }
            if (comboBox1.Text == "Genetic Algorithm")
            {
                label2.Visible = true;
                label6.Visible = false;
                label7.Visible = true;
                label7.Text = "Population Size";
                numericUpDown1.Visible = true;
                numericUpDown4.Visible = true;
                textBox1.Visible = false;

            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (tsp == null)
            {
                tsp = new TSP();
                for (int i = 0; i < city.Count; i++)
                {
                    tsp.points.Add(city[i]);
                }
            }
            tsp.InitDistanceMatrix();
            dataGridView1.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.EnableResizing;
            dataGridView1.RowHeadersWidthSizeMode = DataGridViewRowHeadersWidthSizeMode.EnableResizing; 
            dataGridView1.ColumnCount = tsp.points.Count;
            dataGridView1.RowCount = tsp.points.Count;
            for (int i = 0; i < tsp.points.Count; i++)
            {
                dataGridView1.Rows[i].HeaderCell.Value = (i + 1).ToString();
                for (int j = 0; j < tsp.points.Count; j++)
                {
                    dataGridView1.Columns[j].HeaderCell.Value = (j + 1).ToString();
                    dataGridView1.Rows[i].Cells[j].Value = tsp.distanceMatrix[i, j];
                }
            }
            BeginTsp(null);
        }

        void BeginTsp(Object stateInfo)
        {
            sameAnswer = new List<Tour>();
            comboBox2.Items.Clear();
            chart1.Series.Clear();
            chart1.Series.Add("line");
            chart1.Series[0].ChartType = SeriesChartType.FastLine;
            chart1.Series[0].IsVisibleInLegend = false;
            if (comboBox1.Text == "Hill Climbing(best)")
            {
                int numberOfAttempts = Convert.ToInt32(numericUpDown1.Value);

                Stopwatch stopwatch = new Stopwatch();
                stopwatch.Start();
                HillClimbingCalculator hc = new HillClimbingCalculator(tsp, numberOfAttempts);
                Tour transposition = hc.CalculateBest();
                sameAnswer = hc.bestTours;
                richTextBox1.Text += "Hill climbing(best) solution: " + tsp.CalculateFunction(transposition).ToString() + "\n";
                stopwatch.Stop();
                label5.Text = stopwatch.ElapsedMilliseconds.ToString();
            }
            if (comboBox1.Text == "Hill Climbing(first)")
            {
                int numberOfAttempts = Convert.ToInt32(numericUpDown1.Value);

                Stopwatch stopwatch = new Stopwatch();
                stopwatch.Start();
                HillClimbingCalculator hc = new HillClimbingCalculator(tsp, numberOfAttempts);
                Tour transposition = hc.CalculateFirst();
                sameAnswer = hc.bestTours;
                richTextBox1.Text += "Hill climbing(first) solution: " + tsp.CalculateFunction(transposition).ToString() + "\n";
                stopwatch.Stop();
                label5.Text = stopwatch.ElapsedMilliseconds.ToString();
            }
            if (comboBox1.Text == "Simulated Annealing")
            {
                int numberOfAttempts = Convert.ToInt32(numericUpDown1.Value);
                int exitCount = Convert.ToInt32(numericUpDown4.Value);
                double alph = Convert.ToDouble(textBox1.Text);

                Stopwatch stopwatch = new Stopwatch();
                stopwatch.Start();
                SimulatedAnnealing simulatedAnnealing = new SimulatedAnnealing(tsp, numberOfAttempts, alph, exitCount);
                Tour transposition = simulatedAnnealing.Calculate();
                sameAnswer = simulatedAnnealing.bestTours;
                richTextBox1.Text += "Simulated annealing solution: " + tsp.CalculateFunction(transposition).ToString() + "\n";
                stopwatch.Stop();
                label5.Text = stopwatch.ElapsedMilliseconds.ToString();
            }
            if (comboBox1.Text == "Genetic Algorithm")
            {
                int numberOfAttempts = Convert.ToInt32(numericUpDown1.Value);
                int populationSize = Convert.ToInt32(numericUpDown4.Value);

                Stopwatch stopwatch = new Stopwatch();
                stopwatch.Start();
                Population pop = new Population(tsp, populationSize, true);
                PopulationAndFitness bestTour = pop.GetFittest();
                double fitness = bestTour.fitness;
                //chart1.Series.Add("worst");
                //chart1.Series[1].ChartType = SeriesChartType.FastLine;
                richTextBox1.Text += "Genetic algorithm solution:\nInitial distance: " + fitness + "\n";
                //sameAnswer.Add(pop.GetFittest());
                GA genetic = new GA(tsp, pop);
                for (int i = 0; i < numberOfAttempts; i++)
                {
                    genetic.population = pop;
                    genetic.EvolvePopulation();
                    //bestTour = pop.GetFittest();
                    //sameAnswer.Add(bestTour);
                    //richTextBox1.Text += "y = " + bestTour.fitness + "\n";
                }
                richTextBox1.Text += "Final distance: " + pop.GetFittest().fitness + "\n";
                tsp.Draw(pop.GetFittest().tour);
                stopwatch.Stop();
                label5.Text = stopwatch.ElapsedMilliseconds.ToString();
            }
            if (sameAnswer.Count != 0)
            {
                for (int i = 0; i < sameAnswer.Count; i++)
                {
                    comboBox2.Items.Add(i);
                }
            }
        }
        private void button1_Click(object sender, EventArgs e)
        {
            tsp = null;
            city.Clear();
            this.DrawCityList(city);
            dataGridView1.Columns.Clear();
            dataGridView1.Rows.Clear();
            Stream myStream = null;

            openFileDialog1.FilterIndex = 2;
            openFileDialog1.RestoreDirectory = true;

            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    if ((myStream = openFileDialog1.OpenFile()) != null)
                    {
                        using (myStream)
                        {
                            ReadArray(openFileDialog1.FileName);
                            richTextBox1.Text = "";
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error: Could not read file from disk. Original error: " + ex.Message);
                }
            }
        }
        private void ReadArray(string path)
        {
            using (StreamReader sr = new StreamReader(path))
            {
                string[] lines = File.ReadAllLines(path);
                for (int i = 0; i < lines.Length; i++)
                {
                    string[] elem = lines[i].Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
                    elem[1] = elem[1].Replace('.', ',');
                    elem[2] = elem[2].Replace('.', ',');
                    City point = new City(Convert.ToDouble(elem[1]), Convert.ToDouble(elem[2]));
                    city.Add(point);
                }
                DrawCityList(city);
            }
        }
        private void DrawCityList(List<City> cityList)
        {
            Image cityImage = new Bitmap(pictureBox1.Width, pictureBox1.Height);
            Graphics graphics = Graphics.FromImage(cityImage);

            int k = 0;
            foreach (City city in cityList)
            {
                if (cityList.Count < 50)
                {
                    k++;
                    graphics.DrawString(k.ToString(), new Font("Tahoma", 8), Brushes.Black, (float)city.x, (float)city.y);
                }
                graphics.DrawEllipse(Pens.Black, (float)city.x - 2, (float)city.y - 2, 5, 5);
            }

            this.pictureBox1.Image = cityImage;
        }

        private List<City> ScaleAndMove(List<City> cityList)
        {
            List<City> newCityList = new List<City>(cityList);

            return newCityList;
        }


        private void numericUpDown2_ValueChanged(object sender, EventArgs e)
        {

        }

        private void pictureBox1_MouseDown(object sender, MouseEventArgs e)
        {
            if (tsp != null)
            {
                return;
            }

            city.Add(new City(e.X, e.Y));
            DrawCityList(city);
            richTextBox1.Text = "";
        }

        private void button3_Click(object sender, EventArgs e)
        {
            tsp = null;
            city.Clear();
            DrawCityList(city);
            dataGridView1.Columns.Clear();
            dataGridView1.Rows.Clear();
        }

        private void openFileDialog1_FileOk(object sender, CancelEventArgs e)
        {

        }

        private void button4_Click(object sender, EventArgs e)
        {

        }

        private void comboBox2_SelectedIndexChanged(object sender, EventArgs e)
        {
            tsp.Draw(sameAnswer[comboBox2.SelectedIndex]);
            label8.Text = Math.Round(tsp.CalculateFunction(sameAnswer[comboBox2.SelectedIndex]), 6).ToString();
        }

        private void textBox1_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar) && (e.KeyChar != ','))
            {
                e.Handled = true;
            }

            if ((e.KeyChar == '.') && ((sender as TextBox).Text.IndexOf('.') > -1))
            {
                e.Handled = true;
            }
        }

        private void label7_Click(object sender, EventArgs e)
        {

        }

        private void numericUpDown4_ValueChanged(object sender, EventArgs e)
        {

        }

        private void tabControl1_SelectedIndexChanged(object sender, EventArgs e)
        {
            //if (tabControl1.SelectedIndex == 1)
            //{
            //    if (sameAnswer != null)
            //    {
            //        chart1.Series[0].Points.Clear();
            //        chart1.Series[0].ChartType = SeriesChartType.FastLine;
            //        for (int i = 0; i < sameAnswer.Count; i++)
            //        {
            //            chart1.Series[0].Points.AddY(tsp.CalculateFunction(sameAnswer[i]));
            //        }
            //    }
            //}
        }

        private void button4_Click_1(object sender, EventArgs e)
        {
            saveFileDialog1.ShowDialog();

            if (saveFileDialog1.FileName != "")
            {
                List<string> linesToWrite = new List<string>();
                for (int colIndex = 0; colIndex < city.Count; colIndex++)
                {
                    linesToWrite.Add((colIndex+1).ToString() + " " + city[colIndex].x.ToString() + " " + city[colIndex].y.ToString());
                }

                File.WriteAllLines(saveFileDialog1.FileName, linesToWrite.ToArray());
            }
        }
    }
}
