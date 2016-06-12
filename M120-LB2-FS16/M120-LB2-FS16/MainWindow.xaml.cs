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
    /// Interaktionslogik für MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            datenBereitstellen();
            InitializeComponent();
            UCView.Content = new UC_ListenAnsicht();
        }

        private void button_Click(object sender, RoutedEventArgs e)
        {
            UCView.Content = new UC_ListenAnsicht();
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
            Bibliothek.EinsatzNeu(e1);

            Einsatz e2 = new Einsatz();
            e2.ID = 2;
            e2.Mitarbeiter = Bibliothek.Mitarbeiter_nach_ID(1);
            e2.Projekt = Bibliothek.Projekt_nach_ID(2);
            e2.Start = new DateTime(2016, 6, 10, 11, 0, 0);
            e2.Ende = new DateTime(2016, 6, 10, 18, 0, 0);
            Bibliothek.EinsatzNeu(e2);

            Einsatz e3 = new Einsatz();
            e3.ID = 3;
            e3.Mitarbeiter = Bibliothek.Mitarbeiter_nach_ID(2);
            e3.Projekt = Bibliothek.Projekt_nach_ID(1);
            e3.Start = new DateTime(2016, 6, 14, 10, 0, 0);
            e3.Ende = new DateTime(2016, 6, 14, 14, 0, 0);
            Bibliothek.EinsatzNeu(e3);

            Einsatz e4 = new Einsatz();
            e4.ID = 4;
            e4.Mitarbeiter = Bibliothek.Mitarbeiter_nach_ID(2);
            e4.Projekt = Bibliothek.Projekt_nach_ID(1);
            e4.Start = new DateTime(2016, 6, 15, 10, 0, 0);
            e4.Ende = new DateTime(2016, 6, 15, 14, 0, 0);
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
