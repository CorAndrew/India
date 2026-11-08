using System;

namespace India
{
    public class Piatto
    {
        public string Nome { get; set; }
        public string Categoria { get; set; }
        public double Prezzo { get; set; }

        public Piatto(string nome, string categoria, double prezzo)
        {
            Nome = nome;
            Categoria = categoria;
            Prezzo = prezzo;
        }

        public override string ToString()
        {
            return Nome + " (" + Categoria + ") - " + Prezzo.ToString("F2") + " €";
        }
    }
}