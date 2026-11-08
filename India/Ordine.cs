using System;

namespace India
{
    public class Ordine
    {
        public Piatto PiattoOrdinato { get; set; }
        public int Quantita { get; set; }

        public Ordine(Piatto piatto, int quantita)
        {
            PiattoOrdinato = piatto;
            Quantita = quantita;
        }

        public double CalcolaSubtotale()
        {
            return PiattoOrdinato.Prezzo * Quantita;
        }

        public override string ToString()
        {
            return PiattoOrdinato.Nome + " x" + Quantita + " - Subtotale: " + CalcolaSubtotale().ToString("F2") + " €";
        }
    }
}