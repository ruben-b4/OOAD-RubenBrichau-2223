using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace MyClassLibrary
{
    public enum Status
    {
        InBehandeling = 1,
        Goedgekeurd = 2,
        Verworpen = 3
    }

    public class Ontlening
    {
        public int Id { get; set; }
        public DateTime Vanaf { get; set; }
        public DateTime Tot { get; set; }
        public string Bericht { get; set; }
        public Status Status { get; set; }
        public int VoertuigId { get; set; }
        public int AanvragerId { get; set; }
        public Ontlening()
        {
        }

        public static List<Ontlening> GetAllOntleningen()
        {
            List<Ontlening> ontleningen = new List<Ontlening>();
            string connString = ConfigurationManager.ConnectionStrings["connStr"].ConnectionString;
            using (SqlConnection connection = new SqlConnection(connString))
            {
                connection.Open();

                string query = "SELECT o.*, Voertuig.naam AS voertuignaam " +
                    "FROM Ontlening o " +
                    "JOIN Gebruiker ON o.aanvrager_id = Gebruiker.id " +
                    "JOIN Voertuig ON o.voertuig_id = Voertuig.id ";

                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    Ontlening currentOntlening = null;

                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            int ontleningId = (int)reader["id"];
                            byte statusValue = (byte)reader["status"];

                            if (currentOntlening == null || currentOntlening.Id != ontleningId)
                            {
                                currentOntlening = new Ontlening
                                {
                                    Id = (int)reader["id"],
                                    Vanaf = (DateTime)reader["vanaf"],
                                    Tot = (DateTime)reader["tot"],
                                    Bericht = (string)reader["bericht"],
                                    Status = (Status)statusValue,
                                    VoertuigId = (int)reader["voertuig_id"],
                                    AanvragerId = (int)reader["aanvrager_id"]
                                };

                                ontleningen.Add(currentOntlening);
                            }
                        }
                        return ontleningen;
                    }
                }
            }
        }

        public static List<Ontlening> GetOntleningenByGebruikerId(int gebruikerId)
        {
            List<Ontlening> ontleningen = new List<Ontlening>();

            string connString = ConfigurationManager.ConnectionStrings["connStr"].ConnectionString;

            using (SqlConnection connection = new SqlConnection(connString))
            {
                connection.Open();

                string query = "SELECT o.*, v.naam AS voertuignaam " +
                               "FROM Ontlening o " +
                               "JOIN Voertuig v ON o.voertuig_id = v.id " +
                               "WHERE o.aanvrager_id = @gebruikerId";

                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@gebruikerId", gebruikerId);

                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            Ontlening ontlening = new Ontlening
                            {
                                Id = (int)reader["id"],
                                Vanaf = (DateTime)reader["vanaf"],
                                Tot = (DateTime)reader["tot"],
                                Bericht = (string)reader["bericht"],
                                Status = (Status)(byte)reader["status"],
                                VoertuigId = (int)reader["voertuig_id"],
                                AanvragerId = (int)reader["aanvrager_id"]
                            };

                            ontleningen.Add(ontlening);
                        }
                    }
                }
            }

            return ontleningen;
        }

        public void DeleteFromDB()
        {
            string connString = ConfigurationManager.ConnectionStrings["connStr"].ConnectionString;

            using (SqlConnection conn = new SqlConnection(connString))
            {
                conn.Open();
                SqlCommand comm = new SqlCommand("DELETE FROM Ontlening WHERE id = @ontleningId", conn);
                comm.Parameters.AddWithValue("@ontleningId", Id);
                comm.ExecuteNonQuery();
            }
        }

        public void SaveToDB()
        {
            string connString = ConfigurationManager.ConnectionStrings["connStr"].ConnectionString;

            using (SqlConnection conn = new SqlConnection(connString))
            {
                conn.Open();

                SqlCommand comm = new SqlCommand(@"INSERT INTO [Ontlenin]g (vanaf, tot, bericht, status, voertuig_id, aanvrager_id) VALUES (@vanaf, @tot, @bericht, @status, @voertuigId, @aanvragerId) SELECT SCOPE_IDENTITY()", conn);

                comm.Parameters.AddWithValue("@vanaf", Vanaf);
                comm.Parameters.AddWithValue("@tot", Tot);
                comm.Parameters.AddWithValue("@bericht", Bericht);
                comm.Parameters.AddWithValue("@status", (byte)Status);
                comm.Parameters.AddWithValue("@voertuigId", VoertuigId);
                comm.Parameters.AddWithValue("@aanvragerId", AanvragerId);

                Id = Convert.ToInt32(comm.ExecuteScalar());

            }
        }
    }
}


