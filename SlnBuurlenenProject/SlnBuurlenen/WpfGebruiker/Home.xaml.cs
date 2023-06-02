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

            voertuigen = Voertuig.GetAllVoertuigen();

            voertuigen = voertuigen.Where(v => v.EigenaarId != currentUser.Id).ToList();

            UpdateVehicleDisplay();
        }

        public void DisplayEigenaarNaam(int eigenaarId)
        {
            currentUser.GetEigenaarNaam(eigenaarId);
        }

        private void UpdateVehicleDisplay()
        {
            pnlItems.Children.Clear();

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

            // display filtered vehicles
            foreach (Voertuig voertuig in filteredVoertuigen)
            {
                StackPanel pnl = new StackPanel();
                pnl.Margin = new Thickness(10);

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
                btnInfo.Content = $"Info";
                btnInfo.Click += (sender, e) =>
                {
                    selectedVoertuig = voertuig;
                    VoertuigInfo(sender, e);
                };

                pnl.Children.Add(btnInfo);
                pnlItems.Children.Add(pnl);
            }
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
                    detailsPage.lblEigenaar.Content = $"Eigenaar: {currentUser.GetEigenaarNaam(selectedVoertuig.EigenaarId)}";
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
                    detailsPage.lblEigenaar.Content = $"Eigenaar: {currentUser.GetEigenaarNaam(selectedVoertuig.EigenaarId)}";
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

