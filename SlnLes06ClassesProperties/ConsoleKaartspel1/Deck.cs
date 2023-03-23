using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleKaartspel1
{
    internal class Deck
    {
        private List<Kaart> kaarten;

        public Deck()
        {
            kaarten = new List<Kaart>();
            for (int i = 1; i <= 13; i++)
            {
                kaarten.Add(new Kaart(i, "C"));
                kaarten.Add(new Kaart(i, "S"));
                kaarten.Add(new Kaart(i, "H"));
                kaarten.Add(new Kaart(i, "D"));
            }
        }

        public List<Kaart> Kaarten
        {
            get { return kaarten; }
        }

        public void Schudden()
        {
            Random rnd = new Random();
            for (int i = 0; i < kaarten.Count; i++)
            {
                // bron chat gpt
                int randomIndex = rnd.Next(0, kaarten.Count);
                Kaart tempKaart = kaarten[i];
                kaarten[i] = kaarten[randomIndex];
                kaarten[randomIndex] = tempKaart;
            }
        }

        public Kaart NeemKaart()
        {
            if (kaarten.Count > 0)
            {
                Kaart kaart = kaarten[0];
                kaarten.RemoveAt(0);     // bron https://www.tutorialspoint.com/How-to-remove-an-item-from-a-Chash-list-by-using-an-index#:~:text=To%20remove%20an%20item%20from%20a%20list%20in%20C%23%20using,use%20the%20RemoveAt()%20method.
                return kaart;
            }
            else
            {
                throw new ArgumentOutOfRangeException("Geen kaarten meer in de stapel!");
            }
        }
    }
}
