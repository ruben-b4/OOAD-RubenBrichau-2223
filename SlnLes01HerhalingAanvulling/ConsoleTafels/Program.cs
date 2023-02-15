using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleTafels
{
    internal class Program
    {
        // private static void Druktafel(int get = 4, int tel = 2)
        // {
        //    int i;

        // Console.WriteLine("4x8 tafel:");
        //    for (i = 1; i <= 8; i++)
        //    {
        //        Console.Write("{0} x {1} = {2} \n", get, i, i * get);
        //    }

        // Console.WriteLine("\n2x5 tafel:");
        //    for (i = 1; i <= 5; i++)
        //    {
        //        Console.Write("{0} x {1} = {2} \n", tel, i, i * tel);
        //    }
        // }

        // private static int VraagPositiefGetal(string waarvoor)
        // {
        //    int get, len;

        // Console.Write("\nGeef een getal: ");
        //    get = Convert.ToInt32(Console.ReadLine());                     // onthoudt getal (get)
        //    if (get <= 0)
        //    {
        //        Console.Write("Het getal moet positief zijn!");
        //        Console.Write(" Geef een getal: ");
        //        get = int.Parse(Console.ReadLine());
        //    }

        // Console.Write("Geef een lengte: ");
        //    len = int.Parse(Console.ReadLine());

        // Console.WriteLine("7x3 tafel:");
        //    for (int i = 1; i <= len; i++)
        //    {
        //        Console.WriteLine(get + " x " + i + " = " + get * i);     // schrijft getal x i = getal*i, for telt I++ op tot het lengte haalt
        //    }
        //    Console.ReadKey();
        // }

        // static void Main(string[] args)
        // {
        //    Druktafel();
        //    VraagPositiefGetal();
        // }
        static void Main(string[] args)
        {
            Console.WriteLine(Druktafel(4, 8));
            Console.WriteLine(Druktafel(2, 5));

            int getal = VraagpositiefGetal("Geef een getal: ");
            int lengte = VraagpositiefGetal("Geef een lengte: ");

            Console.WriteLine(Druktafel(getal, lengte));
            Console.ReadLine();
        }

        static string Druktafel(int getal1, int getal2)
        {
            // tonen van maaltafel
            string maal = $"{getal1}x{getal2} tafel:{Environment.NewLine}";

            // berekening van maaltafel
            for (int teller = 1; teller <= getal2; teller++)
            {
                maal += $"{getal1} x {teller} = {getal1 * teller}{Environment.NewLine}";
            }
            return maal;
        }

        static int VraagpositiefGetal(string vraag)
        {
            Console.Write(vraag);
            int getal = Convert.ToInt32(Console.ReadLine());
            while (getal < 1)
            {
                Console.Write("Het getal moet positief zijn! ");
                Console.Write(vraag);
                getal = Convert.ToInt32(Console.ReadLine());
            }
            return getal;
        }
    }
}
