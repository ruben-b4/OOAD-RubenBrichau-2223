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
        private Gebruiker currentUser;

        public Home(Gebruiker user)
        {
            InitializeComponent();
            currentUser = user;

            string connString = ConfigurationManager.ConnectionStrings["connStr"].ConnectionString;

            // Connect to your SQL database and retrieve the image data for each vehicle
            using (SqlConnection connection = new SqlConnection(connString))
            {
                connection.Open();

                string query = "SELECT v.naam, v.merk, v.model, f.data, v.type, v.bouwjaar, v.beschrijving, v.eigenaar_id, v.gewicht, v.MaxBelasting, v.Geremd, v.Afmetingen " +
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
                            int? bouwjaar = reader.IsDBNull(5) ? null : (int?)reader.GetInt32(5);  // Retrieve Bouwjaar column
                            string beschrijving = reader.GetString(6);
                            int eigenaarId = reader.GetInt32(7);
                            int gewicht = reader.IsDBNull(8) ? 0 : reader.GetInt32(8);
                            int maxBel = reader.IsDBNull(9) ? 0 : reader.GetInt32(9);
                            string afmeting = reader.IsDBNull(11) ? string.Empty : reader.GetString(11);

                            // Create a Voertuig object
                            Voertuig voertuig = new Voertuig
                            {
                                Naam = naam,
                                Merk = merk,
                                Model = model,
                                ImageData = imageData,
                                Type = type,
                                Bouwjaar = bouwjaar,
                                Beschrijving = beschrijving,
                                EigenaarId = eigenaarId,
                                Gewicht = gewicht,
                                MaxBelasting = maxBel,
                                Afmetingen = afmeting
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
            foreach (Voertuig voertuig in filteredVoertuigen)
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
                lblNaam.Content = $"{voertuig.Naam}";
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

        private string GetEigenaarNaam(int eigenaarId)
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
        private void VoertuigInfo(object sender, RoutedEventArgs e)
        {
            if (selectedVoertuig != null)
            {
                if (selectedVoertuig.Type == 2)
                {
                    VoertuigDetailsGetrokken detailsPage = new VoertuigDetailsGetrokken();

                    detailsPage.imgVehicle.Source = Foto.GetBitmapImageFromByteArray(selectedVoertuig.ImageData);
                    detailsPage.lblName.Content = $"{selectedVoertuig.Naam}";
                    detailsPage.lblMerk.Content = $"Merk: {selectedVoertuig.Merk}";
                    detailsPage.lblModel.Content = $"Model: {selectedVoertuig.Model}";
                    detailsPage.lblGewicht.Content = $"Gewicht: {selectedVoertuig.Gewicht} kg";
                    detailsPage.lblMaxBel.Content = $"Max Belasting: {selectedVoertuig.MaxBelasting} kg";
                    detailsPage.lblGeremd.Content = $"Geremd: {selectedVoertuig.Geremd}";
                    detailsPage.lblBouwjaar.Content = $"Bouwjaar: {selectedVoertuig.Bouwjaar}";
                    detailsPage.lblEigenaar.Content = $"Eigenaar: {GetEigenaarNaam(selectedVoertuig.EigenaarId)}";
                    detailsPage.lblAfmetingen.Content = $"Afmetingen: {selectedVoertuig.Afmetingen} cm";
                    detailsPage.lblBeschrijving.Content = $"Beschrijving: {selectedVoertuig.Beschrijving}";
                    NavigationService.Navigate(detailsPage);
                }
                else
                {
                    VoertuigDetails detailsPage = new VoertuigDetails();

                    detailsPage.imgVehicle.Source = Foto.GetBitmapImageFromByteArray(selectedVoertuig.ImageData);
                    detailsPage.lblName.Content = $"{selectedVoertuig.Naam}";
                    detailsPage.lblMerk.Content = $"Merk: {selectedVoertuig.Merk}";
                    detailsPage.lblModel.Content = $"Model: {selectedVoertuig.Model}";
                    detailsPage.lblBrandstof.Content = $"Type: {selectedVoertuig.Brandstof}";
                    detailsPage.lblBouwjaar.Content = $"Bouwjaar: {selectedVoertuig.Bouwjaar}";
                    detailsPage.lblEigenaar.Content = $"Eigenaar: {GetEigenaarNaam(selectedVoertuig.EigenaarId)}";
                    detailsPage.lblTransmissie.Content = $"Transmissie: {selectedVoertuig.Transmissie}";
                    detailsPage.lblBeschrijving.Content = $"Beschrijving: {selectedVoertuig.Beschrijving}";
                    NavigationService.Navigate(detailsPage);
                }
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

