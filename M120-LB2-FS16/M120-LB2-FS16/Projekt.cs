using System;
using System.Collections.Generic;
using System.Windows.Media;

namespace M120_LB2_FS16
{
    class Projekt
    {
        public Int32 ID { get; set; }
        public String Name { get; set; }
        public DateTime StartDatum { get; set; }
        public DateTime EndDatum { get; set; }
        public Double GesamtZeitStunden { get; set; }
        public Double OffeneZeitStunden { get; set; }
        public Boolean IstAktiv { get; set; }
        public Color Farbe { get; set; }
        public List<Einsatz> Einsaetze { get; set; }

        public Projekt()
        {
            Einsaetze = new List<Einsatz>();
        }

    }
}
