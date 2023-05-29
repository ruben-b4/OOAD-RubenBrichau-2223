using System;
using System.Collections.Generic;
using System.Configuration;
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
    /// Interaction logic for Ontlening.xaml
    /// </summary>
    public partial class Ontlening : Page
    {
        private List<Voertuig> voertuigen;
        private Voertuig selectedVoertuig;
        private Ontlening ontlening;
        public Ontlening()
        {
            InitializeComponent();

            string connString = ConfigurationManager.ConnectionStrings["connStr"].ConnectionString;

            string query = "SELECT v.naam, v.merk, v.model, f.data, v.type, v.bouwjaar, v.beschrijving, v.eigenaar_id, v.gewicht, v.MaxBelasting, v.Geremd, v.Afmetingen " +
                           "FROM Voertuig v " +
                           "LEFT JOIN Foto f ON v.id = f.voertuig_id";
        }
    }
}
