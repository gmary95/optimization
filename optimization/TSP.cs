﻿using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace optimization
{
    class TSP
    {
        public List<City> points = new List<City>();
        Image cityImage;
        Graphics cityGraphics;
        public double CalculateFunction(Transposition x)
        {
            double f = 0;
            for (int i = 0; i < x.points.Count; i++)
            {
                if (i == x.points.Count - 1)
                {
                    f += Math.Sqrt(Math.Pow(points[x.points[i]].x - points[x.points[0]].x, 2) * Math.Pow(points[x.points[i]].y - points[x.points[0]].y, 2));
                }
                else
                {
                    f += Math.Sqrt(Math.Pow(points[x.points[i]].x - points[x.points[i + 1]].x, 2) * Math.Pow(points[x.points[i]].y - points[x.points[i + 1]].y, 2));
                }
            }
            return f;
        }
        public void Draw(Transposition x)
        {
            Form1 form = (Form1)Application.OpenForms[0];
            if (cityImage == null)
            {
                cityImage = new Bitmap(form.pictureBox1.Width, form.pictureBox1.Height);
                cityGraphics = Graphics.FromImage(cityImage);
            }
     
            cityGraphics.FillRectangle(Brushes.White, 0, 0, cityImage.Width, cityImage.Height);
            foreach (City point in points)
            {
                // Draw a circle for the city.
                cityGraphics.DrawEllipse(Pens.Black, (float)point.x - 2, (float)point.y - 2, 5, 5);
            }
            for (int i = 0; i < x.points.Count; i++)
            {
                if (i == x.points.Count - 1)
                {
                    cityGraphics.DrawLine(Pens.Black, (float)points[x.points[i]].x, (float)points[x.points[i]].y, (float)points[x.points[0]].x, (float)points[x.points[0]].y);
                }
                else
                {
                    cityGraphics.DrawLine(Pens.Black, (float)points[x.points[i]].x, (float)points[x.points[i]].y, (float)points[x.points[i + 1]].x, (float)points[x.points[i + 1]].y);
                }
            }    

            form.pictureBox1.Image = cityImage;
        }
    }
}
