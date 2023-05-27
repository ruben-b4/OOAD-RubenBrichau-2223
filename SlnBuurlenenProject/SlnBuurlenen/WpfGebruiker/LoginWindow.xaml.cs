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
using System.Windows.Shapes;
using MyClassLibrary;

namespace WpfGebruiker
{
    /// <summary>
    /// Interaction logic for LoginWindow.xaml
    /// </summary>
    public partial class LoginWindow : Window
    {
        private string connString = ConfigurationManager.ConnectionStrings["connStr"].ConnectionString;
        public LoginWindow()
        {
            InitializeComponent();
        }

        private void TextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            // this.btnLogin.IsEnabled = this.tbxLogin != null && this.tbxWachtwoord.Text != null;
        }

        private void BtnLogin_Click(object sender, RoutedEventArgs e)
        {
            Gebruiker gebruiker = new Gebruiker();

            if (!gebruiker.UserInDB(tbxLogin.Text, tbxWachtwoord.Text))
            {
                lbxResults.Visibility = Visibility.Visible;
            } 
            else
            {
                MainWindow mainwindow = new MainWindow(gebruiker);
                mainwindow.Show();
                this.Close();
            }
        }
    }
}
