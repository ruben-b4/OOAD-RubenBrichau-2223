using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using MyClassLibrary;

namespace WpfGebruiker
{
    /// <summary>
    /// Interaction logic for Home.xaml
    /// </summary>
    public partial class Home : Page
    {
        public Home()
        {
            InitializeComponent();
        }

        private void LoadVehicles()
        {
            string connString = ConfigurationManager.ConnectionStrings["connStr"].ConnectionString;

            // Connect to your SQL database and retrieve the image data for each vehicle
            using (SqlConnection connection = new SqlConnection(connString))
            {
                connection.Open();

                string query = "SELECT id, data, voertuig_id FROM Foto";

                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            int id = reader.GetInt32(0);
                            byte[] imageData = (byte[])reader["Data"];
                            int voertuigId = reader.GetInt32(2);

                            // Create a Foto instance and add it to your list
                            Foto foto = new Foto
                            {
                                Id = id,
                                Data = imageData,
                                VoertuigId = voertuigId
                            };

                            // Add the Foto instance to your list or collection of vehicles
                            // For example: allVehicles.Add(foto);
                            foto.Data = imageData;
                            pnlItems.Children.Add(foto);

                        }
                    }
                }
            }
        }
    }
}
