using System;
using System.Windows;
using System.Windows.Media;

namespace M120_LB2_FS16
{
    /// <summary>
    /// Interaktionslogik für MainWindow.xaml
    /// Das Hauptfenster des Programmes. Hier werden die verschiedenen Ansichten dagestellt.
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            // Die vordefinierten Daten werden generiert.
            datenBereitstellen();
            InitializeComponent();
            // Wenn das MainWindow geschlossen wird soll das ganze Programm geschlossen werden.
            App.Current.ShutdownMode = ShutdownMode.OnMainWindowClose;
            // Als Standart Ansicht wird die ListenAnsicht gesezt.
            UCView.Content = new UC_ListenAnsicht();
        }

        /// <summary>
        /// Wenn auf den Knopf Einzel Ansicht geklickt wird,
        /// wird ein neues Objekt vom Typ UC_EinzelAnsicht erzeugt und angezeigt.
        /// Dies öffnet das Fenster um einen neuen Einsatz zu erstellen.
        /// </summary>
        /// <param name="sender">Button Objekt</param>
        /// <param name="e">Event Parameter</param>
        private void einzel_ansicht_neu_Click(object sender, RoutedEventArgs e)
        {
            EinzelAnsicht einzelAnsicht = new EinzelAnsicht(0);
            einzelAnsicht.Show();
            // Solange das EinzelAnsicht Fenster offen ist, soll das MainWindow gesperrt sein.
            this.IsEnabled = false;
            // Die Breite und die Höhe des MainWindow werden auf 500 und 300 gesezt.
            this.Width = this.MinWidth = this.MaxWidth = 500;
            this.Height = 300;
            // Das MainWindow darf nicht vergrössert oder verkleinert werden.
            this.ResizeMode = ResizeMode.NoResize;
            // Wenn das EinzelAnsicht Fenster geschlossen wird, wird das MainWindow wieder freigegeben und die Listen Ansicht wird angezeigt.
            einzelAnsicht.Closed += new EventHandler((o, args) =>
            {
                this.IsEnabled = true;
                UCView.Content = new UC_ListenAnsicht();
            });
        }

        /// <summary>
        /// Wenn auf den Knopf Kalender Ansicht geklickt wird, 
        /// wird ein neues Objekt vom Typ UC_KalenderAnsicht erzeugt und angezeigt.
        /// Dies wechselt von der Listen Ansicht zur Kalender Ansicht.
        /// </summary>
        /// <param name="sender">Button Objekt</param>
        /// <param name="e">Event Parameter</param>
        private void kalender_ansicht_button_Click(object sender, RoutedEventArgs e)
        {
            // Die Kalender Ansicht wird erzeugt und angezeigt.
            UCView.Content = new UC_KalenderAnsicht();
            // Die maximale und minimale Breite werden beide auf 800 gesezt.
            // Dies bewirkt das das Fenster zwar in der Höhe vergrössert oder verkleinert werden kann aber nicht in der Breite.
            this.Width = this.MinWidth = this.MaxWidth = 800;
            this.Height = 500;
            // Nun wird noch gesezt das das Fenster überhaupt geändert werden kann.
            this.ResizeMode = ResizeMode.CanResize;
        }

        /// <summary>
        /// Wenn auf den Knopf Listen Ansicht geklickt wird, 
        /// wird ein neues Objekt vom Typ UC_ListenAnsicht erzeugt und angezeigt.
        /// Dies wechselt von der Kalender Ansicht zur Listen Ansicht.
        /// </summary>
        /// <param name="sender">Button Objekt</param>
        /// <param name="e">Event Parameter</param>
        private void listen_ansicht_Click(object sender, RoutedEventArgs e)
        {
            // Die Listen Anischt wird erzeugt und angezeigt. 
            UCView.Content = new UC_ListenAnsicht();
            // Die maximale und minimale Breite werden beide auf 500 gesezt.
            // Dies bewirkt das das Fenster wieder in der normalen Grösse angezeigt wird.
            this.Width = this.MinWidth = this.MaxWidth = 500;
            this.Height = 300;
            // Es wird gesezt das das Fenster nicht mehr in der Grösse geändert werden kann
            this.ResizeMode = ResizeMode.NoResize;
        }

        #region Testdaten
        private void demoDatenMitarbeiter()
        {
            Mitarbeiter ma1 = new Mitarbeiter();
            ma1.ID = 1;
            ma1.Name = "Affolter";
            ma1.Vorname = "Anton";
            ma1.IstAktiv = true;
            ma1.Farbe = Colors.Aqua;
            Bibliothek.Mitarbeiter_Neu(ma1);

            Mitarbeiter ma2 = new Mitarbeiter();
            ma2.ID = 2;
            ma2.Name = "Bangerter";
            ma2.Vorname = "Beat";
            ma2.IstAktiv = true;
            ma2.Farbe = Colors.BlanchedAlmond;
            Bibliothek.Mitarbeiter_Neu(ma2);
        }
        private void demoDatenProjekte()
        {
            Projekt p1 = new Projekt();
            p1.ID = 1;
            p1.Name = "Projekt Zeiterfassung";
            p1.IstAktiv = true;
            p1.StartDatum = new DateTime(2016, 3, 1);
            p1.EndDatum = new DateTime(2016, 10, 1);
            p1.GesamtZeitStunden = 120;
            p1.OffeneZeitStunden = 120;
            p1.Farbe = Colors.Violet;
            Bibliothek.Projekt_Neu(p1);

            Projekt p2 = new Projekt();
            p2.ID = 2;
            p2.Name = "Projekt YellowLabel";
            p2.IstAktiv = true;
            p2.StartDatum = new DateTime(2016, 4, 2);
            p2.EndDatum = new DateTime(2016, 7, 30);
            p2.GesamtZeitStunden = 80;
            p2.OffeneZeitStunden = 80;
            p2.Farbe = Colors.Yellow;
            Bibliothek.Projekt_Neu(p2);
        }
        private void demoDatenEinsaetze()
        {
            Einsatz e1 = new Einsatz();
            e1.ID = 1;
            e1.Mitarbeiter = Bibliothek.Mitarbeiter_nach_ID(1);
            e1.Projekt = Bibliothek.Projekt_nach_ID(1);
            e1.Start = new DateTime(2016, 6, 7, 8, 0, 0);
            e1.Ende = new DateTime(2016, 6, 7, 15, 0, 0);
            e1.Farbe = Colors.LightBlue;
            Bibliothek.EinsatzNeu(e1);

            Einsatz e2 = new Einsatz();
            e2.ID = 2;
            e2.Mitarbeiter = Bibliothek.Mitarbeiter_nach_ID(1);
            e2.Projekt = Bibliothek.Projekt_nach_ID(2);
            e2.Start = new DateTime(2016, 6, 10, 11, 0, 0);
            e2.Ende = new DateTime(2016, 6, 10, 18, 0, 0);
            e2.Farbe = Colors.LightBlue;
            Bibliothek.EinsatzNeu(e2);

            Einsatz e3 = new Einsatz();
            e3.ID = 3;
            e3.Mitarbeiter = Bibliothek.Mitarbeiter_nach_ID(2);
            e3.Projekt = Bibliothek.Projekt_nach_ID(1);
            e3.Start = new DateTime(2016, 6, 14, 10, 0, 0);
            e3.Ende = new DateTime(2016, 6, 14, 14, 0, 0);
            e3.Farbe = Colors.LightBlue;
            Bibliothek.EinsatzNeu(e3);

            Einsatz e4 = new Einsatz();
            e4.ID = 4;
            e4.Mitarbeiter = Bibliothek.Mitarbeiter_nach_ID(2);
            e4.Projekt = Bibliothek.Projekt_nach_ID(1);
            e4.Start = new DateTime(2016, 6, 15, 10, 0, 0);
            e4.Ende = new DateTime(2016, 6, 15, 14, 0, 0);
            e4.Farbe = Colors.LightBlue;
            Bibliothek.EinsatzNeu(e4);
        }
        private void datenBereitstellen()
        {
            demoDatenMitarbeiter();
            demoDatenProjekte();
            demoDatenEinsaetze();
        }
        #endregion

    }
}
