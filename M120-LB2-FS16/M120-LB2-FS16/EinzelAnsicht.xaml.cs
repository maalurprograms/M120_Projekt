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
using System.Windows.Shapes;

namespace M120_LB2_FS16
{
    /// <summary>
    /// Interaktionslogik für EinzelAnsicht.xaml
    /// </summary>
    public partial class EinzelAnsicht : Window
    {
        private Einsatz einsatz;
        public EinzelAnsicht(int einsatzID)
        {
            InitializeComponent();
            einsatz = Bibliothek.Einsatz_nach_ID(einsatzID);
            setMitarbeiterBox();
            setProjektBox();
            datum.SelectedDate = einsatz.Start.Date;
            start_zeit.Text = einsatz.Start.ToShortTimeString();
            end_zeit.Text = einsatz.Ende.ToShortTimeString();
            farbe.Text = einsatz.Farbe.ToString();
        }

        private void mitarbeiter_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            speichern.IsEnabled = ueberpruefeZeit();
        }

        private void setMitarbeiterBox()
        {
            List<String> mitarbeiterNamen = new List<string>();
            foreach (Mitarbeiter mitarbeiter in Bibliothek.Mitarbeiter_Alle())
            {
                mitarbeiterNamen.Add(mitarbeiter.Vorname + " " + mitarbeiter.Name);
            }
            this.mitarbeiter.ItemsSource = mitarbeiterNamen;
            this.mitarbeiter.SelectedIndex = einsatz.Mitarbeiter.ID - 1;
        }

        private void setProjektBox()
        {
            List<String> projektNamen = new List<string>();
            foreach (Projekt projekt in Bibliothek.Projekt_Alle())
            {
                projektNamen.Add(projekt.Name);
            }
            this.projekt.ItemsSource = projektNamen;
            this.projekt.SelectedIndex = einsatz.Projekt.ID - 1;
        }

        private bool ueberpruefeZeit()
        {
            bool keineUeberschneidung = true;
            Mitarbeiter mitarbeiter = Bibliothek.Mitarbeiter_nach_ID(this.mitarbeiter.SelectedIndex + 1);
            foreach (Einsatz einsatz in Bibliothek.Einsatz_Alle())
            {
                if (einsatz.Mitarbeiter.ID == mitarbeiter.ID)
                {
                    if (einsatz.Start > this.einsatz.Start && einsatz.Start < this.einsatz.Ende)
                    {
                        MessageBox.Show(
                            "Dieser Mitarbeiter kann nicht an diesem Einsatz arbeiten da er sich zeitlich mit einem anderen Einsatz des Mitarbeiters überschneidet.");
                        keineUeberschneidung = false;
                    }
                    else if (einsatz.Ende > this.einsatz.Start && einsatz.Ende < this.einsatz.Ende)
                    {
                        MessageBox.Show(
                            "Dieser Mitarbeiter kann nicht an diesem Einsatz arbeiten da er sich zeitlich mit einem anderen Einsatz des Mitarbeiters überschneidet.");
                        keineUeberschneidung = false;
                    }
                }
            }
            return keineUeberschneidung;
        }
    }
}
