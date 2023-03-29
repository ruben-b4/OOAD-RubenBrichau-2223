using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleVeiling
{
    internal class Bod
    {
        public Koper Koper { get; set; }
        public double Bedrag { get; set; }

        public Bod(Koper koper, double bedrag)
        {
            Koper = koper;
            Bedrag = bedrag;
        }
    }
}
