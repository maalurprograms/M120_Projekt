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

            foreach (Einsatz einsatz in Bibliothek.Einsatz_Alle())
            {
                
            }
            listen_ansicht.ItemsSource = Bibliothek.Einsatz_Alle();
        }

        private class Data
        {
            public Data(String Mitarbeiter, String Projekt)
            {
                
            }
        }
    }
}
