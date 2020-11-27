using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WpfApp_Alytalo
{
    class Sauna
    {
        public Boolean Switched { get; set; }
        public double saunaTemp { get; set; }
        public double saunaCurrentValue { get; set; }
        public double saunaInitialValue { get; set; }
        public double saunaRecedingValue { get; set; }

        public void Lammita()
        {
            saunaInitialValue++;
        }
        public void Jaahdyta()
        {
            saunaCurrentValue--;
        }
        public void KiuasPaalle()
        {
            Switched = true;
        }
        public void KiuasPois()
        {
            Switched = false;
        }
    }
}
