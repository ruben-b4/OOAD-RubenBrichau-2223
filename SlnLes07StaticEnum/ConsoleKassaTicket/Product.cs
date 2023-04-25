using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleKassaTicket
{
    internal class Product
    {
        public string Naam { get; set; }

        public decimal Price { get; set; }
        private string Code { get; set; } // _ staat omdat het een variable is vr u properties

        // ontvangt string geeft boolean terug
        public Product(string naam, decimal price, string code)
        {
            if (ValideerCode(code) == false)
            {
                throw new Exception("Gelieve een geldige productcode in te geven");
            }
            Naam = naam;
            Price = price;
            Code = code;
        }
        public static bool ValideerCode(string code)
        {
            if (code.Length != 6 || !code.StartsWith("P"))
            {
                return false;
            }
            return true;
        }

        public override string ToString()
        {
            return $"({Code}) {Naam} {Price}";
        }
    }
}
