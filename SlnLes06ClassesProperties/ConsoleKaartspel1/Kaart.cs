using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleKaartspel1
{
    internal class Kaart
    {
        private int nummer;
        private string kleur;

        public Kaart(int nummer, string kleur)
        {
            // extra check
            if (nummer < 1 || nummer > 13)
            {
                throw new ArgumentOutOfRangeException("Nummer moet tussen 1 en 13 liggen");
            }

            // properties waarde
            this.nummer = nummer;
            this.kleur = kleur;
        }

        public int Nummer
        {
            get { return nummer; } // implementatie getter
        }

        public string Kleur
        {
            get { return kleur; }
        }
    }
}
