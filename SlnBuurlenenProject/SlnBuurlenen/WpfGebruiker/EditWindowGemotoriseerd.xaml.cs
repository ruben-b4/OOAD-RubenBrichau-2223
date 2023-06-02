using System;
using System.Collections.Generic;
using System.IO;
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
using Microsoft.Win32;
using MyClassLibrary;

namespace WpfGebruiker
{
    /// <summary>
    /// Interaction logic for EditWindowGemotoriseerd.xaml
    /// </summary>
    public partial class EditWindowGemotoriseerd : Page
    {
        private Gebruiker currentUser;
        private Voertuig currentVoertuig;
        public EditWindowGemotoriseerd(Gebruiker user, Voertuig voertuig)
        {
            InitializeComponent();

            currentUser = user;
            currentVoertuig = voertuig;
        }
        private void BtnAfbeeldingen_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "Afbeeldingsbestanden|*.jpg;*.jpeg;*.png;*.gif";
            openFileDialog.Multiselect = true;

            if (openFileDialog.ShowDialog() == true)
            {
                string[] selectedFiles = openFileDialog.FileNames;

                int maxImages = 3;
                if (selectedFiles.Length > maxImages)
                {
                    MessageBox.Show($"Je kunt maximaal {maxImages} afbeeldingen selecteren.", "Waarschuwing", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }

                StackPanel imageStackPanel = new StackPanel();
                imageStackPanel.Orientation = Orientation.Horizontal;
                imageStackPanel.HorizontalAlignment = HorizontalAlignment.Left;
                imageStackPanel.Margin = new Thickness(0, 10, 0, 0);

                Voertuig voertuig = new Voertuig();

                for (int i = 0; i < selectedFiles.Length; i++)
                {
                    string imagePath = selectedFiles[i];
                    BitmapImage bitmapImage = new BitmapImage(new Uri(imagePath));

                    Image imageControl = new Image();
                    imageControl.Width = 200;
                    imageControl.Source = bitmapImage;

                    imageStackPanel.Children.Add(imageControl);
                    using (MemoryStream stream = new MemoryStream())
                    {
                        BitmapEncoder encoder = new JpegBitmapEncoder();
                        encoder.Frames.Add(BitmapFrame.Create(bitmapImage));
                        encoder.Save(stream);
                        voertuig.ImageData = stream.ToArray();
                    }
                }

                // Voeg de StackPanel met afbeeldingen toe aan de Grid
                Grid.SetColumn(imageStackPanel, 1);
                Grid.SetRow(imageStackPanel, 2);
                Grid.SetColumnSpan(imageStackPanel, 2);
                grid.Children.Add(imageStackPanel);
            }
        }

        private void BtnOpslaan_Click(object sender, RoutedEventArgs e)
        {
            if (!ValidateForm())
            {
                return;
            }

            string naam = tbxNaam.Text; // Update the current vehicle properties
            string beschrijving = tbxBeschrijving.Text;
            string merk = tbxMerk.Text;
            int bouwjaar;
            if (int.TryParse(tbxBouwjaar.Text, out bouwjaar))
            {
                currentVoertuig.Bouwjaar = bouwjaar;
            }
            else
            {
                currentVoertuig.Bouwjaar = null; // or any default value you prefer when the conversion fails
            }
            Brandstof brandstof = (Brandstof)cbxBrandstof.SelectedIndex + 1;
            Transmissie transmissie = (Transmissie)cbxTransmissie.SelectedIndex + 1;
            string model = tbxModel.Text;
            int eigenaarId = currentUser.Id;
            int type = (int)Tag;

            Voertuig voertuig = new Voertuig
            {
                Naam = naam,
                Beschrijving = beschrijving,
                Merk = merk,
                Bouwjaar = bouwjaar,
                Brandstof = brandstof,
                Transmissie = transmissie,
                Model = model,
                EigenaarId = eigenaarId,
                Type = type
            };

            if (imgVehicle.Source != null)
            {
                BitmapImage bitmapImage = (BitmapImage)imgVehicle.Source;
                using (MemoryStream stream = new MemoryStream())
                {
                    BitmapEncoder encoder = new JpegBitmapEncoder(); // gebruik gemaakt van chatgpt om img toe te voegen, functioneert niet
                    encoder.Frames.Add(BitmapFrame.Create(bitmapImage));
                    encoder.Save(stream);
                    voertuig.ImageData = stream.ToArray();
                }
            }

            int newId = currentVoertuig.UpdateInDb(naam, beschrijving, merk, bouwjaar, model, brandstof, transmissie, eigenaarId, type, currentVoertuig.ImageData);
            Voertuigen voertuigenPage = new Voertuigen(currentUser);
            NavigationService.Navigate(voertuigenPage);
        }

        private bool ValidateForm()
        {
            bool isValid = true;

            // Validate 'tbxNaam'
            if (string.IsNullOrWhiteSpace(tbxNaam.Text))
            {
                lblNaamCheck.Visibility = Visibility.Visible;
                isValid = false;
            }
            else
            {
                lblNaamCheck.Visibility = Visibility.Collapsed;
            }

            // Validate 'tbxBeschrijving'
            if (string.IsNullOrWhiteSpace(tbxBeschrijving.Text))
            {
                lblBeschrijvingCheck.Visibility = Visibility.Visible;
                isValid = false;
            }
            else
            {
                lblBeschrijvingCheck.Visibility = Visibility.Collapsed;
            }

            return isValid;
        }

        private void BtnAnnuleren_Click(object sender, RoutedEventArgs e)
        {
            Voertuigen voertuigenPage = new Voertuigen(currentUser);

            NavigationService.Navigate(voertuigenPage);
        }
    }
}
