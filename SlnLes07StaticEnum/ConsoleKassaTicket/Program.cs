using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleKassaTicket
{
    public enum Betaalwijze 
    {   
        Visa, 
        Cash, 
        Bancontact 
    }
    internal class Program
    {
        static void Main(string[] args)
        {
            List<Product> products = new List<Product>();
            products.Add(new Product("Bananen", 1.75m, "P02384"));
            products.Add(new Product("brood", 2.10m, "P01820"));
            products.Add(new Product("Kaas", 3.99m, "P45612"));
            products.Add(new Product("Koffie", 4.10m, "P98754"));

            Ticket ticket = new Ticket(products, "Annie", Betaalwijze.Visa);

            ticket.DrukTicket();
        }
    }
}
