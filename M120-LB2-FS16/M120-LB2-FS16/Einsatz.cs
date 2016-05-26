using System;
using System.Windows.Media;

namespace M120_LB2_FS16
{
    class Einsatz
    {
        public Int32 ID { get; set; }
        public DateTime Start { get; set; }
        public DateTime Ende { get; set; }
        public Projekt Projekt { get; set; }
        public Mitarbeiter Mitarbeiter { get; set; }
        public Color Farbe { get; set; }
        public Einsatz()
        {

        }
    }
}
