using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace optimization
{
    class NeighboardCity
    {
        public int city { set; get; }
        public bool isRepeat { set; get; }

        public NeighboardCity(int city, bool isRepeat)
        {
            this.city = city;
            this.isRepeat = isRepeat;
        }
    }
}
