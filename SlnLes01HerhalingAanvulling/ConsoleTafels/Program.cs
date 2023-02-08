using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleTafels
{
    internal class Program
    {
        private static void Druktafel(int get = 4, int tel = 2)
        {
            int i;
            
            Console.WriteLine("4x8 tafel:");
            for (i = 1; i <= 8; i++)
            {
                Console.Write("{0} x {1} = {2} \n", get, i, i * get);
            }

            Console.WriteLine("\n2x5 tafel:");
            for (i = 1; i <= 5; i++)
            {
                Console.Write("{0} x {1} = {2} \n", tel, i, i * tel);
            }
        }

        private static void Deel2()
        {
            int get, len;

            Console.Write("\nGeef een getal: ");
            get = Convert.ToInt32(Console.ReadLine());                     // onthoudt getal (get)
            if (get <= 0)
            {
                Console.Write("Het getal moet positief zijn!");
                Console.Write(" Geef een getal: ");
                get = int.Parse(Console.ReadLine());
            }

            Console.Write("Geef een lengte: ");
            len = int.Parse(Console.ReadLine());

            Console.WriteLine("7x3 tafel:");
            for (int i = 1; i <= len; i++)                      
            {
                Console.WriteLine(get + " x " + i + " = " + get * i);     // schrijft getal x i = getal*i, for telt I++ op tot het lengte haalt
            }
            Console.ReadKey();
        }

        static void Main(string[] args)
        {
            Druktafel();
            Deel2();
        }
    }
}
