using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GZSProjesi
{
    class Temp
    {
        public int id { get; set; }
        public String date { get; set; }
        public String time { get; set; }
        public String celcius { get; set; }
        public Temp(int id,String date,String time,String celcius)
        {
            this.id = id;
            this.date = date;
            this.time = time;
            this.celcius = celcius;
        }
    }
}
