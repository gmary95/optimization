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

        public void DeleteCity(int city)
        {
            for (int i = 0; i < neignboardCity.Count; i++)
            {
                if (neignboardCity[i].city == city)
                {
                    neignboardCity.RemoveAt(i);
                }
            }
        }

        public int FindNextCity()
        {
            int nextCity = neignboardCity[0].city;
            for (int i = 0; i < neignboardCity.Count; i++)
            {
                if (neignboardCity[i].isRepeat == true)
                {
                    nextCity = neignboardCity[i].city;
                    break;
                }
            }
            return nextCity;
        }
    }
}
