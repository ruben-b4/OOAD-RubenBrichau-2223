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
using System.Windows.Media.Media3D;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace WpfKerstverlichting
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private readonly Random random = new Random();
        private readonly List<Ellipse> lights = new List<Ellipse>();
        public MainWindow()
        {
            InitializeComponent();

            double treeWidth = imgTree.Width;
            double treeHeight = imgTree.Height;

            // maak het lampje met een ellips (cirkel)
            for (int i = 0; i < 41; i++)
            {
                Ellipse newLight = new Ellipse();
                newLight.Width = 10;
                newLight.Height = 10;
                newLight.Fill = Brushes.Gray;
                newLight.Stroke = Brushes.Black;

                bool isOnWhitePixel = true;
                while (isOnWhitePixel)
                {
                    double xPos = random.NextDouble() * (treeWidth - newLight.Width);
                    double yPos = random.NextDouble() * (treeHeight - newLight.Height);
                    newLight.SetValue(Canvas.LeftProperty, xPos);
                    newLight.SetValue(Canvas.TopProperty, yPos);
                    isOnWhitePixel = PixelIsWhite(imgTree, (int)xPos, (int)yPos);
                }

                // voeg ellips toe aan het canvas
                cnvTree.Children.Add(newLight);
                lights.Add(newLight);
            }
        }

        

        public static bool PixelIsWhite(Image img, int x, int y)
        {
            BitmapSource source = img.Source as BitmapSource;
            Color color = Colors.White;
            CroppedBitmap cb = new CroppedBitmap(source, new Int32Rect(x, y, 1, 1));
            byte[] pixels = new byte[4];
            cb.CopyPixels(pixels, 4, 0);
            color = Color.FromRgb(pixels[2], pixels[1], pixels[0]);
            return color.ToString() == "#FFFFFFFF";
        }

      
    }
}
