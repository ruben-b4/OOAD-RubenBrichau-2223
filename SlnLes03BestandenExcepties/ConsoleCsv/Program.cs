using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleCsv
{
    internal class Program
    {
        static void Main(string[] args)
        {
            // lijst spelers
            string[] spelers = { "Zakaria", "Saleha", "Indra", "Ralph", "Francisco", "Marie" };

            // lijst spellen
            string[] spellen = { "schaken", "dammen", "backgammon" };

            // bureaublad locatie
            string desktopPath = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
            string filename = Path.Combine(desktopPath, "wedstrijden.csv"); // naam bestand

            // genereer 100 wedstrijden
            Random random = new Random();
            for (int i = 0; i < 100; i++)
            {
                // kies spel
                string speler1 = spelers[random.Next(spelers.Length)];
                string speler2 = spelers[random.Next(spelers.Length)];

                // kies 2 spelers
                string spel = spellen[random.Next(0, spellen.Length)];

                // genereer willekeurige resultaten voor de 3 partijen
                int[] results = { random.Next(3) };
                string resultaat = $"{results}";

                // schrijf de record-rij naar het CSV-bestand
                using (StreamWriter writer = new StreamWriter(filename, true))
                {
                    writer.WriteLine($"{i};{speler1};{speler2};{spel};{resultaat};");
                }
            }
        }
    }
}
