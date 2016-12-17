using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace optimization
{
    public partial class Form1 : Form
    {
        TSP tsp = new TSP();

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
            if (comboBox1.Text == "Hill Climbing")
            {
                int numberOfAttempts = Convert.ToInt32(numericUpDown1.Value);
                int eps = Convert.ToInt32(numericUpDown2.Value);
                
                Stopwatch stopwatch = new Stopwatch();
                stopwatch.Start();
                HillClimbingCalculator hc = new HillClimbingCalculator(tsp, numberOfAttempts, eps);
                Transposition transposition = hc.Calculate();
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
                Transposition transposition = simulatedAnnealing.Calculate();
                stopwatch.Stop();
                label5.Text = stopwatch.ElapsedMilliseconds.ToString();
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            button2.Enabled = true;
            Random rnd = new Random();
            for (int i = 0; i < 1000; i++)
            {
                double x = rnd.Next();
                double y = rnd.Next();
                Points point = new Points(x, y);
                tsp.points.Add(point);
            }
        }

        private void numericUpDown2_ValueChanged(object sender, EventArgs e)
        {

        }
    }
}
