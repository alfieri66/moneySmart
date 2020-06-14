using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using Xamarin.Essentials;
using System.IO;
using System.Collections.ObjectModel;
using Newtonsoft.Json;
using System.Net.Http;



namespace moneySmart.Pagine
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class paginaStore : ContentPage
    {
        struct tCassa
        {
            public long codiceLocale;
            public string emailUtente;
            public float acconto;
            public float recupero;
            public float daRiportare;
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
        struct tCheckBox
        {
            public string id { get; set; }
            public Boolean stato { get; set; }

        }

        public struct tOperazioneCassa
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
        struct tinfoLista
        {
            public int idOperazione { get; set; }
            public long codiceLocale { get; set; }
            public string nomeLocale { get; set; }
            public float acconto { get; set; }
            public float recupero { get; set; }
            public float daRiportare { get; set; }
            public DateTime dataOperazione { get; set; }

            public string valIdOperazione 
            { 
                get {
                    return $"{idOperazione}";
                    }
            }
            public string valCodiceLocale {
                get
                {
                    return $"{codiceLocale}";
                }
            }

            public string valNomeLocale { 
                get
                {
                    return $"{nomeLocale}";
                }
            }

            public string valAcconto {
                get
                {
                    return $"{acconto}";
                }
            }
            public string valRecupero { 
                get
                {
                    return $"{recupero}";
                }
            }

            public string strDaRiportare {
                get
                {
                    return $"{daRiportare}";
                }
            }

            public string valDataOperazione 
            { 
                get
                {
                    return $"{daRiportare}";
                }
            }

            public string chkEliminaOperazione
            {
                get
                {
                    string tmp = "{chkEliminaOp" + idOperazione.ToString().Trim() + "}"; 
                    return tmp;
                }
            }

            public string valInfo
            {
                get
                {
                    string tmp = "<strong>" +
                                 codiceLocale.ToString().Trim() +
                                 "</strong>" +
                                 "<br>" + 
                                 "acconto: " + String.Format("{0:0,0.00}", acconto) +
                                 "<br>" +
                                 "recupero: " + String.Format("{0:0,0.00}", recupero) +
                                 "<br>"+
                                 "da riportare: " + String.Format("{0:0,0.00}", daRiportare);
                    return tmp.Trim();
                }
            }
        }

        cGlobali globale = new cGlobali();
        cCostanti costanti = new cCostanti();
        cDatabase archivioOperazione = new cDatabase();
        List<cGlobali.tOperazione> infoListaDb = new List<cGlobali.tOperazione>();
        List<tCheckBox> statoCheckBox = new List<tCheckBox>();
        tEsitoD esitoD = new tEsitoD();

        public paginaStore()
        {
            InitializeComponent();
            if (App.loginOk == false)
            {
                System.Environment.Exit(0);
            }
            archivioOperazione.apriDB();

        }

        private void btnCreaValoriDiProva_Click(object sender, EventArgs e)
        {
            Boolean esito = false;
            string tmpUser;
            tmpUser = Preferences.Get("mbUser", "");
            esito = archivioOperazione.aggiungOperazione(100100101, tmpUser, 51, 51, 51);
            esito = archivioOperazione.aggiungOperazione(100100101, tmpUser, 101, 101, 101);
            esito = archivioOperazione.aggiungOperazione(100100101, tmpUser, 151, 151, 151);
            esito = archivioOperazione.aggiungOperazione(200200201, "salvatore.alfieri.66@libero.it", 200, 200, 200);
        }

        private void btnCancellaRighe_Click(object sender, EventArgs e)
        {
            cGlobali.tOperazione operazione = new cGlobali.tOperazione();
            while (archivioOperazione.estraiTestaOperazione(ref operazione.codiceLocale, ref operazione.emailUtente, ref operazione.acconto, ref operazione.recupero, ref operazione.acconto, ref operazione.dataOperazione))
            {
            }
        }

        private void btnCreaDatabase_Click(object sender, EventArgs e)
        {
            File.Delete(costanti.dbPath);
            archivioOperazione.apriDB();
            MostraInTabella();
        }

        private void btnMostraInTabella_Click(object sender, EventArgs e)
        {
            MostraInTabella();
        }

        public void MostraInTabella()
        {
            List<tinfoLista> infoLista = new List<tinfoLista>();
            string tmpUser;
            Random dado = new Random();
            int i, n;
            tinfoLista operazione;

            tmpUser = Preferences.Get("mbUser", "");
            infoLista.Clear();
            statoCheckBox.Clear();
            infoListaDb.Clear();
            archivioOperazione.apriDB();
            archivioOperazione.estraiTutteLeOperazioni(tmpUser, infoListaDb);

            n = infoListaDb.Count();

            for (i = 0; i<n; i++)
            {
                operazione = new tinfoLista();
                operazione.idOperazione = infoListaDb[i].idOperazione;
                operazione.codiceLocale = infoListaDb[i].codiceLocale;
                operazione.acconto = infoListaDb[i].acconto;
                operazione.recupero= infoListaDb[i].recupero;
                operazione.daRiportare= infoListaDb[i].daRiportare;
                operazione.dataOperazione= infoListaDb[i].dataOperazione;
                infoLista.Add(operazione);
            }
            listaOPerazioni.ItemsSource = infoLista;
        }

        private int cercaPosizioneId(string id)
        {
            int i=0, p = -1;
            tCheckBox stato = new tCheckBox();
            while (i< statoCheckBox.Count && p<0)
            {
                if (statoCheckBox[i].id == id)
                    p = i;
                i++;
            }
            if (p==-1)
            {
                stato.id = id;
                stato.stato = false;
                p = statoCheckBox.Count;
                statoCheckBox.Add(stato);
            }
            return p;
        }

        private void chkEliminaOp_Changed(object sender, CheckedChangedEventArgs e)
        {
            CheckBox chkEliminaOp = (CheckBox)sender;
            tCheckBox tmpCheckBoxx = new tCheckBox();
            int p;
            
            p=cercaPosizioneId(chkEliminaOp.Id.ToString());
            tmpCheckBoxx = statoCheckBox[p];
            if (chkEliminaOp.IsChecked)
            {
                tmpCheckBoxx.stato = true;
                statoCheckBox[p]= tmpCheckBoxx;
                lblRisultato.Text = tmpCheckBoxx.stato.ToString();
              
            }
            else
            {
                tmpCheckBoxx.stato = false;
                statoCheckBox[p] = tmpCheckBoxx;
                lblRisultato.Text = tmpCheckBoxx.stato.ToString();
            }
        }

        private void btnCancellaDati_Click(object sender, EventArgs e)
        {
            int n, id, i = 0;
            n = infoListaDb.Count();

            for (i = 0; i < n; i++)
            {

                if (statoCheckBox[i].stato)
                {
                   id= infoListaDb[i].idOperazione;
                   archivioOperazione.eliminaOperazione(id);
                }
            }
            MostraInTabella();

        }

        private void btnRegistraDati_Click(object sender, EventArgs e)
        {

            int n, id, i = 0;
            string tmpUser;
            tmpUser = Preferences.Get("mbUser", "");
            n = infoListaDb.Count();

            for (i = 0; i < n; i++)
            {

                if (statoCheckBox[i].stato)
                {
                    id = infoListaDb[i].idOperazione;
                    verificaOperazioneCassa(id, infoListaDb[i].codiceLocale, tmpUser, infoListaDb[i].acconto, infoListaDb[i].recupero, infoListaDb[i].daRiportare);
                }
            }
            
        }

        async public void eseguiOperazioneCassa(long codiceLocale, string emailUtente, float acconto, float recupero, float daRiportare)
        {
            tOperazioneCassa operazioneCassa = new tOperazioneCassa();
            string stringaJSON;
            HttpClient _client;

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
                    MostraInTabella();
                    /*
                    string content = await response.Content.ReadAsStringAsync();
                    esitoD = JsonConvert.DeserializeObject<tEsitoD>(content);
                    esitoCassa = JsonConvert.DeserializeObject<tEsistoCassa>(esitoD.d);

                    esitoCassa.messaggio = esitoCassa.messaggio.Replace("<br>", "\n");
                    await DisplayAlert("Operazione eseguita!", esitoCassa.messaggio, "OK");
                    */
                }
                else
                {
                    await DisplayAlert("Attenzione!", "Operazione NON eseguita!", "OK");
                }
            }
            catch 
            {
            }
        }



        async public void verificaOperazioneCassa(int id, long codiceLocale, string emailUtente, float acconto, float recupero, float daRiportare)
        {
            tCassa infoCassa = new tCassa();
            TLocali locale = new TLocali();
            string stringaJSON;
            HttpClient _client;
            string messaggio = "";

            infoCassa.emailUtente = Preferences.Get("mbUser", "");
            infoCassa.codiceLocale = codiceLocale;
            infoCassa.acconto = acconto;
            infoCassa.recupero = recupero;
            infoCassa.daRiportare = daRiportare;

            if (Connectivity.NetworkAccess == NetworkAccess.Internet)
            {
                App.ConnessioneOk = true;
            }
            else
            {
                App.ConnessioneOk = false;
            }

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
                            bool risposta = await DisplayAlert("Verifica i seguenti dati:", messaggio, "Conferma", "Annulla");
                            if (risposta)
                            {
                                eseguiOperazioneCassa(locale.codiceLocale, infoCassa.emailUtente, acconto, recupero, daRiportare);
                                archivioOperazione.eliminaOperazione(id);
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
                await DisplayAlert("Attenzione!", "Connessione non presente ", "Ok");
            }
        }
    }
}