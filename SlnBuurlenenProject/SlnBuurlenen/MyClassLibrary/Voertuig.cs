using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Navigation;
using System.Windows;

namespace MyClassLibrary
{
    public enum Transmissie
    {
        Manueel,
        Automatisch
    }

    public enum Brandstof
    {
        Benzine,
        Diesel,
        LPG
    }
    public class Voertuig
    {
        public int Id { get; set; }
        public string Naam { get; set; }
        public string Beschrijving { get; set; }
        public int? Bouwjaar { get; set; }
        public string Merk { get; set; }
        public string Model { get; set; }
        public int? Type { get; set; }
        public Transmissie Transmissie { get; set; }
        public Brandstof Brandstof { get; set; }
        public int? Gewicht { get; set; }
        public int? MaxBelasting { get; set; }
        public string Afmetingen { get; set; }
        public bool Geremd { get; set; }
        public int EigenaarId { get; set; }
        public byte[] ImageData { get; set; }
        public Voertuig()
        {
        }
        public bool GetCar(string naam, string beschrijving, string model, int id )
        {
            string connString = ConfigurationManager.ConnectionStrings["connStr"].ConnectionString;
            using (SqlConnection conn = new SqlConnection(connString))
            {
                conn.Open();

                SqlCommand comm = new SqlCommand("SELECT * from [Voertuig] WHERE naam = @naam AND beschrijving = @beschrijving AND model = @model", conn);
                comm.Parameters.AddWithValue("@naam", naam);
                comm.Parameters.AddWithValue("@beschrijving", beschrijving);
                comm.Parameters.AddWithValue("@model", model);
                SqlDataReader reader = comm.ExecuteReader();

                if (!reader.Read()) return false;
                Voertuig voertuig = new Voertuig();
                voertuig.Naam = (string)reader["naam"];
                voertuig.Beschrijving = (string)reader["beschrijving"];
                voertuig.Model = (string)reader["model"];
                return true;
            }
        }

    }
}
