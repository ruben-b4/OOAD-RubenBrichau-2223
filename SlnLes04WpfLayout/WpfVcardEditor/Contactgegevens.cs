using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;
using Microsoft.Win32;

namespace WpfVcardEditor
{
    internal class Contactgegevens
    {
        public class VCard
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

        public class VCardWriter
        {
            public void Write(VCard vCard, string filePath)
            {
                SaveFileDialog saveFileDialog = new SaveFileDialog();
                saveFileDialog.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
                saveFileDialog.Filter = "vCard Files (*.vcf|*.vcf";
                saveFileDialog.FileName = "vCard.vcf";

                if (saveFileDialog.ShowDialog() == true)
                {
                    // open stream and start writing
                    using (StreamWriter writer = new StreamWriter(saveFileDialog.FileName))
                    {
                        writer.WriteLine("BEGIN:VCARD");
                        writer.WriteLine("VERSION:3.0");
                        writer.WriteLine($"FN;CHARSET=UTF-8:{vCard.Voornaam} {vCard.Achternaam}");
                        writer.WriteLine($"N;CHARSET=UTF-8:{vCard.Achternaam};{vCard.Voornaam};;;");
                        writer.WriteLine($"NICKNAME;CHARSET=UTF-8:{vCard.Nickname}");
                        writer.WriteLine($"GENDER:{vCard.Gender}");
                        writer.WriteLine($"BDAY:{vCard.Geboortedatum?.ToString("yyyyMMdd")}");
                        writer.WriteLine($"EMAIL;CHARSET=UTF-8;type=HOME,INTERNET:{vCard.PriveEmail}");
                        writer.WriteLine($"EMAIL;CHARSET=UTF-8;type=WORK,INTERNET:{vCard.WerkEmail}");
                        writer.WriteLine($"TEL;TYPE=HOME,VOICE:{vCard.PriveTelefoon}");
                        writer.WriteLine($"TEL;TYPE=WORK,VOICE:{vCard.WerkTelefoon}");
                        writer.WriteLine($"ADR;CHARSET=UTF-8;TYPE=HOME:;;;;;;{vCard.Land}");
                        writer.WriteLine($"TITLE;CHARSET=UTF-8:{vCard.Jobtitel}");
                        writer.WriteLine($"ORG;CHARSET=UTF-8:{vCard.Bedrijf}");
                        writer.WriteLine($"X-SOCIALPROFILE;TYPE=facebook:{vCard.Facebook}");
                        writer.WriteLine($"X-SOCIALPROFILE;TYPE=linkedin:{vCard.LinkedIn}");
                        writer.WriteLine($"X-SOCIALPROFILE;TYPE=instagram:{vCard.Instagram}");
                        writer.WriteLine($"X-SOCIALPROFILE;TYPE=youtube:{vCard.Youtube}");
                        writer.WriteLine($"REV:{vCard.UpdateDatum.ToString("yyyy-MM-ddTHH:mm:ss.fffZ")}");    // bron https://stackoverflow.com/questions/296920/how-do-you-get-the-current-time-of-day

                        // bron grotendeels chat gpt
                        if (vCard.Foto != null)
                        {
                            JpegBitmapEncoder encoder = new JpegBitmapEncoder();
                            encoder.Frames.Add(BitmapFrame.Create(vCard.Foto));
                            using (MemoryStream ms = new MemoryStream())
                            {
                                encoder.Save(ms);
                                string base64String = Convert.ToBase64String(ms.ToArray());
                                writer.WriteLine($"PHOTO;ENCODING=b;TYPE=JPEG:{base64String}");
                            }
                        }
                        writer.WriteLine("END:VCARD");
                    } // stream closes automatically
                }
            }
        }
    }
}
