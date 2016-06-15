using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Media;

namespace M120_LB2_FS16
{
    /// <summary>
    /// Interaktionslogik für EinzelAnsicht.xaml
    /// Diese Klasse dient dazu, einen Einsatz zu bearbeiten oder zu erstellen.
    /// </summary>
    public partial class EinzelAnsicht : Window
    {
        // Die Fehler werden gesezt da wir sie später z.B. bei der Zeitüberprüfung benötigen
        private String Fehler_Zeit_ueberschneidung =
            "Dieser Mitarbeiter kann nicht an diesem Einsatz arbeiten," +
            " da er sich zeitlich mit einem anderen Einsatz des Mitarbeiters überschneidet.";

        private String Fehler_Falsches_Zeit_Format =
            "Bitte geben Sie die Zeit in diesem Format an: 10:57.\n" +
            "Die Startzeit darf nicht nach der Endzeit liegen oder umgekehrt.\n"+
            "Der Einsatz darf nicht ausserhalb der Projekt Zeit sein\n"+
            "Bitte geben Sie das Datum in diesem Format an: 12.04.16 oder benützen Sie den Kalender zum auswählen.";

        // Nun wird der Einsatz definiert der später neu erstellt oder abgeändert wird.
        private Einsatz einsatz;
        // Ist neu gesezt wird ein neuer Einsatz erstellt.
        private bool Neu;

        /// <param name="einsatzID">Die ID des Einsatzes der geändert werden soll oder 0 bei einem neuen Einsatz</param>
        public EinzelAnsicht(int einsatzID)
        {
            InitializeComponent();
            // Wenn einsatzID 0 ist wird Neu auf True gesezt und es ist nun klar das ein neuer Einsatz erstellt werden soll.
            Neu = (einsatzID == 0);
            if (!Neu)
            {
                // Falls kein neuer Einsatz erstellt werden soll, wird die Instanzvariabel einsatz auf das Objekt gesezt das Einsatz_nach_ID() zurückgibt.
                einsatz = Bibliothek.Einsatz_nach_ID(einsatzID);
                // Nun werden noch die Felder in der Benutzeroberfläche mit den richtigen Werten versehen.
                id.Content = einsatz.ID.ToString();
                setMitarbeiterBox();
                setProjektBox();
                datum.SelectedDate = einsatz.Start.Date;
                start_zeit.Text = einsatz.Start.ToShortTimeString();
                end_zeit.Text = einsatz.Ende.ToShortTimeString();
                farbe.Background = new SolidColorBrush(einsatz.Farbe);
            }
            else
            {
                // Wenn ein neuer Einsatz erstellt werden soll, wird ein neues Objekt vom Typ Einsatz erstellt und an die Referenz einsatz gehängt.
                this.einsatz = new Einsatz();
                // Nun wird die nächst höchste ID gesucht und diesem Einsatz vergeben.
                int freieID = 0;
                foreach (Einsatz einsatz in Bibliothek.Einsatz_Alle()) if (einsatz.ID > freieID) freieID = einsatz.ID;
                this.einsatz.ID = freieID + 1;
                // Nun werden noch die Felder in der Benutzeroberfläche mit den standart Werten versehen.
                id.Content = this.einsatz.ID.ToString();
                setMitarbeiterBox();
                setProjektBox();
                farbe.Background = new SolidColorBrush(Colors.LightBlue);
            }
        }

        /// <summary>
        /// Diese Methode liest alle Mitarbeiter und schreibt Sie in eine ComboBox
        /// damit Einsätze an andere Personen weitergegeben werden können.
        /// </summary>
        private void setMitarbeiterBox()
        {
            List<String> mitarbeiterNamen = new List<string>();
            // Für jeden Mitarbeiter wird dessen kompletter Name in eine Liste gespeichert
            foreach (Mitarbeiter mitarbeiter in Bibliothek.Mitarbeiter_Alle())
            {
                mitarbeiterNamen.Add(mitarbeiter.Vorname + " " + mitarbeiter.Name);
            }
            // Dann wird diese Liste als Quelle für die ComboBox der Mitarbeiter angegeben
            this.mitarbeiter.ItemsSource = mitarbeiterNamen;
            // Wenn der Einsatz bereits existiert, wird der Mitarbeiter als Standart gesezt, der am Einsatz arbeitet.
            // Falls dies ein neuer Einsatz ist, wird einfach der erste Mitarbeiter als Standart gesezt.
            if (Neu)this.mitarbeiter.SelectedIndex = 1;
            else this.mitarbeiter.SelectedIndex = einsatz.Mitarbeiter.ID - 1;
        }

        /// <summary>
        /// Diese Methode liest alle Projekte und schreibt Sie in eine ComboBox
        /// damit Einsätze anderen Projekten zugeteilt werden können.
        /// </summary>
        private void setProjektBox()
        {
            List<String> projektNamen = new List<string>();
            foreach (Projekt projekt in Bibliothek.Projekt_Alle())
            {
                // Für jedes Projekt wird der Name in eine Liste gespeichert
                projektNamen.Add(projekt.Name);
            }
            // Dann wird diese Liste als Quelle für die ComboBox der Projekte angegeben
            this.projekt.ItemsSource = projektNamen;
            // Wenn der Einsatz sbereits existiert, wird das Projekt als Standart gesezt zudem der Einsatz gehört.
            // Falls dies ein neuer Einsatz ist, wird einfach das erste Projekt als Standart gesezt.
            if (Neu)this.projekt.SelectedIndex = 1;
            else this.projekt.SelectedIndex = einsatz.Projekt.ID - 1;
        }

        /// <summary>
        /// Diese Methode überprüft, dass der aktuelle Einsatz ( ob neu oder geändert ist in diesem Falle egal ) keine Zeitüberschneidung mit einem existierenden Einsatz hat.
        /// </summary>
        /// <returns>Gibt False zurück wenn eine Zeitüberschneidung mit einem existierenden Einsatz besteht und True wenn nicht</returns>
        private bool ueberpruefeZeit()
        {
            /*
             * Es wird ein neuer Einsatz mit den gleichen werten wie der neue oder bearbeitete Einsatz.
             * Dies ist notwendig da wenn die Daten des aktuellen Einsatzes auf die Werte der eingaben gesezt werden, 
             * müsste mann alle Werte auf die Werte bevor der änderung zurücksetzen wenn ein Wert nicht stimmt.
             * Mit einem testEinsatz aber ist es egal wie die Werte sind da er einfach gelöscht werden kann.
             */
            Einsatz testEinsatz = new Einsatz();
            // testEinsatz wird eine Kopie des aktuellen Einsatzes ohne dabei nur die Referenz zu kopieren.
            setzeEinsatz(testEinsatz);
            // Nun wird für jeden Einsatz ausser den aktuellen überprüft, ob eine Zeitüberschneidung besteht.
            bool keineUeberschneidung = true;
            Mitarbeiter mitarbeiter = Bibliothek.Mitarbeiter_nach_ID(this.mitarbeiter.SelectedIndex + 1);
            foreach (Einsatz e in Bibliothek.Einsatz_Alle())
            {
                if (testEinsatz.ID != e.ID)
                {
                    if (e.Mitarbeiter.ID == mitarbeiter.ID)
                    {
                        if (e.Start.Date == testEinsatz.Start.Date)
                        {
                            /*
                             * Falls der ausgelesene Einsatz nicht dem aktuellen Einsatz entspricht,
                             * der Mitarbeiter gleich ist wie der der in der ComboBox ausgewählt wurde und
                             * der Einsatz am gleichen Tag wie der aktuelle Einsatz statt findet,
                             * wird berprüft das Sie sich nicht überschreiben.
                             */
                            if (
                                // Das Startdatum ist gleich wie das Enddatum
                                (testEinsatz.Start != testEinsatz.Ende) ||
                                // Das Start oder Enddatum liegt auf dem zu überprüfenden Datum
                                (testEinsatz.Ende > testEinsatz.Start) ||
                                (testEinsatz.Start == e.Start || testEinsatz.Ende == e.Ende) || 
                                // Das Start und Enddatum liegt zwischen den zu überprüfenden Daten
                                (testEinsatz.Start > e.Start && testEinsatz.Ende < e.Ende) || 
                                // Das Startdatum liegt zwischen den zu überprüfenden Daten
                                (testEinsatz.Start > e.Start && testEinsatz.Start < e.Ende) ||
                                // Das Enddatum liegt zwischen den zu überprüfenden Daten
                                (testEinsatz.Ende > e.Start && testEinsatz.Ende < e.Ende) || 
                                // Das Start und das Enddatum  liegen ausserhalb der zu überprüfenden Daten.
                                (testEinsatz.Start<e.Start && testEinsatz.Ende>e.Ende) ||
                                // Das Start oder Enddatum liegt ausserhalb des Projekt Zeitraumes.
                                (testEinsatz.Start < testEinsatz.Projekt.StartDatum || testEinsatz.Ende > testEinsatz.Projekt.EndDatum)
                                )
                            {
                                // Wenn sie sich überschneiden wird ein entsprechender Fehler ausgegeben.
                                MessageBox.Show(Fehler_Zeit_ueberschneidung);
                                keineUeberschneidung = false;
                            }
                        }
                    }
                }
            }
            return keineUeberschneidung;
        }

        /// <summary>
        /// Diese Methode überprüft ob richtige Zeitwerte einegegeben wurden.
        /// </summary>
        /// <returns>Gibt True bei keinen Fehler und False bei Fehler zurück</returns>
        private bool ueberpruefeEingaben()
        {
            try
            {
                // Wenn die Eingegeben Werte in ein Datum umgewandelt werden kann, sind es richtige Werte.
                setzeDatum(new Einsatz());
            }
            catch (Exception)
            {
                // Falls falsche Werte einegegeben wurden, wird ein dementsprechender Fehler zurückgegeben.
                MessageBox.Show(Fehler_Falsches_Zeit_Format);
                return false;
            }
//          Noch die Farbe überprüfen aber weiss noch nicht wie ich das mache
            return true;
        }

        /// <summary>
        /// Diese Methode nimmt die Werte die der Benutzer einegegeben hat und speichert sie in einen Einsatz,
        /// </summary>
        /// <param name="e">Einsatz der mit den Werten vom Benutzer gefüllt werden soll</param>
        private void setzeEinsatz(Einsatz e)
        {
            e.ID = einsatz.ID;
            e.Mitarbeiter = Bibliothek.Mitarbeiter_nach_ID(this.mitarbeiter.SelectedIndex + 1);
            e.Projekt = Bibliothek.Projekt_nach_ID(this.projekt.SelectedIndex + 1);
            e.Farbe = (farbe.Background as SolidColorBrush).Color;
            setzeDatum(e);
        }

        /// <summary>
        /// Diese Methode nimmt die Werte die der Benutzer als Zeiten eingegeben hat und rechnet sie so um,
        /// das sie in einen Einsatz gespeichert werden können (e).
        /// </summary>
        /// <param name="e">Einsatz für den das Eingegeben Datum gesezt werden soll.</param>
        private void setzeDatum(Einsatz e)
        {
            // Hier wird ein String vom Format 2016-04-13T10:27 erstellt da dieses Format benötigt wird um ein DateTime Objekt daraus zu generieren
            // dass in dem Einsatz (e) gespeichert werden kann. Hier wird Start und Endzeit gesezt.
            String startDatumString = datum.SelectedDate.Value.Year.ToString() + "-" + datum.SelectedDate.Value.Month.ToString() +
                                          "-" + datum.SelectedDate.Value.Day.ToString() + "T" + start_zeit.Text + ":00";
            e.Start = DateTime.Parse(startDatumString);
            String endDatumString = datum.SelectedDate.Value.Year.ToString() + "-" + datum.SelectedDate.Value.Month.ToString() +
                                    "-" + datum.SelectedDate.Value.Day.ToString() + "T" + end_zeit.Text + ":00";
            e.Ende = DateTime.Parse(endDatumString);
        }

        /// <summary>
        /// Wenn auf den Knopf Speicher geklickt wird, werden die Benutzereingaben überprüft
        /// sowie dass keine Zeitüberschneidungen bestehen. Falls beides in Ordnung ist, wird der Einsatz erstellt oder geändert
        /// und das Fenster wird geschlossen.
        /// </summary>
        /// <param name="sender">Button Objekt</param>
        /// <param name="e">Event Parameter</param>
        private void speichern_Click(object sender, RoutedEventArgs e)
        {
            if (ueberpruefeEingaben() && ueberpruefeZeit())
            {
                setzeEinsatz(this.einsatz);
                if (Neu) Bibliothek.EinsatzNeu(this.einsatz);
                this.Close();
            }
        }

        /// <summary>
        /// Wenn auf das Farbfeld geklickt wird, wird ein Dialog aufgerufen in dem der Benutzer eine Farbe für den Einsatz auswählen kann.
        /// </summary>
        /// <param name="sender">Button Objekt</param>
        /// <param name="e">Event Parameter</param>
        private void farbe_Click(object sender, RoutedEventArgs e)
        {
            // Ein Instanz des Objektes Farben wird erstellt und angezeigt.
            Farben farbenAuswahl = new Farben();
            farbenAuswahl.Show();
            // Das EinzelAnsicht Febster wird gesperrt solange der Farbdialog offen ist.
            Window.GetWindow(this).IsEnabled = false;
            // Es wird ein EventHandler gesezt, der kurz bevor das Fenster geschlossen wird, die Farbe liest die der Benutzer angeklickt hat.
            farbenAuswahl.Closed += new EventHandler((o, args) =>
            {
                // Nach dem bei dem Farbdialog auf eine Farbe geklickt wurde, wird das EinzelAnsicht Fenster wieder entsperrt und die angeklickte Farbe wird ausgelesen
                // und als neue Hintergrundfarbe des Farbenauswahlfeldes gesezt.
                Window.GetWindow(this).IsEnabled = true;
                farbe.Background = new SolidColorBrush(farbenAuswahl.getFarbe);
            });
        }
    }
}