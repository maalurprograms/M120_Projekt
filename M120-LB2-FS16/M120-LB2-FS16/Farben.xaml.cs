using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace M120_LB2_FS16
{
    /// <summary>
    /// Interaktionslogik für Farben.xaml
    /// Diese Klasse dient dazu den Benutzer aus einem Vordefinierten Set von Farben auswählen zu lassen
    /// </summary>
    public partial class Farben : Window
    {
        // Die Farbe die am schluss ausgewählt wurde.
        private Color Farbe;
        public Farben()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Wenn auf eine Farbe geklickt wird, wird diese in der Instanzvariabel Farbe abgespeichert und das Fenster wird geschlossen.
        /// </summary>
        /// <param name="sender">Button Objekt</param>
        /// <param name="e">Event Parameter</param>
        private void farbButtonClick(object sender, RoutedEventArgs e)
        {
            // Es wird ein Button aus dem sender Parameter erstellt und dessen Hintergrundfarbe wird gespeichert.
            Button farbe = sender as Button;
            Farbe = (farbe.Background as SolidColorBrush).Color;
            this.Close();
        }

        /// <summary>
        /// Damit auf die Farbe zugegriffen werden kann wird eine getter verwendet.
        /// </summary>
        public Color getFarbe
        {
            get { return Farbe; }
        }
    }
}
