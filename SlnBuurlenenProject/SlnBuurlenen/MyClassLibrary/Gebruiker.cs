using System;
using System.Configuration;
using System.Data.SqlClient;

namespace MyClassLibrary
{
    public enum Gender
    {
        Male,
        Female
    }
    public class Gebruiker
    {
        public int Id { get; set; }
        public string VoorNaam { get; set; }
        public string AchterNaam { get; set; }
        public string Email { get; set; }
        public string Passwoord { get; set; }
        public DateTime Aanmaakdatum { get; set; }
        public byte[] Profielfoto { get; set; }
        public bool UserInDB(string email, string paswoord)
        {
            string connString = ConfigurationManager.ConnectionStrings["connStr"].ConnectionString;
            using (SqlConnection conn = new SqlConnection(connString))
            {
                conn.Open();

                SqlCommand comm = new SqlCommand("SELECT * from [Gebruiker] WHERE email = @email AND paswoord = @password", conn);
                comm.Parameters.AddWithValue("@email", email);
                comm.Parameters.AddWithValue("@password", paswoord);
                SqlDataReader reader = comm.ExecuteReader();

                if (!reader.Read()) return false;
                Gebruiker gebruiker = new Gebruiker();
                gebruiker.VoorNaam = (string)reader["voornaam"];
                gebruiker.AchterNaam = (string)reader["achternaam"];
                return true;
            }
        }

        public Gebruiker()
        {
        }
    }
}
