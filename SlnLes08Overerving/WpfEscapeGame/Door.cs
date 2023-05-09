using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WpfEscapeGame
{
    internal class Door : LockableItem
    {
        public Room ConnectedRoom { get; set; }

        public Door(string name, string desc)
            : base(name, desc, true)
        {
        }

        public override string ToString()
        {
            return Name;
        }
    }
}
