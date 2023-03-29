using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleVeiling
{
    internal class Koper
    {
        public string Naam { get; set; }
        public List<Item> GekochteItems { get; set; }

        public Koper(string naam)
        {
            Naam = naam;
            GekochteItems = new List<Item>();
        }

        public void VoegItemToe(Item item)
        {
            GekochteItems.Add(item);
        }
    }
}
