using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace WpfEscapeGame
{
    internal class LockableItem : Item
    {
        public new bool IsPortable { get; set; } = true;

        public LockableItem(string name, string desc, bool portable)
            : base(name, desc)
        {
            IsPortable = portable;
        }
    }
}
