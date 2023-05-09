﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace WpfEscapeGame
{
    internal class Room : Actor
    {
        public List<Item> Items { get; set; } = new List<Item>();
        public List<Door> Doors { get; set; }
        public Room(string name, string desc)
            : base(name, desc)
        {
            Name = name;
            Description = desc;
            Doors = new List<Door>();
        }
    }
}
