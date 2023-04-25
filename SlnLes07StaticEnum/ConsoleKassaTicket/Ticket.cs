using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleKassaTicket
{
    internal class Ticket
    {
        private List<Product> producten;
        private Betaalwijze betaaldMet;
        private string kassier;
        private decimal totaalprijs;

        public Ticket(List<Product> producten, string kassier, Betaalwijze betaaldMet = Betaalwijze.Cash)
        {
            if (string.IsNullOrEmpty(kassier))
            {
                throw new Exception("kassier moet ingegeven zijn");
            }
            this.producten = producten;
            this.betaaldMet = betaaldMet;
            this.kassier = kassier;

            this.totaalprijs = 0;
            foreach (Product product in producten)
            {
                this.totaalprijs += product.Price;
            }

            if (this.betaaldMet == Betaalwijze.Visa)
            {
                this.totaalprijs += 0.12m;
            }
        }

        public decimal TotaalPrijs
        {
            get { return this.TotaalPrijs; }
        }

        public void DrukTicket()
        {
            Console.WriteLine("KASSATICKET");
            Console.WriteLine("===========");
            Console.WriteLine("Uw kassier: " + this.kassier);
            Console.WriteLine("");
            foreach (Product product in producten)
            {
                Console.WriteLine(product.ToString());
            }
            Console.WriteLine("----------");
            Console.WriteLine("visa kosten: 0.12");
            Console.WriteLine("totaalprijs: " + this.totaalprijs.ToString());
            Console.ReadKey();
        }
    }
}
