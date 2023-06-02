using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
 using System.Windows.Navigation;
using System.Windows;
using System.Data;

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
        public Foto VoertuigFoto { get; set; }
        public Voertuig()
        {
        }
        public bool GetCar(string naam, string beschrijving, string model, int id)
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

        public static List<Voertuig> GetAllVoertuigen()
        {
            List<Voertuig> voertuigen = new List<Voertuig>();

            string connString = ConfigurationManager.ConnectionStrings["connStr"].ConnectionString;

            using (SqlConnection connection = new SqlConnection(connString))
            {
                connection.Open();

                string query = "SELECT v.*, f.id AS foto_id, f.data AS foto_data FROM [Voertuig] v LEFT JOIN [Foto] f ON v.id = f.voertuig_id";

                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    Voertuig currentVoertuig = null;

                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            int voertuigId = (int)reader["id"];

                            if (currentVoertuig == null || currentVoertuig.Id != voertuigId)
                            {

                                currentVoertuig = new Voertuig
                                {
                                    Id = (int)reader["id"],
                                    Naam = (string)reader["naam"],
                                    Beschrijving = (string)reader["beschrijving"],
                                    Bouwjaar = reader.IsDBNull(reader.GetOrdinal("bouwjaar")) ? null : (int?)reader["bouwjaar"],
                                    Merk = reader.IsDBNull(reader.GetOrdinal("merk")) ? null : (string)reader["merk"],
                                    Model = reader.IsDBNull(reader.GetOrdinal("model")) ? null : (string)reader["model"],
                                    Type = reader.IsDBNull(reader.GetOrdinal("type")) ? null : (int?)reader["type"],
                                    Gewicht = reader.IsDBNull(reader.GetOrdinal("gewicht")) ? null : (int?)reader["gewicht"],
                                    MaxBelasting = reader.IsDBNull(reader.GetOrdinal("maxBelasting")) ? null : (int?)reader["maxBelasting"],
                                    Afmetingen = reader.IsDBNull(reader.GetOrdinal("afmetingen")) ? string.Empty : (string)reader["afmetingen"],
                                    EigenaarId = reader.IsDBNull(reader.GetOrdinal("eigenaar_id")) ? 0 : (int)reader["eigenaar_id"],
                                };
                                // Only retrieve the first photo for the current vehicle chatgpt

                                using (SqlConnection fotoConn = new SqlConnection(connString))
                                {
                                    fotoConn.Open();

                                    SqlCommand fotoComm = new SqlCommand("SELECT TOP 1 * FROM [Foto] WHERE voertuig_id = @voertuigId", fotoConn);
                                    fotoComm.Parameters.AddWithValue("@voertuigId", voertuigId);
                                    SqlDataReader fotoReader = fotoComm.ExecuteReader();

                                    if (fotoReader.Read())
                                    {
                                        Foto foto = new Foto
                                        {
                                            Id = (int)fotoReader["id"],
                                            Data = (byte[])fotoReader["data"],
                                            VoertuigId = (int)fotoReader["voertuig_id"]
                                        };
                                        currentVoertuig.ImageData = foto.Data;
                                    }
                                }
                                voertuigen.Add(currentVoertuig);
                            }
                        }
                    }
                }
            }

            return voertuigen;
        }

        public int InsertToDB()
        {
            string connString = ConfigurationManager.ConnectionStrings["connStr"].ConnectionString;

            using (SqlConnection conn = new SqlConnection(connString))
            {
                conn.Open();

                SqlCommand comm = new SqlCommand(@"INSERT INTO [Voertuig] (naam, beschrijving, bouwjaar, merk, model, type, transmissie, brandstof, eigenaar_id) output INSERTED.ID VALUES(@par1,@par2,@par3,@par4,@par5,@par6,@par7,@par8,@par9)", conn);

                comm.Parameters.AddWithValue("@par1", Naam);
                comm.Parameters.AddWithValue("@par2", Beschrijving);
                if (Bouwjaar == null)
                {
                    comm.Parameters.AddWithValue("@par3", DBNull.Value);
                }
                else
                {
                    comm.Parameters.AddWithValue("@par3", Bouwjaar);
                }
                comm.Parameters.AddWithValue("@par4", Merk);
                comm.Parameters.AddWithValue("@par5", Model);
                if (Type == null)
                {
                    comm.Parameters.AddWithValue("@par6", DBNull.Value);
                }
                else
                {
                    comm.Parameters.AddWithValue("@par6", Type);
                }
                comm.Parameters.AddWithValue("@par7", Transmissie);
                comm.Parameters.AddWithValue("@par8", Brandstof);
                comm.Parameters.AddWithValue("@par9", EigenaarId);

                return (int)comm.ExecuteScalar();
            }
        }
    }
}
