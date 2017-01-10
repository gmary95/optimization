using System;
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

        public City GetCity(int tourPosition)
        {
            return (City)points[tourPosition];
        }

        public double CalculateFunction(Tour x)
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

        public bool containsCity(City city)
        {
            return points.Contains(city);
        }

        public String toString()
        {
            String geneString = "|";
            for (int i = 0; i < points.Count; i++)
            {
                geneString += GetCity(i) + "|";
            }
            return geneString;
        }

        public void Draw(Tour x)
        {
            Form1 form = (Form1)Application.OpenForms[0];
            if (cityImage == null)
            {
                cityImage = new Bitmap(form.pictureBox1.Width, form.pictureBox1.Height);
                cityGraphics = Graphics.FromImage(cityImage);
            }

            cityGraphics.FillRectangle(Brushes.White, 0, 0, cityImage.Width, cityImage.Height);
            int k = 0;
            foreach (City point in points)
            {
                k++;
                cityGraphics.DrawString(k.ToString(), new Font("Tahoma", 8), Brushes.Black, (float)point.x, (float)point.y);
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
