using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleVeiling
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Koper koper1 = new Koper("Ruben");
            Koper koper2 = new Koper("Wannes");
            Koper koper3 = new Koper("Johan");

            Item item1 = new Item("Stripboek", 10);
            item1.PlaatsBod(koper1, 15);
            item1.PlaatsBod(koper2, 20);
            item1.SluitKoop();
            Console.WriteLine($"Winnaar {item1}: {item1.GetWinnaar()?.Naam}");

            Item item2 = new Item("Fiets", 100);
            item2.PlaatsBod(koper1, 120);
            item2.PlaatsBod(koper3, 150);
            item2.SluitKoop();
            Console.WriteLine($"Winnaar {item2}: {item2.GetWinnaar()?.Naam}");

            ToonItemsInBezit(koper1);
            ToonItemsInBezit(koper2);
            ToonItemsInBezit(koper3);

            Console.ReadKey();
        }

        static void ToonItemsInBezit(Koper koper)
        {
            Console.WriteLine($"{koper.Naam} heeft de volgende items:");
            foreach (Item item in koper.GekochteItems)
            {
                Console.WriteLine($"- {item.Naam}");
            }
        }
    }
}
