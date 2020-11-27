using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WpfApp_Alytalo
{
    class Lights
    {
        public bool On { get; set; }
        public double Lumen { get; set; }
        public void LightsOn()
        {
            On = true;
        }
        public void LightsOff()
        {
            On = false;
        }
    }
}
