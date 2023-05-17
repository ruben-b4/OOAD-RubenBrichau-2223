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
            this.btnLogin.IsEnabled = this.tbxLogin != null && this.tbxWachtwoord.Text != null;
        }

        private void BtnLogin_Click(object sender, RoutedEventArgs e)
        {
            using (SqlConnection conn = new SqlConnection(connString))
            {
                // open connection
                conn.Open();

                // voer connectie uit 
                SqlCommand comm = new SqlCommand("SELECT email, paswoord FROM Gebruiker where email = @Email AND paswoord = @Password", conn);
                comm.Parameters.AddWithValue("@Email", tbxLogin.Text);
                string password = tbxWachtwoord.Text;

                SqlDataReader reader = comm.ExecuteReader();

                // lees en verwerk resultaten
                while (reader.Read())
                {
                    int id = Convert.ToInt32(reader["id"]);
                    string firstname = Convert.ToString(reader["voornaam"]);
                    string lastname = Convert.ToString(reader["achternaam"]);
                    ListBoxItem item = new ListBoxItem();
                    item.Content = $"{id}: {firstname} {lastname} ";
                    item.Tag = id;
                    lbxResults.Items.Add(item);
                }
            }
        }
    }
}
