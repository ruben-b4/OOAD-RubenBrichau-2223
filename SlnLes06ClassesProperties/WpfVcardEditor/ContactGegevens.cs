using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;

public class Contactgegevens
{
    public string Voornaam { get; set; }
    public string Achternaam { get; set; }
    public string Nickname { get; set; }
    public string Gender { get; set; }
    public DateTime? Geboortedatum { get; set; }
    public string PriveEmail { get; set; }
    public string WerkEmail { get; set; }
    public string PriveTelefoon { get; set; }
    public string WerkTelefoon { get; set; }
    public string Land { get; set; }
    public string Jobtitel { get; set; }
    public string Bedrijf { get; set; }
    public string Facebook { get; set; }
    public string LinkedIn { get; set; }
    public string Instagram { get; set; }
    public string Youtube { get; set; }
    public BitmapImage Foto { get; set; }
    public DateTime UpdateDatum { get; set; }
}
