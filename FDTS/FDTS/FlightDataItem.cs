using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FDMS
{
    public class FlightDataItem
    {
        public string TailID { get; set; }
        public float accelX { get; set; }

        public float accelY { get; set; }
        public float accelZ { get; set; }
        public float weight { get; set; }
        public float altitude { get; set; }
        public float pitch { get; set; }
        public float bank { get; set; }
        public string time { get; set; }

    }
}
