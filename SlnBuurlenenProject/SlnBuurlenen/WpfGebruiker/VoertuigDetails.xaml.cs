using System;
using System.Collections.Generic;
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
    /// Interaction logic for VoertuigDetails.xaml
    /// </summary>
    public partial class VoertuigDetails : Page
    {
        private Gebruiker currentUser;
        private Voertuig currentVoertuig;

        public VoertuigDetails()
        {
            InitializeComponent();
            currentVoertuig = new Voertuig();
            currentUser = new Gebruiker();
        }

        private void BtnBevestigen_Click(object sender, RoutedEventArgs e)
        {
            DateTime vanaf = dtpVan.SelectedDate ?? DateTime.MinValue;
            DateTime tot = dtpTot.SelectedDate ?? DateTime.MinValue;
            string bericht = txtBericht.Text;

            if (vanaf == DateTime.MinValue || tot == DateTime.MinValue)
            {
                MessageBox.Show("Selecteer een geldige start- en einddatum.", "Ongeldige invoer", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            if (vanaf > tot)
            {
                MessageBox.Show("De einddatum kan niet voor de startdatum liggen.", "Ongeldige invoer", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            MyClassLibrary.Ontlening newOntlening = new MyClassLibrary.Ontlening()
            {
                Vanaf = vanaf,
                Tot = tot,
                Bericht = bericht,
                Status = MyClassLibrary.Status.InBehandeling,
                VoertuigId = currentVoertuig.Id,
                AanvragerId = currentUser.Id
            };

            // Add the new Ontlening to the database
            newOntlening.SaveToDB();

            Ontlening ontleningPage = (Ontlening)Application.Current.MainWindow.Content;
            ontleningPage.AddOntleendItem($"{currentVoertuig.Naam} - {vanaf.ToShortDateString()} tot {tot.ToShortDateString()}", newOntlening, Brushes.Yellow);

            MessageBox.Show("Ontlening toegevoegd.", "Succes", MessageBoxButton.OK, MessageBoxImage.Information);
        }
    }
}
