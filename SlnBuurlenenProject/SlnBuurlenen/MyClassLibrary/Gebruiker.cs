using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Windows.Documents;

namespace MyClassLibrary
{
    public enum Gender
    {
        Male,
        Female
    }
    public class Gebruiker
    {
        private static string connString = ConfigurationManager.ConnectionStrings["connStr"].ConnectionString;

        public int Id { get; set; }
        public string VoorNaam { get; set; }
        public string AchterNaam { get; set; }
        public string Email { get; set; }
        public string Passwoord { get; set; }
        public DateTime Aanmaakdatum { get; set; }
        public byte[] Profielfoto { get; set; }
        public bool UserInDB(string email, string paswoord, out int userId)
        {
            using (SqlConnection conn = new SqlConnection(connString))
            {
                conn.Open();

                SqlCommand comm = new SqlCommand("SELECT * FROM [Gebruiker] WHERE email = @email AND paswoord = @password", conn);
                comm.Parameters.AddWithValue("@email", email);
                comm.Parameters.AddWithValue("@password", paswoord);
                SqlDataReader reader = comm.ExecuteReader();

                if (reader.Read())
                {
                    Id = (int)reader["id"];
                    userId = Id;
                    return true;
                }
            }

            userId = -1;
            return false;
        }

        public string GetEigenaarNaam(int eigenaarId)
        {
            string voornaam;
            string achternaam;
            string fullnaam = string.Empty;

            string connString = ConfigurationManager.ConnectionStrings["connStr"].ConnectionString;

            using (SqlConnection connection = new SqlConnection(connString))
            {
                connection.Open();

                string query = "SELECT voornaam, achternaam FROM Gebruiker WHERE id = @eigenaarId";

                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@eigenaarId", eigenaarId);

                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            voornaam = reader.GetString(0);
                            achternaam = reader.GetString(1);
                            fullnaam = voornaam + " " + achternaam;
                        }
                    }
                }
            }
            return fullnaam;
        }

        public Gebruiker()
        {
        }

        public Gebruiker(int id, string voornaam, string achternaam)
        {
            Id = id;
            VoorNaam = voornaam;
            AchterNaam = achternaam;
        }
    }
}
