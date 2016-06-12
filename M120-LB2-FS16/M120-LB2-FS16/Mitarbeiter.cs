using System;
using System.Windows.Controls;
using System.Windows.Media;

namespace M120_LB2_FS16
{
    public class Mitarbeiter
    {
        public Int32 ID { get; set; }
        public String Name { get; set; }
        public String Vorname { get; set; }
        public Boolean IstAktiv { get; set; }
        public Image Bild { get; set; }
        public Color Farbe { get; set; }
        public Mitarbeiter()
        {

        }
    }
}
