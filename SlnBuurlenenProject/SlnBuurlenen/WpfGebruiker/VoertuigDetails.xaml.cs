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
        private List<Voertuig> voertuigen;
        private Voertuig currentVoertuig;


        public VoertuigDetails()
        {
            InitializeComponent();

        }

        private void BtnBevestigen_Click(object sender, RoutedEventArgs e)
        {


        }
    }
}
