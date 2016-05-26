using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace M120_LB2_FS16
{
    static class Bibliothek
    {
        private static List<Mitarbeiter> Mitarbeiter { get; set; }
        private static List<Projekt> Projekte { get; set; }
        private static List<Einsatz> Einsaetze { get; set; }

        #region Mitarbeiter
        public static void Mitarbeiter_Neu(Mitarbeiter mitarbeiter)
        {
            if (Mitarbeiter == null)
            {
                Mitarbeiter = new List<Mitarbeiter>();
            }
            Mitarbeiter.Add(mitarbeiter);
        }
        public static List<Mitarbeiter> Mitarbeiter_Alle()
        {
            return Mitarbeiter;
        }
        public static Mitarbeiter Mitarbeiter_nach_ID(Int32 id)
        {
            return (from element in Mitarbeiter where element.ID == id select element).FirstOrDefault();
        }
        #endregion
        #region Projekte
        public static void Projekt_Neu(Projekt projekt)
        {
            if (Projekte == null)
            {
                Projekte = new List<Projekt>();
            }
            Projekte.Add(projekt);
        }
        public static List<Projekt> Projekt_Alle()
        {
            return Projekte;
        }
        public static Projekt Projekt_nach_ID(Int32 id)
        {
            return (from element in Projekte where element.ID == id select element).FirstOrDefault();
        }
        #endregion
        #region Einsaetze
        public static void EinsatzNeu(Einsatz einsatz)
        {
            if (Einsaetze == null)
            {
                Einsaetze = new List<Einsatz>();
            }
            Einsaetze.Add(einsatz);
            // Stunden nachtragen
            einsatz.Projekt.OffeneZeitStunden = einsatz.Projekt.OffeneZeitStunden - (einsatz.Ende.Subtract(einsatz.Start).TotalHours);
            // Projekt nachtragen
            Projekt_nach_ID(einsatz.Projekt.ID).Einsaetze.Add(einsatz);
        }
        public static List<Einsatz> Einsatz_Alle()
        {
            return Einsaetze;
        }
        public static Einsatz Einsatz_nach_ID(Int32 id)
        {
            return (from element in Einsaetze where element.ID == id select element).FirstOrDefault();
        }
        public static List<Einsatz> Einsaetz_an_Datum(DateTime tag)
        {
            return (from element in Einsaetze where element.Start > tag.AddDays(-1) && element.Start < tag.AddDays(1) select element).ToList();
        }
        #endregion
    }
}
