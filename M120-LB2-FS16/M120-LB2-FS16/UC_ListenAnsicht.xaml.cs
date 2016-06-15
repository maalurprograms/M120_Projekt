using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace M120_LB2_FS16
{
    /// <summary>
    /// Interaktionslogik für UC_ListenAnsicht.xaml
    /// Die ListenAnsicht zeigt alle Einsätze mit Mitarbeiter, Projekt, Dauer, Start sowie Endzeit und ID an.
    /// </summary>
    public partial class UC_ListenAnsicht : UserControl
    {
        public UC_ListenAnsicht()
        {
            InitializeComponent();
            // Die daten für die Liste werden generiert.
            setData();
        }

        /// <summary>
        /// Diese Methode nimmt aus jedem Einsatz die Werte die sie benötigt und
        /// füllt diese in ein Data Objekt ein. Dieses wird dann zur liste dataList hinzugefügt.
        /// </summary>
        private void setData()
        {
            // In der dataList werden von allen Einsätzen inform von Data Objekten gespeichert.
            List<Data> dataList = new List<Data>();
            // Eine Liste aller EInsätze.
            List<Einsatz> einsaetze = Bibliothek.Einsatz_Alle();
            foreach (Einsatz einsatz in einsaetze)
            {
                // Für jeden Einsatz wird nun ein Data Objekt erstellt.
                // Dieses wird dann zu der Liste dataList hinzugefügt.
                // ID des Einsatzes
                int id = einsatz.ID;
                String mitarbeiter = einsatz.Mitarbeiter.Vorname + " " + einsatz.Mitarbeiter.Name;
                String projekt = einsatz.Projekt.Name;
                String dauer = (einsatz.Ende - einsatz.Start).ToString();
                String zeitspanne = einsatz.Start.ToShortTimeString() + " bis " + einsatz.Ende.ToShortTimeString();
                String datum = einsatz.Start.ToShortDateString();
                dataList.Add(new Data(id, mitarbeiter, projekt, dauer, zeitspanne, datum));
            }
            // Die Liste dataList wird als Resource für das DataGrid Objekt gesezt.
            // Dieses generiert dann AUtomatisch den Kopf und die Werte der Liste.
            listen_ansicht.ItemsSource = dataList;
        }

        /// <summary>
        /// Diese Klasse speichert die Werte der Einsätze die für die Listenansicht bennötigt werden
        /// </summary>
        private class Data
        { 
            public int EinsatzID;
            public String MitarbeiterName;
            public String ProjektName;
            public String EinsatzDauer;
            public String EinsatzZeitspanne;
            public String EinsatzDatum;


            public Data(int einsatzID, String mitarbeiterName, String projektName, String einsatzDauer, String einsatzZeitspanne, String einsatzDatum)
            {
                EinsatzID = einsatzID;
                MitarbeiterName = mitarbeiterName;
                ProjektName = projektName;
                EinsatzDauer = einsatzDauer;
                EinsatzZeitspanne = einsatzZeitspanne;
                EinsatzDatum = einsatzDatum;
            }

            #region Getters
            // Nun werden noch getters definiert damit das DataGrid auf die Daten zugreiffen kann.
            public int ID
            {
                get { return EinsatzID; }
            }

            public string Mitarbeiter
            {
                get { return MitarbeiterName; }
            }

            public string Projekt
            {
                get { return ProjektName; }
            }

            public string Dauer
            {
                get { return EinsatzDauer; }
            }

            public string Zeitspanne
            {
                get { return EinsatzZeitspanne; }
            }

            public string Datum
            {
                get { return EinsatzDatum; }
            }
            #endregion
        }

        /// <summary>
        /// Wenn auf einen Einsatz geklickt wird, wird eine EinzelAnsicht geöffnet in der der Einsatz
        /// bearbeitet werden kann.
        /// </summary>
        /// <param name="sender">DataGrid Objekt</param>
        /// <param name="e">Event Parameter</param>
        private void listen_ansicht_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            // Das Sender Objekt wird in ein DataGrid konvertiert und
            // das ausgewählte Element wird geholt.
            DataGrid grid = sender as DataGrid;
            Data einsatzData = grid.SelectedValue as Data;
            // Eine EinzelAnsicht wird erstellt und als Parameter die Id des geklickten Einsatzes.
            EinzelAnsicht einzelAnsicht = new EinzelAnsicht(einsatzData.ID);
            // Die EinzelAnsicht wird angezeigt und das MainWindow wird gesperrt.
            einzelAnsicht.Show();
            Window.GetWindow(this).IsEnabled = false;
            // Wenn das EinzelAnsicht Fenster geschlossen wird, wird das 
            // MainWindow wieder aktiviert, und ddas DataGrid wird aktualisiert.
            einzelAnsicht.Closed += new EventHandler((o, args) =>
            {
                Window.GetWindow(this).IsEnabled = true;
                setData();
            });
        }
    }
}
