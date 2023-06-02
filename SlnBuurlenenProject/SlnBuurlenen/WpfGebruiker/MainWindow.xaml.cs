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
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private string connString = ConfigurationManager.ConnectionStrings["connStr"].ConnectionString;
        private Gebruiker gebruiker;
        
        public MainWindow(Gebruiker gb)
        {
            InitializeComponent();
            gebruiker = gb;
            Main.Content = new Home(gebruiker);
        }

        private void BtnHome_Click(object sender, RoutedEventArgs e)
        {
            Main.Content = new Home(gebruiker);
        }

        private void BtnVoertuigen_Click(object sender, RoutedEventArgs e)
        {
            Main.Content = new Voertuigen(gebruiker);
        }

        private void BtnOntlening_Click(object sender, RoutedEventArgs e)
        {
            Main.Content = new Ontlening(gebruiker);
        }
    }
}
