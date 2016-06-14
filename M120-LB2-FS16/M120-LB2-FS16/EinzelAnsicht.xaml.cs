using System;
using System.Collections.Generic;
using System.Diagnostics.Eventing.Reader;
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
using System.Windows.Shapes;

namespace M120_LB2_FS16
{
    /// <summary>
    /// Interaktionslogik für EinzelAnsicht.xaml
    /// </summary>
    public partial class EinzelAnsicht : Window
    {
        private String Fehler_Zeit_ueberschneidung =
            "Dieser Mitarbeiter kann nicht an diesem Einsatz arbeiten," +
            " da er sich zeitlich mit einem anderen Einsatz des Mitarbeiters überschneidet.";

        private String Fehler_Nichts_geandert =
            "Sie müssen zuerst Änderungen vornhemen bevor Sie speichern können.";

        private String Fehler_Falsches_Zeit_Format =
            "Bitte geben Sie die Zeit in diesem Format an: 10:57.\n" +
            "Die Startzeit darf nicht ^nach der Endzeit liegen oder umgekehrt.\n"+
            "Bitte geben Sie das Datum in diesem Format an: 12.04.16 oder benützen Sie den Kalender zum auswählen.";

        private Einsatz einsatz;
        private bool Neu;

        public EinzelAnsicht(int einsatzID)
        {
            InitializeComponent();
            Neu = (einsatzID == 0);
            if (!Neu)
            {
                einsatz = Bibliothek.Einsatz_nach_ID(einsatzID);
                id.Content = einsatz.ID.ToString();
                setMitarbeiterBox();
                setProjektBox();
                datum.SelectedDate = einsatz.Start.Date;
                start_zeit.Text = einsatz.Start.ToShortTimeString();
                end_zeit.Text = einsatz.Ende.ToShortTimeString();
                farbe.Text = einsatz.Farbe.ToString();
            }
            else
            {
                this.einsatz = new Einsatz();
//              Nächste freie ID finden und für den neuen Einsatz setzen
                int freieID = 0;
                foreach (Einsatz einsatz in Bibliothek.Einsatz_Alle()) if (einsatz.ID > freieID) freieID = einsatz.ID;
                this.einsatz.ID = freieID + 1;
                id.Content = this.einsatz.ID.ToString();
                setMitarbeiterBox();
                setProjektBox();
            }
        }

        private void setMitarbeiterBox()
        {
            List<String> mitarbeiterNamen = new List<string>();
            foreach (Mitarbeiter mitarbeiter in Bibliothek.Mitarbeiter_Alle())
            {
                mitarbeiterNamen.Add(mitarbeiter.Vorname + " " + mitarbeiter.Name);
            }
            this.mitarbeiter.ItemsSource = mitarbeiterNamen;
            if (Neu)
            {
                this.mitarbeiter.SelectedIndex = 1;
            }
            else this.mitarbeiter.SelectedIndex = einsatz.Mitarbeiter.ID - 1;
        }

        private void setProjektBox()
        {
            List<String> projektNamen = new List<string>();
            foreach (Projekt projekt in Bibliothek.Projekt_Alle())
            {
                projektNamen.Add(projekt.Name);
            }
            this.projekt.ItemsSource = projektNamen;
            if (Neu)
            {
                this.projekt.SelectedIndex = 1;
            }
            else this.projekt.SelectedIndex = einsatz.Projekt.ID - 1;
        }

        private bool ueberpruefeZeit()
        {
            Einsatz testEinsatz = new Einsatz();
            setzeEinsatz(testEinsatz);
            bool keineUeberschneidung = true;
            Mitarbeiter mitarbeiter = Bibliothek.Mitarbeiter_nach_ID(this.mitarbeiter.SelectedIndex + 1);
            foreach (Einsatz e in Bibliothek.Einsatz_Alle())
            {
                if (testEinsatz.ID != e.ID)
                {
                    if (e.Mitarbeiter.ID == mitarbeiter.ID)
                    {
                        if (e.Start.Date == testEinsatz.Start.Date)
                        {       // Das Start oder Enddatum liegt auf dem zu überprüfenden Datum
                            if ((testEinsatz.Ende > testEinsatz.Start) ||
                                (testEinsatz.Start == e.Start || testEinsatz.Ende == e.Ende) || 
                                // Das Start und Enddatum liegt zwischen den zu überprüfenden Daten
                                (testEinsatz.Start > e.Start && testEinsatz.Ende < e.Ende) || 
                                // Das Startdatum liegt zwischen den zu überprüfenden Daten
                                (testEinsatz.Start > e.Start && testEinsatz.Start < e.Ende) ||
                                // Das Enddatum liegt zwischen den zu überprüfenden Daten
                                (testEinsatz.Ende > e.Start && testEinsatz.Ende < e.Ende) || 
                                // Das Start und das Enddatum  liegen ausserhalb der zu überprüfenden Daten.
                                (testEinsatz.Start<e.Start && testEinsatz.Ende>e.Ende))
                            {
                                MessageBox.Show(Fehler_Zeit_ueberschneidung);
                                keineUeberschneidung = false;
                            }
                        }
                    }
                }
            }
            return keineUeberschneidung;
        }

        private bool ueberpruefeEingaben()
        {
            try
            {
                setzeDatum(new Einsatz());
            }
            catch (Exception)
            {
                MessageBox.Show(Fehler_Falsches_Zeit_Format);
                return false;
            }
//          Noch die Farbe überprüfen aber weiss noch nicht wie ich das mache
            return true;
        }

        private void setzeEinsatz(Einsatz e)
        {
            e.ID = einsatz.ID;
            e.Mitarbeiter = Bibliothek.Mitarbeiter_nach_ID(this.mitarbeiter.SelectedIndex + 1);
            e.Projekt = Bibliothek.Projekt_nach_ID(this.projekt.SelectedIndex + 1);
            e.Farbe = new Color();
            setzeDatum(e);
        }

        private void setzeDatum(Einsatz e)
        {
            String startDatumString = datum.SelectedDate.Value.Year.ToString() + "-" + datum.SelectedDate.Value.Month.ToString() +
                                          "-" + datum.SelectedDate.Value.Day.ToString() + "T" + start_zeit.Text + ":00";
            e.Start = DateTime.Parse(startDatumString);
            String endDatumString = datum.SelectedDate.Value.Year.ToString() + "-" + datum.SelectedDate.Value.Month.ToString() +
                                    "-" + datum.SelectedDate.Value.Day.ToString() + "T" + end_zeit.Text + ":00";
            e.Ende = DateTime.Parse(endDatumString);
        }

        private void speichern_Click(object sender, RoutedEventArgs e)
        {
            if (ueberpruefeEingaben() && ueberpruefeZeit())
            {
                setzeEinsatz(this.einsatz);
                if (Neu) Bibliothek.EinsatzNeu(this.einsatz);
                this.Close();
            }
        }
    }
}