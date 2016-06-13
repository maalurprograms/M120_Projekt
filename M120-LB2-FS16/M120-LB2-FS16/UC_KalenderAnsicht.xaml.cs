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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace M120_LB2_FS16
{
    /// <summary>
    /// Interaktionslogik für UC_KalenderAnsicht.xaml
    /// </summary>
    public partial class UC_KalenderAnsicht : UserControl
    {
        private DateTime[] Woche = new DateTime[7];
        public UC_KalenderAnsicht()
        {
            InitializeComponent();
            setWoche();
            foreach (Einsatz e in Bibliothek.Einsatz_Alle())
            {
                foreach (DateTime tag in Woche)
                {
                    if (e.Start.Date == tag.Date)
                    {
                        MessageBox.Show(e.ID.ToString() + " " + e.Start.Date.DayOfWeek);
                    }
                }
            }
        }

        private void setWoche()
        {
            Woche[0] = DateTime.Now;
            switch (DateTime.Now.DayOfWeek)
            {
                case DayOfWeek.Tuesday:
                    {
                        Woche[0] = DateTime.Now.AddDays(-1);
                        break;
                    }

                case DayOfWeek.Wednesday:
                    {
                        Woche[0] = DateTime.Now.AddDays(-2);
                        break;
                    }

                case DayOfWeek.Thursday:
                    {
                        Woche[0] = DateTime.Now.AddDays(-3);
                        break;
                    }

                case DayOfWeek.Friday:
                    {
                        Woche[0] = DateTime.Now.AddDays(-4);
                        break;
                    }
                case DayOfWeek.Saturday:
                    {
                        Woche[0] = DateTime.Now.AddDays(-5);
                        break;
                    }
                case DayOfWeek.Sunday:
                    {
                        Woche[0] = DateTime.Now.AddDays(-6);
                        break;
                    }
            }
            montag_datum.Content += "\n" + Woche[0].Date.ToShortDateString();
            Woche[1] = Woche[0].AddDays(1);
            dienstag_datum.Content += "\n" + Woche[1].Date.ToShortDateString();
            Woche[2] = Woche[0].AddDays(2);
            mittwoch_datum.Content += "\n" + Woche[2].Date.ToShortDateString();
            Woche[3] = Woche[0].AddDays(3);
            donnerstag_datum.Content += "\n" + Woche[3].Date.ToShortDateString();
            Woche[4] = Woche[0].AddDays(4);
            freitag_datum.Content += "\n" + Woche[4].Date.ToShortDateString();
            Woche[5] = Woche[0].AddDays(5);
            samstag_datum.Content += "\n" + Woche[5].Date.ToShortDateString();
            Woche[6] = Woche[0].AddDays(6);
            sonntag_datum.Content += "\n" + Woche[6].Date.ToShortDateString();
        }
    }
}
