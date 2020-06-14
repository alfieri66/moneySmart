using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using Newtonsoft.Json;
using Xamarin.Essentials;
using System.Net.Http;

namespace moneySmart.Pagine
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class paginaCassa : ContentPage
    {
        struct tCassa
        {
            public long codiceLocale;
            public string emailUtente;
            public float acconto;
            public float recupero;
            public float daRiportare;
        }
        struct tEsitoD
        {
            public string d;
        }

        struct tEsistoCassa
        {
            public Boolean esito;
            public string messaggio;
        }

        public struct TLocali
        {
            public string stato;
            public int idLocale;
            public string nome;
            public string citta;
            public string indirizzo;
            public string tel;
            public Int64 codiceLocale;
        }

        public struct tOperazioneCassa
        {
            public long codiceLocale;
            public string emailUtente;
            public float acconto;
            public float recupero;
            public float daRiportare;
        }


        cCostanti costanti = new cCostanti();
        tEsitoD esitoD = new tEsitoD();
        //tEsistoCassa esitoCassa = new tEsistoCassa();
        cDatabase archivio = new cDatabase();

        public paginaCassa()
        {
            InitializeComponent();

        }

        async public void eseguiOperazioneCassa(long codiceLocale, string emailUtente, float acconto, float recupero, float daRiportare)
        {
            tOperazioneCassa operazioneCassa = new tOperazioneCassa();
            string stringaJSON;
            HttpClient _client;

            tEsistoCassa esitoCassa = new tEsistoCassa();

            operazioneCassa.codiceLocale = codiceLocale;
            operazioneCassa.emailUtente = emailUtente;
            operazioneCassa.acconto = acconto;
            operazioneCassa.recupero = recupero;
            operazioneCassa.daRiportare = daRiportare;

            stringaJSON = JsonConvert.SerializeObject(operazioneCassa);
            _client = new HttpClient();

            var request = new HttpRequestMessage
            {
                Method = HttpMethod.Post,
                RequestUri = new Uri(costanti.uri + "eseguiOperazioneCassa"),
                Content = new StringContent(stringaJSON, Encoding.UTF8, "application/json"),
            };
            try
            {
                HttpResponseMessage response = await _client.SendAsync(request);
                if (response.IsSuccessStatusCode)
                {
                    string content = await response.Content.ReadAsStringAsync();
                    esitoD = JsonConvert.DeserializeObject<tEsitoD>(content);
                    esitoCassa = JsonConvert.DeserializeObject<tEsistoCassa>(esitoD.d);

                    esitoCassa.messaggio = esitoCassa.messaggio.Replace("<br>", "\n");
                    await DisplayAlert("Operazione eseguita!", esitoCassa.messaggio, "OK");
                    
                }
            }
            catch 
            {
            }


        }


        async private void btnInviaCassa_Click(object sender, EventArgs e)
        {
            tCassa infoCassa = new tCassa();
            TLocali locale = new TLocali();
            string stringaJSON;
            long codiceLocale;
            HttpClient _client;
            string messaggio="";
            

            //int decAcconto, decRecupero, decDaRiportare;
            //int intAcconto, intRecupero, intDaRiportare;
            Boolean boolAcconto, boolRecupero, boolDaRiportare, boolCodiceLocale;
            float acconto, recupero, daRiportare;
            boolAcconto= float.TryParse(txtAcconto.Text, out acconto);
            boolRecupero = float.TryParse(txtRecupero.Text, out recupero);
            boolDaRiportare = float.TryParse(txtDaRiportare.Text, out daRiportare);
            if (!boolAcconto) {acconto = 0;}
            if (!boolRecupero) {recupero = 0;}
            if (!boolDaRiportare) {daRiportare = 0;}
            //boolAcconto = int.TryParse((txtAcconto00.Text+"000").Substring(0,2), out decAcconto);
            //boolRecupero = int.TryParse((txtRecupero00.Text + "000").Substring(0, 2), out decRecupero);
            //boolDaRiportare = int.TryParse((txtDaRiportare00.Text + "000").Substring(0, 2), out decDaRiportare);
            //if (!boolAcconto) { decAcconto = 0; }
            //if (!boolRecupero) { decRecupero = 0; }
            //if (!boolDaRiportare) { decDaRiportare = 0; }
            //acconto = (float) (intAcconto + decAcconto / 100);
            //recupero = (float) (intRecupero + decRecupero / 100);
            //daRiportare = (float) (intDaRiportare + decDaRiportare / 100);

            if (Connectivity.NetworkAccess == NetworkAccess.Internet)
            {
                App.ConnessioneOk = true;
            }
            else
            {
                App.ConnessioneOk = false;
            }

            boolCodiceLocale = long.TryParse(txtCodLocale.Text, out codiceLocale);

            if (boolCodiceLocale)
            {
                infoCassa.emailUtente = Preferences.Get("mbUser", "");
                infoCassa.codiceLocale = codiceLocale;
                infoCassa.acconto = acconto;
                infoCassa.recupero = recupero;
                infoCassa.daRiportare = daRiportare;
                if (App.ConnessioneOk == true)
                {


                    stringaJSON = JsonConvert.SerializeObject(infoCassa);
                    _client = new HttpClient();

                    var request = new HttpRequestMessage
                    {
                        Method = HttpMethod.Post,
                        RequestUri = new Uri(costanti.uri + "verificaOperazioneCassa"),
                        Content = new StringContent(stringaJSON, Encoding.UTF8, "application/json"),
                    };

                    try
                    {
                        HttpResponseMessage response = await _client.SendAsync(request);
                        if (response.IsSuccessStatusCode)
                        {
                            string content = await response.Content.ReadAsStringAsync();
                            esitoD = JsonConvert.DeserializeObject<tEsitoD>(content);
                            locale = JsonConvert.DeserializeObject<TLocali>(esitoD.d);

                            if (locale.nome.Trim() == "")
                            {
                                await DisplayAlert("Attenzione", "Dati non corretti", "Ok");
                            }
                            else
                            {
                                messaggio = "nome: " + locale.nome + "\n" +
                                           "indirizzo:   " + locale.indirizzo + "\n" +
                                           "città:   " + locale.citta + "\n" +
                                           "codice:   " + codiceLocale.ToString() + "\n\n" +
                                           "acconto: € " + String.Format("{0:0,0.00}", acconto) + "\n" +
                                           "recupero: € " + String.Format("{0:0,0.00}", recupero) + "\n" +
                                           "da riportare: € " + String.Format("{0:0,0.00}", daRiportare) + "\n";
                                bool risposta = await DisplayAlert("Verifica i seguenti dati:", messaggio, "Confermi?", "Annulla");
                                if (risposta)
                                {
                                    eseguiOperazioneCassa(locale.codiceLocale, infoCassa.emailUtente, acconto, recupero, daRiportare);
                                    txtCodLocale.Text = "";
                                    txtAcconto.Text = "";
                                    txtRecupero.Text = "";
                                    txtDaRiportare.Text = "";
                                }
                            }
                        }
                    }
                    catch 
                    {
                    }
                }
                else
                {
                    bool risposta = await DisplayAlert("Attenzione!", "Connessione non presente. Vuoi registrare in locale?", "Si?", "No");
                    if (risposta)
                    {
                        archivio.apriDB();
                        archivio.aggiungOperazione(infoCassa.codiceLocale, infoCassa.emailUtente, infoCassa.acconto, infoCassa.recupero, infoCassa.daRiportare);
                        txtCodLocale.Text = "";
                        txtAcconto.Text = "";
                        txtRecupero.Text = "";
                        txtDaRiportare.Text = "";
                    }
                }
            }
        }
    }
}