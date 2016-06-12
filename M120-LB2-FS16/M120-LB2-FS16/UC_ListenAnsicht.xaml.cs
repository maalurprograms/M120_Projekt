using System;
using System.Collections.Generic;
using System.IO.Packaging;
using System.Linq;
using System.Media;
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
    /// Interaktionslogik für UC_ListenAnsicht.xaml
    /// </summary>
    public partial class UC_ListenAnsicht : UserControl
    {
        public UC_ListenAnsicht()
        {
            InitializeComponent();
            setData();
        }

        private void setData()
        {
            List<Einsatz> einsaetze = Bibliothek.Einsatz_Alle();
            List<Data> dataList = new List<Data>();
            foreach (Einsatz einsatz in einsaetze)
            {
                int id = einsatz.ID;
                String mitarbeiter = einsatz.Mitarbeiter.Vorname + " " + einsatz.Mitarbeiter.Name;
                String projekt = einsatz.Projekt.Name;
                String dauer = (einsatz.Ende - einsatz.Start).ToString();
                String zeitspanne = einsatz.Start.ToShortTimeString() + " bis " + einsatz.Ende.ToShortTimeString();
                String datum = einsatz.Start.ToShortDateString();
                dataList.Add(new Data(id, mitarbeiter, projekt, dauer, zeitspanne, datum));
            }
            listen_ansicht.ItemsSource = dataList;
        }

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

            public int ID
            {
                get { return EinsatzID; }
            }

            public string Mitarbeiter
            {
                get { return MitarbeiterName; }
            }

            public string Projekt1
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
        }

        private void listen_ansicht_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            DataGrid grid = sender as DataGrid;
            Data einsatzData = grid.SelectedValue as Data;
            EinzelAnsicht einzelAnsicht = new EinzelAnsicht(einsatzData.ID);
            einzelAnsicht.Show();
        }
    }
}
