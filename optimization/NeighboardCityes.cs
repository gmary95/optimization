using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace optimization
{
    class NeighboardCityes
    {
        public List<NeighboardCity> neignboardCity = new List<NeighboardCity>();

        public bool isContains(int city)
        {
            bool isContains = false;
            for (int i = 0; i < neignboardCity.Count; i++)
            {
                if (neignboardCity[i].city == city)
                {
                    isContains = true;
                }
            }
            return isContains;
        }
    }
}
