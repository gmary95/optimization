using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace optimization
{
    public partial class Form1 : Form
    {
        TSP tsp;
        List<City> city = new List<City>();

        public Form1()
        {
            InitializeComponent();
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
            if (comboBox1.Text == "Hill Climbing")
            {
                label6.Visible = false;
                label7.Visible = false;
                numericUpDown4.Visible = false;
                textBox1.Visible = false;
            }
            if (comboBox1.Text == "Simulated Annealing")
            {
                label6.Visible = true;
                label7.Visible = true;
                numericUpDown4.Visible = true;
                textBox1.Visible = true;
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
            //ThreadPool.QueueUserWorkItem(new WaitCallback(BeginTsp));
            BeginTsp(null);
           
        }

        void BeginTsp(Object stateInfo)
        {
            if (comboBox1.Text == "Hill Climbing")
            {
                int numberOfAttempts = Convert.ToInt32(numericUpDown1.Value);
                int eps = Convert.ToInt32(numericUpDown2.Value);

                Stopwatch stopwatch = new Stopwatch();
                stopwatch.Start();
                HillClimbingCalculator hc = new HillClimbingCalculator(tsp, numberOfAttempts, eps);
                Tour transposition = hc.Calculate();
                richTextBox1.Text += "Hill climbing solution: " + tsp.CalculateFunction(transposition).ToString() + "\n";
                stopwatch.Stop();
                label5.Text = stopwatch.ElapsedMilliseconds.ToString();
            }
            if (comboBox1.Text == "Simulated Annealing")
            {
                int numberOfAttempts = Convert.ToInt32(numericUpDown1.Value);
                int eps = Convert.ToInt32(numericUpDown2.Value);
                int exitCount = Convert.ToInt32(numericUpDown4.Value);
                double alph = Convert.ToDouble(textBox1.Text);

                Stopwatch stopwatch = new Stopwatch();
                stopwatch.Start();
                SimulatedAnnealing simulatedAnnealing = new SimulatedAnnealing(tsp, numberOfAttempts, eps, alph, exitCount);
                Tour transposition = simulatedAnnealing.Calculate();
                richTextBox1.Text += "Simulated annealing solution: " + tsp.CalculateFunction(transposition).ToString() + "\n";
                stopwatch.Stop();
                label5.Text = stopwatch.ElapsedMilliseconds.ToString();
            }
        }
        private void button1_Click(object sender, EventArgs e)
        {
            /* tsp = new TSP();
             Random rnd = new Random();
             for (int i = 0; i < 1000; i++)
             {
                 double x = rnd.Next();
                 double y = rnd.Next();
                 Points point = new Points(x, y);
                 tsp.points.Add(point);
             }*/
            tsp = null;
            city.Clear();
            this.DrawCityList(city);
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

            foreach (City city in cityList)
            {
                graphics.DrawEllipse(Pens.Black, (float)city.x - 2, (float)city.y - 2, 5, 5);
            }

            this.pictureBox1.Image = cityImage;
        }


        private void numericUpDown2_ValueChanged(object sender, EventArgs e)
        {

        }

        private void pictureBox1_MouseDown(object sender, MouseEventArgs e)
        {
            if (tsp != null)
            {
                //StatusLabel.Text = "Cannot alter city list while running";
                //StatusLabel.ForeColor = Color.Red;
                return;
            }

            city.Add(new City(e.X, e.Y));
            DrawCityList(city);
            richTextBox1.Text = "";
        }

        private void button3_Click(object sender, EventArgs e)
        {
            //if (tsp != null)
            //{
            //MessageBox.Show("Cannot alter city list while running");
            //return;
            //}
            tsp = null;
            city.Clear();
            DrawCityList(city);
        }

        private void openFileDialog1_FileOk(object sender, CancelEventArgs e)
        {

        }

        private void button4_Click(object sender, EventArgs e)
        {
            if (tsp == null)
            {
                tsp = new TSP();
                for (int i = 0; i < city.Count; i++)
                {
                    tsp.points.Add(city[i]);
                }
            }
            Population pop = new Population(tsp, 100, true);
            richTextBox1.Text += "Initial distance: " + tsp.CalculateFunction(pop.GetFittest()) + "\n";
            GA genetic = new GA(tsp);
            //pop = genetic.EvolvePopulation(pop);
            for (int i = 0; i < 100; i++)
            {
                pop = genetic.EvolvePopulation(pop);
                richTextBox1.Text += "y = " + tsp.CalculateFunction(pop.GetFittest()) + "\n";
            }
            richTextBox1.Text += "Final distance: " + tsp.CalculateFunction(pop.GetFittest()) + "\n";
            tsp.Draw(pop.GetFittest());
        }
    }
}
