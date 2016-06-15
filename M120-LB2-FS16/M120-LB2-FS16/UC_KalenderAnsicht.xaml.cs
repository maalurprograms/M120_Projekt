using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace M120_LB2_FS16
{
    /// <summary>
    /// Interaktionslogik für UC_KalenderAnsicht.xaml
    /// Dies ist die Ansicht in der die Einsätze in einem Kalender dargestellt werden.
    /// </summary>
    public partial class UC_KalenderAnsicht : UserControl
    {
        // Die aktuelle Woce in Daten.
        private DateTime[] Woche = new DateTime[7];
        // Hier wird der Tag zu einem Grid zugewiesen also z.B.: "Monday": montag_content
        private Dictionary<String, Grid> TagZuSpalte = new Dictionary<string, Grid>();
        public UC_KalenderAnsicht()
        {
            InitializeComponent();
            // Es wird herausgefunden welches Datum der sich in der aktuellen Woche befindenden Montag hat.
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
            // Dann, wenn das Daum des Montags herausgefunden wurde, wird das Dictionary Woche gefüllt.
            setzeWoche(montag);
            TagZuSpalte.Add("Monday", montag_content);
            TagZuSpalte.Add("Tuesday", dienstag_content);
            TagZuSpalte.Add("Wednesday", mittwoch_content);
            TagZuSpalte.Add("Thursday", donnerstag_content);
            TagZuSpalte.Add("Friday", freitag_content);
            TagZuSpalte.Add("Saturday", samstag_content);
            TagZuSpalte.Add("Sunday", sonntag_content);
            // Danach wird der Kalender generiert.
            generiereKalender();
        }

        /// <summary>
        /// Diese Methode generiert den Kalender und sezt die Aktuelle Woche. Wenn sich in dieser Woche
        /// Einsätze befinden, werden diese mit Dauer angezeigt.
        /// </summary>
        private void generiereKalender()
        {
            // Den Kopfzeilen werden das Datum hinzugefügt.
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
                        // Wenn das Datum des Wochentages des Einsatzes mit dem Datum des Wochentages in der foreach Schlaufe übereinstimmt,
                        // Wird ein Button generiert.
                        Button feld = new Button();
                        feld.BorderThickness = new Thickness(0,0,0,0);
                        // Der Text des Buttons wird auf die ID des Einsatzes gesezt da für mehr kein Platz ist.
                        feld.Content = "ID: "+e.ID.ToString();
                        // Als ToolTip werden weitere Informationen angezeigt.
                        feld.ToolTip = "Mitarbeiter: "+e.Mitarbeiter.Vorname+" "+e.Mitarbeiter.Name+"\nProjekt: "+e.Projekt.Name;
                        feld.Width = 50;
                        // Die höhe des Buttons entspricht der Dauer des Einsatzes. Eine Minute entspricht einem Pixel.
                        feld.Height = ((e.Ende.Hour * 60) + e.Ende.Minute) - ((e.Start.Hour * 60) + e.Start.Minute);
                        // Die Hintergrundfarbe des Buttons wird auf die Farbe des EInsatzes gsesezt.
                        feld.Background = new SolidColorBrush(e.Farbe);
                        // Der Abstand oben ist die Startzeit in Pixel also bei 02:00 wären es 120 Pixel.
                        // Das bewirkt dass der Button Dort anfängt wo die Startzeit ist.
                        feld.Margin = new Thickness(0, (e.Start.Hour * 60) + e.Start.Minute, 0, 0);
                        // Nun wird noch definiert das der Button standartmässig im linken oberen Ecken ist ( falls keine Abstände definiert sind ).
                        feld.HorizontalAlignment = HorizontalAlignment.Left;
                        feld.VerticalAlignment = VerticalAlignment.Top;
                        // Es wird noch ein EventHandler gesezt das wenn auf den Button geklickt wird,
                        // wird der Einsatz in der Einzel Ansicht dargestellt und kann bearbeitet werden.
                        feld.Click += (sender, args) =>
                        {
                            // Eine Einzel Ansicht wird erstellt und angezeigt. 
                            EinzelAnsicht einzelAnsicht = new EinzelAnsicht(e.ID);
                            einzelAnsicht.Show();
                            // Das MainWindow wird gesperrt.
                            Window.GetWindow(this).IsEnabled = false;
                            // Wenn das Fenster geschlossen wird, soll es den Kalender neu erstellen,
                            // da die Daten des EInsatzes geändert werden können während die EInzel Ansicht offen ist.
                            einzelAnsicht.Closed += new EventHandler((o, args2) =>
                            {
                                // Das MainWindow wird wieder aktiviert.
                                Window.GetWindow(this).IsEnabled = true;
                                // Für jeden Wochentag wird das Grid geleert.
                                foreach (KeyValuePair<string, Grid> eintrag in TagZuSpalte)
                                {
                                    löscheKalender();
                                }
                                // Nun wird der Kalender neu generiert.
                                generiereKalender();
                            });
                        };
                        // Der Button wird zu dem passenden Grid hinzugefügt.
                        TagZuSpalte[e.Start.DayOfWeek.ToString()].Children.Add(feld);
                        // Wenn die Mitarbeiter ID grösser 1 ist, wird der Einsatz in die rechte Kolonne gesezt.
                        if (e.Mitarbeiter.ID > 1) feld.SetValue(Grid.ColumnProperty, 1);
                        // Wenn die Mitarbeiter ID kleiner 1 ist, wird der Einsatz in die linke Kolonne gesezt.
                        else feld.SetValue(Grid.ColumnProperty, 0);
                        // Der Button wird im Grid in die erste Reihe gesezt.
                        feld.SetValue(Grid.RowProperty, 0);
                    }
                }
            }
        }

        /// <summary>
        /// Diese Methode generiert die Woche aus dem Parameter montag und füllt die Woche Liste.
        /// </summary>
        /// <param name="montag">Montag dieser Woche als DateTime</param>
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

        /// <summary>
        /// Diese Methode sezt den Kalender auf Standart zurück und entfert alle Einsätze.
        /// </summary>
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
                // In jedem Wochentag wird das Grid geleert.
                eintrag.Value.Children.Clear();
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender">Button Objekt</param>
        /// <param name="e">Event Parameter</param>
        private void kalender_links_Click(object sender, RoutedEventArgs e)
        {
            // Die lezte Woche wird generiert danach der Kalender neu erstellt.
            setzeWoche(Woche[0].Date.AddDays(-7));
            löscheKalender();
            generiereKalender();
        }

        /// <summary>
        /// Sezt die Woche eines vor.
        /// </summary>
        /// <param name="sender">Button Objekt</param>
        /// <param name="e">Event Parameter</param>
        private void kalender_rechts_Click(object sender, RoutedEventArgs e)
        {
            // Die nächste Woche wird generiert danach der Kalender neu erstellt.
            setzeWoche(Woche[0].Date.AddDays(7));
            löscheKalender();
            generiereKalender();
        }
    }
}
