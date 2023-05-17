using System;
using System.Configuration;
using System.Data.SqlClient;

namespace MyClassLibrary
{
    public class Gebruiker
    {
        string connString = ConfigurationManager.ConnectionStrings["connStr"].ConnectionString;

        public int Id { get; set; }
        public string voorNaam { get; set; }
        public string achterNaam { get; set; }
        public string Email { get; set; }
        public string Passwoord { get; set; }
        public DateTime aanmaakdatum { get; set; }
        public enum Gender
        {
            Male, 
            Female
        }
        public bool UserInDB(string email, string paswoord)
        {
            using (sqlConne)
        }
           
    }
}
