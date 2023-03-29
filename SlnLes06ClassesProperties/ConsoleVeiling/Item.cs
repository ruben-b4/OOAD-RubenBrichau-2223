using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleVeiling
{
    internal class Item
    {
        public string Naam { get; set; }
        public double Minimumprijs { get; set; }
        public bool Gesloten { get; set; }

        private Bod Winnaar { get; set; }

        public Item(string naam, double minimumprijs)
        {
            Naam = naam;
            Minimumprijs = minimumprijs;
            Gesloten = false;
        }

        public void PlaatsBod(Koper koper, double bedrag)
        {
            try
            {
                if (Gesloten) throw new Exception("De koop is gesloten.");
                if (bedrag < Minimumprijs) throw new Exception("Het bod is te laag.");

                if (Winnaar == null || bedrag > Winnaar.Bedrag)
                {
                    Winnaar = new Bod(koper, bedrag);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        public void SluitKoop()
        {
            if (Winnaar != null)
            {
                Gesloten = true;
                Winnaar.Koper.VoegItemToe(this);
            }
        }

        public Koper GetWinnaar()
        {
            return Winnaar?.Koper;
        }
    }
}

