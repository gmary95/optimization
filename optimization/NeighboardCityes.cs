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
        static Random random = new Random();

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

        private static bool isContains(Tour arr, int val)
        {
            for (int i = 0; i < arr.points.Count; i++)
            {
                if (arr.points[i] == val)
                {
                    return true;
                }
            }
            return false;
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

        public int RandomNodeNotInChild(Tour child)
        {
            int len = child.points.Count;
            int randomNode;
            do
            {
                randomNode = random.Next(len);
            } while (isContains(child, randomNode));
            return randomNode;
        }


        public List<int> FindNextCity()
        {
            List<int> candidates = new List<int>();
            for (int i = 0; i < neignboardCity.Count; i++)
            {
                if (neignboardCity[i].isRepeat == true)
                {
                    candidates.Add(neignboardCity[i].city);
                }
            }
            if (candidates.Count == 0)
            {
                candidates = neignboardCity.Select(x => x.city).ToList();
            }

            return candidates;
        }

        public int FindCity(int city)
        {
            int indexCity = 0;
            for (int i = 0; i < neignboardCity.Count; i++)
            {
                if (neignboardCity[i].city == city)
                {
                    indexCity = i;
                    break;
                }
            }
            return indexCity;
        }
    }
}
