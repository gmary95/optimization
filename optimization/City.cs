﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace optimization
{
    class City
    {
        Random random = new Random();

        public double x { get; }
        public double y { get; }

        public City(double x, double y)
        {
            this.x = x;
            this.y = y;
        }

        public City()
        {
            this.x = (random.NextDouble() * 200);
            this.y = (random.NextDouble() * 200);
        }

        public double distanceTo(City city)
        {
            double xDistance = Math.Abs(x - city.x);
            double yDistance = Math.Abs(y - city.y);
            double distance = Math.Sqrt((xDistance * xDistance) + (yDistance * yDistance));

            return distance;
        }

        public String toString()
        {
            return x.ToString() + ", " + y.ToString();
        }

    }
}