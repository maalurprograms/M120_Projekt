using System;
using System.CodeDom;
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
    /// Dies ist die Ansicht in der die Einsätze in einem Kalender dargestellt werden.
    /// </summary>
    public partial class UC_KalenderAnsicht : UserControl
    {
        private DateTime[] Woche = new DateTime[7];
        private Dictionary<String, Grid> TagZuSpalte = new Dictionary<string, Grid>();
        public UC_KalenderAnsicht()
        {
            InitializeComponent();
            DateTime montag = DateTime.Now;
            switch (DateTime.Now.DayOfWeek)
            {
                case DayOfWeek.Tuesday:
                        montag = DateTime.Now.AddDays(-1);
                        break;

                case DayOfWeek.Wednesday:
                        montag = DateTime.Now.AddDays(-2);
                        break;

                case DayOfWeek.Thursday:
                        montag = DateTime.Now.AddDays(-3);
                        break;

                case DayOfWeek.Friday:
                        montag = DateTime.Now.AddDays(-4);
                        break;

                case DayOfWeek.Saturday:
                        montag = DateTime.Now.AddDays(-5);
                        break;

                case DayOfWeek.Sunday:
                        montag = DateTime.Now.AddDays(-6);
                        break;
            }
            setzeWoche(montag);
            TagZuSpalte.Add("Monday", montag_content);
            TagZuSpalte.Add("Tuesday", dienstag_content);
            TagZuSpalte.Add("Wednesday", mittwoch_content);
            TagZuSpalte.Add("Thursday", donnerstag_content);
            TagZuSpalte.Add("Friday", freitag_content);
            TagZuSpalte.Add("Saturday", samstag_content);
            TagZuSpalte.Add("Sunday", sonntag_content);
            generiereKalender();
        }
        private void generiereKalender()
        {
            montag_datum.Content += "\n" + Woche[0].Date.ToShortDateString();
            dienstag_datum.Content += "\n" + Woche[1].Date.ToShortDateString();
            mittwoch_datum.Content += "\n" + Woche[2].Date.ToShortDateString();
            donnerstag_datum.Content += "\n" + Woche[3].Date.ToShortDateString();
            freitag_datum.Content += "\n" + Woche[4].Date.ToShortDateString();
            samstag_datum.Content += "\n" + Woche[5].Date.ToShortDateString();
            sonntag_datum.Content += "\n" + Woche[6].Date.ToShortDateString();
            foreach (Einsatz e in Bibliothek.Einsatz_Alle())
            {
                foreach (DateTime tag in Woche)
                {
                    if (e.Start.Date == tag.Date)
                    {
                        Button feld = new Button();
                        feld.BorderThickness = new Thickness(0,0,0,0);
                        feld.Content = "ID: "+e.ID.ToString()+"\n";
                        feld.ToolTip = "Mitarbeiter: "+e.Mitarbeiter.Vorname+" "+e.Mitarbeiter.Name+"\nProjekt: "+e.Projekt.Name;
                        feld.Width = 50;
                        feld.Height = ((e.Ende.Hour * 60) + e.Ende.Minute) - ((e.Start.Hour * 60) + e.Start.Minute);
                        feld.Background = new SolidColorBrush(e.Farbe);
                        double topMargin = (e.Start.Hour*60) + e.Start.Minute;
                        feld.Margin = new Thickness(0, (e.Start.Hour * 60) + e.Start.Minute, 0, 0);
                        feld.HorizontalAlignment = HorizontalAlignment.Left;
                        feld.VerticalAlignment = VerticalAlignment.Top;
                        feld.Click += (sender, args) =>
                        {
                            EinzelAnsicht einzelAnsicht = new EinzelAnsicht(e.ID);
                            einzelAnsicht.Show();
                            einzelAnsicht.Closed += new EventHandler((o, args2) =>
                            {
                                Window.GetWindow(this).IsEnabled = true;
                                foreach (KeyValuePair<string, Grid> eintrag in TagZuSpalte)
                                {
                                    löscheKalender();
                                }
                                generiereKalender();
                            });
                            Window.GetWindow(this).IsEnabled = false;
                        };
                        TagZuSpalte[e.Start.DayOfWeek.ToString()].Children.Add(feld);
                        if (e.Mitarbeiter.ID > 1) feld.SetValue(Grid.ColumnProperty, 1);
                        else feld.SetValue(Grid.ColumnProperty, 0);
                        feld.SetValue(Grid.RowProperty, 0);
                    }
                }
            }
        }

        private void setzeWoche(DateTime montag)
        {
            Woche[0] = montag;
            Woche[1] = Woche[0].AddDays(1);
            Woche[2] = Woche[0].AddDays(2);
            Woche[3] = Woche[0].AddDays(3);
            Woche[4] = Woche[0].AddDays(4);
            Woche[5] = Woche[0].AddDays(5);
            Woche[6] = Woche[0].AddDays(6);
        }

        private void löscheKalender()
        {
            montag_datum.Content = "Montag";
            dienstag_datum.Content = "Dienstag";
            mittwoch_datum.Content = "Mittwoch";
            donnerstag_datum.Content = "Donnerstag";
            freitag_datum.Content = "Freitag";
            samstag_datum.Content = "Samstag";
            sonntag_datum.Content = "Sonntag";

            foreach (KeyValuePair<string, Grid> eintrag in TagZuSpalte)
            {
                eintrag.Value.Children.Clear();
            }
        }

        private void kalender_links_Click(object sender, RoutedEventArgs e)
        {
            setzeWoche(Woche[0].Date.AddDays(-7));
            löscheKalender();
            generiereKalender();
        }

        private void kalender_rechts_Click(object sender, RoutedEventArgs e)
        {
            setzeWoche(Woche[0].Date.AddDays(7));
            löscheKalender();
            generiereKalender();
        }
    }
}
