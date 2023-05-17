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

namespace WpfGebruiker
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private string connString = ConfigurationManager.ConnectionStrings["connStr"].ConnectionString;
        public MainWindow()
        {
            InitializeComponent();
            using (SqlConnection conn = new SqlConnection(connString))
            {
                // open connection
                conn.Open();

                // voer connectie uit 
                SqlCommand comm = new SqlCommand("SELECT id, voornaam, achternaam FROM Gebruiker", conn);
                SqlDataReader reader = comm.ExecuteReader();

                // lees en verwerk resultaten
                while (reader.Read())
                {
                    int id = Convert.ToInt32(reader["id"]);
                    string firstname = Convert.ToString(reader["voornaam"]);
                    string lastname = Convert.ToString(reader["achternaam"]);
                    ListBoxItem item = new ListBoxItem();
                    item.Content = $"{id}: {firstname} {lastname} ";
                }
            }
        }
    }
}
