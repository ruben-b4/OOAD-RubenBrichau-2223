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
    /// Interaction logic for Voertuigen.xaml
    /// </summary>
    public partial class Voertuigen : Page
    {
        private List<Voertuig> voertuigen;
        private Voertuig selectedVoertuig;
        private Gebruiker currentUser;
        public Voertuigen(Gebruiker user)
        {
            InitializeComponent();

            currentUser = user;
            voertuigen = Voertuig.GetAllVoertuigen();
            voertuigen = FilterVoertuigenByEigenaarId(voertuigen, currentUser.Id);

            UpdateVehicleDisplay();
        }

        private List<Voertuig> FilterVoertuigenByEigenaarId(List<Voertuig> voertuigen, int eigenaarId)
        {
            List<Voertuig> filteredVoertuigen = new List<Voertuig>();

            foreach (Voertuig voertuig in voertuigen)
            {
                if (voertuig.EigenaarId == eigenaarId)
                {
                    filteredVoertuigen.Add(voertuig);
                }
            }

            return filteredVoertuigen;
        }

        private void UpdateVehicleDisplay()
        {
            pnlItems.Children.Clear();

            List<Voertuig> filteredVoertuigen = voertuigen;

            // display the filtered vehicles
            foreach (Voertuig voertuig in filteredVoertuigen)
            {
                StackPanel pnl = new StackPanel();
                pnl.Margin = new Thickness(10);

                Image img = new Image();
                img.Width = 200;
                img.Source = Foto.GetBitmapImageFromByteArray(voertuig.ImageData);
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

                // create grid for buttons
                Grid buttonGrid = new Grid();
                buttonGrid.ColumnDefinitions.Add(new ColumnDefinition() { Width = new GridLength(1, GridUnitType.Star) }); // gebruik chatgpt om columndefinition toe te voegen
                buttonGrid.ColumnDefinitions.Add(new ColumnDefinition() { Width = new GridLength(1, GridUnitType.Star) });

                Button btnInfo = new Button();
                btnInfo.Content = "Info";
                btnInfo.HorizontalAlignment = HorizontalAlignment.Left;
                btnInfo.Click += (sender, e) =>
                {
                    selectedVoertuig = voertuig;
                    VoertuigInfo(sender, e);
                };

                Button btnVerwijderen = new Button();
                btnVerwijderen.Content = "Verwijderen";

                btnVerwijderen.DataContext = voertuig;
                btnVerwijderen.HorizontalAlignment = HorizontalAlignment.Right;
                btnVerwijderen.Click += BtnVerwijderen_Click;

                Button btnEdit = new Button();
                btnEdit.Content = "Edit";
                btnEdit.HorizontalAlignment = HorizontalAlignment.Center;
                btnEdit.Click += (sender, e) =>
                {
                    selectedVoertuig = voertuig;
                    BtnEdit_Click(sender, e);
                };

                buttonGrid.Children.Add(btnInfo);
                buttonGrid.Children.Add(btnVerwijderen);
                buttonGrid.Children.Add(btnEdit);

                Grid.SetColumn(btnVerwijderen, 1);

                pnl.Children.Add(buttonGrid);

                pnlItems.Children.Add(pnl);
            }
        }

        private void BtnEdit_Click(object sender, RoutedEventArgs e)
        {
            if (selectedVoertuig.Type == 1)
            {
                EditWindowGemotoriseerd detailsPage = new EditWindowGemotoriseerd(currentUser, selectedVoertuig);
                detailsPage.Tag = 1;
                detailsPage.tbxNaam.Text = $"{selectedVoertuig.Naam}";
                NavigationService.Navigate(detailsPage);
            }
            else if (selectedVoertuig.Type == 2)
            {
                EditPageGetrokken detailsPage = new EditPageGetrokken(currentUser);
                detailsPage.Tag = 2;
                detailsPage.tbxNaam.Text = $"{selectedVoertuig.Naam}";
                detailsPage.tbxBeschrijving.Text = $"{selectedVoertuig.Beschrijving}";
                detailsPage.tbxMerk.Text = $"{selectedVoertuig.Merk}";
                detailsPage.tbxGewicht.Text = $"{selectedVoertuig.Gewicht}";
                detailsPage.tbxMaxGewicht.Text = $"{selectedVoertuig.MaxBelasting}";
                detailsPage.tbxBouwjaar.Text = $"{selectedVoertuig.Bouwjaar}";
                detailsPage.tbxAfmetingen.Text = $"{selectedVoertuig.Bouwjaar}";

                NavigationService.Navigate(detailsPage);
                UpdateVehicleDisplay();
            }
        }

        private void BtnVerwijderen_Click(object sender, RoutedEventArgs e)
        {
            Button btnVerwijderen = (Button)sender;
            Voertuig voertuig = (Voertuig)btnVerwijderen.DataContext;

            MessageBoxResult result = MessageBox.Show("Weet je zeker dat je dit voertuig wilt verwijderen?", "Bevestig verwijdering", MessageBoxButton.YesNo, MessageBoxImage.Question);

            if (result == MessageBoxResult.Yes)
            {
                voertuig.DeleteFromDB();
                voertuigen.Remove(voertuig);
                UpdateVehicleDisplay();
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

                    detailsPage.lblTransmissie.Content = $"Transmissie: {selectedVoertuig.Transmissie}";
                    detailsPage.lblBeschrijving.Content = $"Beschrijving: {selectedVoertuig.Beschrijving}";
                    NavigationService.Navigate(detailsPage);
                }
            }
        }

        private void BtnToevoegen_Click(object sender, RoutedEventArgs e)
        {
            MessageBoxResult result = MessageBox.Show("Welk type voertuig wil je toevoegen?, Klik op Yes voor Gemotoriseerd, No voor Getrokken", "Voertuig Toevoegen", MessageBoxButton.YesNoCancel);

            if (result == MessageBoxResult.Yes)
            {
                MijnVoertuigenGemotoriseerd detailsPage = new MijnVoertuigenGemotoriseerd(currentUser);
                detailsPage.Tag = 2;
                NavigationService.Navigate(detailsPage);
            }
            else if (result == MessageBoxResult.No)
            {
                MijnVoertuigenGetrokken detailsPage = new MijnVoertuigenGetrokken(currentUser);
                detailsPage.Tag = 1;
                NavigationService.Navigate(detailsPage);
            }
        }
    }
}
