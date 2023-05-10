using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace WpfEscapeGame
{
    internal class Room : Actor
    {
        // https://softwareengineering.stackexchange.com/questions/333591/is-it-ok-to-have-an-empty-abstract-class-to-make-concrete-classes-polymorphic
        public List<Item> Items { get; set; } = new List<Item>();
        public ImageSource Image { get; set; }
        public Room(string name, string desc, ImageSource image)
            : base(name, desc)
        {
            this.Image = image;
        }
    }
}
