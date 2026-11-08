using Avalonia.Controls;
using Avalonia.Interactivity;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;

namespace India
{
    public partial class MainWindow : Window
    {
        private List<Tavolo> ListaRistoranteTavoli;
        private List<Piatto> Menu;
        private const string NomeFileJson = "ristorante_dati.json";

        public MainWindow()
        {
            InitializeComponent();

            ListaRistoranteTavoli = new List<Tavolo>();
            Menu = new List<Piatto>();


            // Caricare i dati da file JSON se esiste, altrimenti inizializzare i tavoli di esempio
            CaricaDatiDaJson();

            AggiornaGrafica();
        }

        // Salvare lo stato attuale della lista dei tavoli su file JSON
        private void SalvaDatiSuJson()
        {
            try
            {
                // Configurare la formattazione JSON in modo leggibile
                JsonSerializerOptions opzioni = new JsonSerializerOptions { WriteIndented = true };
                string jsonString = JsonSerializer.Serialize(ListaRistoranteTavoli, opzioni);
                File.WriteAllText(NomeFileJson, jsonString);
            }
            catch (Exception ex)
            {
                TxtErrori.Text = "Errore durante il salvataggio dei dati.";
            }
        }

        // Caricare i dati dei tavoli dal file JSON all'avvio dell'applicazione
        private void CaricaDatiDaJson()
        {
            if (File.Exists(NomeFileJson))
            {
                try
                {
                    string jsonString = File.ReadAllText(NomeFileJson);
                    List<Tavolo> datiCaricati = JsonSerializer.Deserialize<List<Tavolo>>(jsonString);
                    if (datiCaricati != null)
                    {
                        ListaRistoranteTavoli = datiCaricati;
                        return;
                    }
                }
                catch (Exception ex)
                {
                    TxtErrori.Text = "Errore durante il salvataggio dei dati: " + ex.Message;
                }
            }
        }

        public void BtnPrenota_Click(object sender, RoutedEventArgs e)
        {
            TxtErrori.Text = "";

            int numeroTavolo;
            if (!int.TryParse(TxtTavolo.Text, out numeroTavolo) || numeroTavolo <= 0)
            {
                TxtErrori.Text = "Inserire un numero di tavolo valido.";
                return;
            }

            Tavolo tavoloTrovato = null;
            for (int i = 0; i < ListaRistoranteTavoli.Count; i++)
            {
                if (ListaRistoranteTavoli[i].Numero == numeroTavolo)
                {
                    tavoloTrovato = ListaRistoranteTavoli[i];
                    break;
                }
            }

            if (tavoloTrovato == null)
            {
                tavoloTrovato = new Tavolo(numeroTavolo, 4);
                ListaRistoranteTavoli.Add(tavoloTrovato);
            }

            tavoloTrovato.IsOccupato = true;

            ComboBoxItem itemSelezionato = CmbCibo.SelectedItem as ComboBoxItem;
            if (itemSelezionato == null)
            {
                return;
            }
            string nomePiattoScelto = itemSelezionato.Content.ToString();

            Piatto piattoTrovato = null;
            for (int i = 0; i < Menu.Count; i++)
            {
                if (Menu[i].Nome == nomePiattoScelto)
                {
                    piattoTrovato = Menu[i];
                    break;
                }
            }

            int quantita = (int)NumPiatti.Value;

            if (piattoTrovato != null)
            {
                Ordine nuovoOrdine = new Ordine(piattoTrovato, quantita);
                tavoloTrovato.Ordini.Add(nuovoOrdine);
            }

            // Salvare su JSON dopo ogni modifica importante
            SalvaDatiSuJson();

            AggiornaGrafica();
            TxtTavolo.Text = "";
        }

        public void BtnLibera_Click(object sender, RoutedEventArgs e)
        {
            Tavolo tavoloSelezionato = ListaTavoli.SelectedItem as Tavolo;
            if (tavoloSelezionato == null)
            {
                TxtErrori.Text = "Seleziona un tavolo dalla lista da liberare.";
                return;
            }

            tavoloSelezionato.IsOccupato = false;
            tavoloSelezionato.Ordini.Clear();

            SalvaDatiSuJson();

            AggiornaGrafica();
            TxtErrori.Text = "Tavolo " + tavoloSelezionato.Numero + " liberato con successo.";
        }

        private void AggiornaGrafica()
        {
            ListaTavoli.ItemsSource = null;
            ListaTavoli.ItemsSource = ListaRistoranteTavoli;

            int liberi = 0;
            int occupati = 0;

            for (int i = 0; i < ListaRistoranteTavoli.Count; i++)
            {
                if (ListaRistoranteTavoli[i].IsOccupato)
                {
                    occupati++;
                }
                else
                {
                    liberi++;
                }
            }

            if (TxtStatistiche != null)
            {
                TxtStatistiche.Text = "Tavoli liberi: " + liberi + " | Tavoli occupati: " + occupati;
            }

            string dettaglioTavoli = "";
            for (int i = 0; i < ListaRistoranteTavoli.Count; i++)
            {
                Tavolo t = ListaRistoranteTavoli[i];
                if (t.IsOccupato)
                {
                    dettaglioTavoli = dettaglioTavoli + "Tavolo " + t.Numero + " (Totale: " + t.CalcolaTotale().ToString("F2") + " €):\n";
                    for (int j = 0; j < t.Ordini.Count; j++)
                    {
                        dettaglioTavoli = dettaglioTavoli + " - " + t.Ordini[j].ToString() + "\n";
                    }
                    dettaglioTavoli = dettaglioTavoli + "\n";
                }
            }
        }
    }
}