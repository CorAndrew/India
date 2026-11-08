using System;
using System.Collections.Generic;

namespace India
{
    public class Tavolo
    {
        public int Numero { get; set; }
        public int Capienza { get; set; }
        public bool IsOccupato { get; set; }
        public List<Ordine> Ordini { get; set; }

        public Tavolo(int numero, int capienza)
        {
            Numero = numero;
            Capienza = capienza;
            IsOccupato = false;
            Ordini = new List<Ordine>();
        }

        public double CalcolaTotale()
        {
            double totale = 0.0;
            for (int i = 0; i < Ordini.Count; i++)
            {
                totale = totale + Ordini[i].CalcolaSubtotale();
            }
            return totale;
        }

        public override string ToString()
        {
            string stato = IsOccupato ? "Occupato" : "Libero";
            return "Tavolo " + Numero + " (Capienza: " + Capienza + ") - Stato: " + stato;
        }
    }
}