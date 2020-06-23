using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using Xamarin.Essentials;
using System.Net.Http;
using Newtonsoft.Json;

namespace moneySmart.Pagine
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class paginaVisualizza : ContentPage
    {
        public paginaVisualizza()
        {
            InitializeComponent();
        }

        struct tEsitoLetturaD
        {
            public string d;
        }

        struct tEsitoVista        {
            public string emailUtente;
            public string codiceLocale;
            public string nomeLocale;
            public string data;
            public string acconto;
            public string recupero;
            public string daRiportare;
        }

        struct tInfoOperazione
        {
            public string codiceLocale;
            public string nomeLocale;
            public string data;
            public Single acconto;
            public Single recupero;
            public Single daRiportare;
        }

        struct tRecEsito
        {
            public Boolean esito;
            public string messaggio;
        }

        struct tParametriOp
        {
            public string email;
            public string dataIni;
            public string dataFin;
        }
        struct tParametriOpPlus
        {
            public string email;
            public string dataIni;
            public string dataFin;
            public Single monete;
            public Single carta;
        }

        struct tinfoVista
        {
            public int idOperazione { get; set; }
            public string codiceLocale { get; set; }
            public string nomeLocale { get; set; }
            public Single acconto { get; set; }
            public Single recupero { get; set; }
            public Single daRiportare { get; set; }
            public string dataOperazione { get; set; }

            public string valInfo
            {
                
                get
                {
                    string tmp;
                    tmp = "<strong> " +
                          nomeLocale +
                          "</strong>" +
                          " (" + codiceLocale + ") " +
                          "<br>" +
                          "data: " + dataOperazione +
                          "<br>";
                    if (acconto == 0)
                    {
                        tmp += "acconto: € <br>";
                    }
                    else
                    {
                        tmp += "acconto: € " + acconto + "<br>"; ;
                    }
                    if (recupero == 0)
                    {
                        tmp += "recupero da riportare: € <br>";
                    }
                    else
                    {
                        tmp += "recupero da riportare: € " + recupero + "<br>";
                    }
                    if (daRiportare == 0)
                    {
                        tmp += "da riportare: € ";
                    }
                    else
                    {
                        tmp += "da riportare: € " + daRiportare;
                    }
                    return tmp.Trim();
                }
            }
        }

        HttpClient _client;
        cCostanti costanti = new cCostanti();
        tRecEsito esito;
        private void btnVistaOperazioni_Click(object sender, EventArgs e)
        {
 
            List<tInfoOperazione> listaOperazioni = new List<tInfoOperazione>();
            DateTime dataTmp;
            string dataIni;
            string dataFin;

            string tmpUser;
            
            if (App.ConnessioneOk && App.loginOk)
            {
                tmpUser = Preferences.Get("mbUser", "");
                dataTmp = dataIniziale.Date;
                if (dataIniziale.Date==DateTime.Now.Date || dataIniziale.Date == DateTime.Now.Date.AddDays(-1))
                {
                    dataIni = dataTmp.ToString("yyyy-MM-dd");
                    dataFin = dataTmp.ToString("yyyy-MM-dd");
                    leggiOperazioni(tmpUser, dataIni, dataFin, listaOperazioni);
                }
            }
        }

        async void leggiOperazioni(string tmpUser, string  dataIni, string dataFin, List<tInfoOperazione> listaOperazioni)
        {
            string strMsgSend;
            Single totAcconto = 0, totDaRiportare = 0, totRecupero = 0;
            tParametriOp datiOp = new tParametriOp();
            tEsitoLetturaD esitoLetturaD = new tEsitoLetturaD();
            List<tinfoVista> infoVista = new List<tinfoVista>();

            int i, n;
            tinfoVista operazione;


            datiOp.email = tmpUser;
            datiOp.dataIni = dataIni;
            datiOp.dataFin = dataFin;

            strMsgSend = JsonConvert.SerializeObject(datiOp);
            infoVista.Clear();

            _client = new HttpClient();
            var request = new HttpRequestMessage
            {
                Method = HttpMethod.Post,
                RequestUri = new Uri(costanti.uri + "leggiCassaUtente"),
                Content = new StringContent(strMsgSend, Encoding.UTF8, "application/json"),
            };

            try
            {
                HttpResponseMessage response = await _client.SendAsync(request);
                if (response.IsSuccessStatusCode)
                {
                    string content = await response.Content.ReadAsStringAsync();
                    esitoLetturaD = JsonConvert.DeserializeObject<tEsitoLetturaD>(content);
                    listaOperazioni = JsonConvert.DeserializeObject<List<tInfoOperazione>>(esitoLetturaD.d);

                    n = listaOperazioni.Count();

                    for (i = 0; i < n; i++)
                    {
                        operazione = new tinfoVista();
                        operazione.idOperazione = i + 1;
                        operazione.codiceLocale = listaOperazioni[i].codiceLocale;
                        operazione.nomeLocale = listaOperazioni[i].nomeLocale;
                        operazione.dataOperazione = listaOperazioni[i].data;
                        operazione.acconto = listaOperazioni[i].acconto;
                        operazione.recupero = listaOperazioni[i].recupero;
                        operazione.daRiportare = listaOperazioni[i].daRiportare;
                        infoVista.Add(operazione);
                        totAcconto += operazione.acconto; 
                        totRecupero += operazione.recupero; 
                        totDaRiportare += operazione.daRiportare; 
                    }
                    vistaOperazioni.ItemsSource = infoVista;
                    lblAcconto.Text = String.Format("{0:0,0.00}", totAcconto);
                    lbldaRiportare.Text = String.Format("{0:0,0.00}", totDaRiportare);
                    lblRecupero.Text = String.Format("{0:0,0.00}", totRecupero);
                    if (n>0)
                    { 
                        btnPdfAndEmail.IsVisible = true; 
                    }
                    else
                    {
                        btnPdfAndEmail.IsVisible = false;
                    }
                }
            }
            catch 
            {
            }
        }

        private void btnPdfAndEmail_Click(object sender, EventArgs e)
        {
            DateTime dataTmp;
            string dataIni;
            string dataFin;
            string tmpUser;

            if (App.ConnessioneOk && App.loginOk)
            {
                tmpUser = Preferences.Get("mbUser", "");
                dataTmp = dataIniziale.Date;
                dataIni = dataTmp.ToString("yyyy-MM-dd");
                dataFin = dataTmp.ToString("yyyy-MM-dd");

                inviaPdf(tmpUser, dataIni, dataFin);
                DisplayAlert("Informazione!", "file PDF inviato via e-mail", "OK");
            }
        }

        async public void inviaPdf(string tmpUser, string dataIni, string dataFin)
        {
            string strMsgSend;
            tParametriOpPlus datiOp = new tParametriOpPlus();
            tEsitoLetturaD esitoLetturaD = new tEsitoLetturaD();
            string strMonete, strCarta, dataPortaMonete;
            string strMoneteIeri, strCartaIeri, dataPortaMoneteIeri;
            string strDataOdierna;

            dataPortaMoneteIeri = Preferences.Get("dataPortaMoneteIeri", "");
            strMoneteIeri = Preferences.Get("MoneteIeri", "0");
            strCartaIeri = Preferences.Get("CartaIeri", "0");

            dataPortaMonete = Preferences.Get("dataPortaMonete", "");
            strMonete = Preferences.Get("Monete", "0");
            strCarta = Preferences.Get("Carta", "0");
            strDataOdierna = DateTime.Now.ToString("dd-MM-yyyy");

            if (DateTime.Now.ToString("yyyy-MM-dd") != dataIni)
            {
                if (DateTime.Now.AddDays(-1).ToString("yyyy-MM-dd") == dataIni)
                {
                    strMonete = strMoneteIeri;
                    strCarta = strCartaIeri;
                }
                else
                {
                    strMonete = "0";
                    strCarta = "0";
                }
            }

            datiOp.email = tmpUser;
            datiOp.dataIni = dataIni;
            datiOp.dataFin = dataIni; 
            datiOp.monete = Single.Parse(strMonete);
            datiOp.carta = Single.Parse(strCarta);

            esito.messaggio = "";
            esito.esito = false;

            _client = new HttpClient();
            strMsgSend = JsonConvert.SerializeObject(datiOp);
            var request = new HttpRequestMessage
            {
                Method = HttpMethod.Post,
                RequestUri = new Uri(costanti.uri + "pdfCassaUtente"),
                Content = new StringContent(strMsgSend, Encoding.UTF8, "application/json"),
            };

            try
            {
                HttpResponseMessage response = await _client.SendAsync(request);
                if (response.IsSuccessStatusCode)
                {
                    string content = await response.Content.ReadAsStringAsync();
                    esitoLetturaD = JsonConvert.DeserializeObject<tEsitoLetturaD>(content);
                    esito = JsonConvert.DeserializeObject<tRecEsito>(esitoLetturaD.d);
                }
            }
            catch
            {
            }
        }
    }
}