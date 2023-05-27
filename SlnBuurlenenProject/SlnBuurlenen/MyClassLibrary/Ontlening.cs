using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyClassLibrary
{
    public enum Status
    {
        InAanvraag,
        Goedgekeurd,
        Verworpen
    }

    public class Ontlening
    {
        public int Id { get; set; }
        public DateTime Vanaf { get; set; }
        public DateTime Tot { get; set; }
        public string Bericht { get; set; }
        public Status Status { get; set; }
        public int VoertuigId { get; set; }
        public int AanvragerId { get; set; }
        public Ontlening()
        {
        }
    }
}
