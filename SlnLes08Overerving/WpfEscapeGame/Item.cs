using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WpfEscapeGame
{
    internal class Item : Actor
    {
        internal bool IsPortable { get; set; } = true;
        internal bool IsLocked { get; set; } = false;
        public Item Key { get; set; }
        public Item HiddenItem { get; set; }

        public Item(string name, string desc) 
            : base(name, desc)
        {
        }

        public override string ToString()
        {
            return Name;
        }
    }
}
