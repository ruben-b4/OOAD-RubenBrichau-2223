using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Imaging;
using MyClassLibrary;


namespace WpfGebruiker
{
    public partial class Home : Page
    {
        private List<Voertuig> voertuigen;
        private Voertuig selectedVoertuig;

        public Home()
        {
            InitializeComponent();

            string connString = ConfigurationManager.ConnectionStrings["connStr"].ConnectionString;

            // Connect to your SQL database and retrieve the image data for each vehicle
            using (SqlConnection connection = new SqlConnection(connString))
            {
                connection.Open();

                string query = "SELECT v.naam, v.merk, v.model, f.data, v.type " +
                               "FROM Voertuig v " +
                               "LEFT JOIN Foto f ON v.id = f.voertuig_id";

                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        voertuigen = new List<Voertuig>();

                        while (reader.Read())
                        {
                            string naam = reader.GetString(0);
                            string merk = reader.IsDBNull(1) ? string.Empty : reader.GetString(1);
                            string model = reader.IsDBNull(2) ? string.Empty : reader.GetString(2);
                            byte[] imageData = reader.IsDBNull(3) ? null : (byte[])reader["data"];
                            int type = reader.IsDBNull(4) ? 0 : reader.GetInt32(4);

                            // Create a Voertuig object
                            Voertuig voertuig = new Voertuig
                            {
                                Naam = naam,
                                Merk = merk,
                                Model = model,
                                ImageData = imageData,
                                Type = type
                            };

                            voertuigen.Add(voertuig);
                        }
                    }
                }
            }

            UpdateVehicleDisplay();
        }

        private void UpdateVehicleDisplay()
        {
            pnlItems.Children.Clear();

            // Filter vehicles based on the selected checkboxes
            int selectedType = 0;

            if (ChkAuto.IsChecked == true)
            {
                selectedType = 1;
            }
            else if (ChkMotor.IsChecked == true)
            {
                selectedType = 2;
            }

            List<Voertuig> filteredVoertuigen = voertuigen;

            if (selectedType != 0)
            {
                filteredVoertuigen = voertuigen.Where(v => v.Type == selectedType).ToList();
            }

            // Display the filtered vehicles
            foreach (var voertuig in filteredVoertuigen)
            {
                // Create stackpanel to group labels
                StackPanel pnl = new StackPanel();
                pnl.Margin = new Thickness(10);

                // Create labels for name, brand, and model
                Image img = new Image();
                img.Width = 200;
                img.Source = Foto.GetBitmapImageFromByteArray(voertuig.ImageData); // chatchpt voor images dynamisch toevoegen
                pnl.Children.Add(img);

                Label lblNaam = new Label();
                lblNaam.Content = $"Naam: {voertuig.Naam}";
                pnl.Children.Add(lblNaam);

                Label lblMerk = new Label();
                lblMerk.Content = $"Merk: {voertuig.Merk}";
                pnl.Children.Add(lblMerk);

                Label lblModel = new Label();
                lblModel.Content = $"Model: {voertuig.Model}";
                pnl.Children.Add(lblModel);

                Button btnInfo = new Button();
                btnInfo.Content = $"Type: {voertuig.Type}";
                btnInfo.Click += (sender, e) =>
                {
                    selectedVoertuig = voertuig;
                    VoertuigInfo(sender, e);
                }; 

                pnl.Children.Add(btnInfo);


                // Add stackpanel to wrappanel
                pnlItems.Children.Add(pnl);
            }
        }

        private void VoertuigInfo(object sender, RoutedEventArgs e)
        {
            if (selectedVoertuig != null)
            {
                VoertuigDetails detailsPage = new VoertuigDetails();

                detailsPage.imgVehicle.Source = Foto.GetBitmapImageFromByteArray(selectedVoertuig.ImageData);
                detailsPage.lblName.Content = $"Naam: {selectedVoertuig.Naam}";
                detailsPage.lblMerk.Content = $"Merk: {selectedVoertuig.Merk}";
                detailsPage.lblModel.Content = $"Model: {selectedVoertuig.Model}";
                detailsPage.lblType.Content = $"Type: {selectedVoertuig.Type}";
                detailsPage.lblBouwjaar.Content = $"Bouwjaar: {selectedVoertuig.Bouwjaar}";
                detailsPage.lblEigenaar.Content = $"Eigenaar: {selectedVoertuig.EigenaarId}";
                detailsPage.lblTransmisse.Content = $"Transmissie: {selectedVoertuig.Transmissie}";
                detailsPage.lblBeschrijving.Content = $"{selectedVoertuig.Beschrijving}";
                NavigationService.Navigate(detailsPage);
            }
        }

        private void ChkAuto_Checked(object sender, RoutedEventArgs e)
        {
            UpdateVehicleDisplay();
        }

        private void ChkAuto_Unchecked(object sender, RoutedEventArgs e)
        {
            UpdateVehicleDisplay();
        }
    }
}

