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
    public partial class paginaPortamonete : ContentPage
    {
        string strMonete, strCarta, strTarga, dataPortaMonete, strNote;
        string strMoneteIeri, strCartaIeri, strTargaIeri, dataPortaMoneteIeri, strNoteIeri;
        string strDataOdierna, strDataIeri;
        string strChilometri, strRifornimento;
        string strChilometriIeri, strRifornimentoIeri;

        struct tEsitoLetturaD
        {
            public string d;
        }

        struct tParametriOpPlus
        {
            public string email;
            public string dataIni;
            public string dataFin;
            public Single monete;
            public Single carta;
            public string targa;
            public int km;
            public Single rifornimento;
            public string note;

        }
        public paginaPortamonete()
        {
            InitializeComponent();
            caricaPortaMonete();
        }
        struct tRecEsito
        {
            public Boolean esito;
            public string messaggio;
        }


        HttpClient _client;
        cCostanti costanti = new cCostanti();
        tRecEsito esito;
        private void caricaPortaMonete()
        {
            dataPortaMonete = Preferences.Get("dataPortaMonete", "");
            strMonete = Preferences.Get("Monete", "");
            strCarta= Preferences.Get("Carta", "");
            strTarga= Preferences.Get("Targa", "");
            strChilometri = Preferences.Get("Chilometri", "");
            strRifornimento = Preferences.Get("Rifornimento", "");
            strNote= Preferences.Get("Note", "");

            dataPortaMoneteIeri = Preferences.Get("dataPortaMoneteIeri", "");
            strMoneteIeri = Preferences.Get("MoneteIeri", "");
            strCartaIeri = Preferences.Get("CartaIeri", "");
            strTargaIeri = Preferences.Get("TargaIeri", "");
            strChilometriIeri = Preferences.Get("ChilometriIeri", "");
            strRifornimentoIeri = Preferences.Get("RifornimentoIeri", "");
            strNoteIeri = Preferences.Get("NoteIeri", "");

            strDataOdierna = DateTime.Now.ToString("yyyy-MM-dd");
            strDataIeri = DateTime.Now.AddDays(-1).ToString("yyyy-MM-dd");

            if (dataPortaMonete==strDataIeri)
            {
                dataPortaMoneteIeri = strDataIeri;
                strMoneteIeri = strMonete;
                strCartaIeri = strCarta;
                strTargaIeri = strTarga;
                strChilometriIeri = strChilometri;
                strRifornimentoIeri = strRifornimento;
                strNoteIeri = strNote;
                Preferences.Set("MoneteIeri", strMoneteIeri);
                Preferences.Set("CartaIeri", strCartaIeri);
                Preferences.Set("TargaIeri", strTargaIeri);
                Preferences.Set("ChilometriIeri", strChilometriIeri);
                Preferences.Set("RifornimentoIeri", strRifornimentoIeri);
                Preferences.Set("NoteIeri", strNoteIeri);
                Preferences.Set("dataPortaMoneteIeri", strDataIeri);
            }
            if (dataPortaMoneteIeri != strDataIeri)
            {
                dataPortaMoneteIeri = strDataIeri;
                strMoneteIeri = "";
                strCartaIeri = "";
                strTargaIeri = "";
                strChilometriIeri = "";
                strRifornimentoIeri = "";
                strNoteIeri = "";
                Preferences.Set("MoneteIeri", strMoneteIeri);
                Preferences.Set("CartaIeri", strCartaIeri);
                Preferences.Set("TargaIeri", strTargaIeri);
                Preferences.Set("ChilometriIeri", strChilometriIeri);
                Preferences.Set("RifornimentoIeri", strRifornimentoIeri);
                Preferences.Set("NoteIeri", strNoteIeri);
                Preferences.Set("dataPortaMoneteIeri", strDataIeri);
            }

            if (strDataOdierna!=dataPortaMonete)
            {
                strMonete = "";
                strCarta = "";
                strChilometri = "";
                strRifornimento = "";
                strNote = "";
                dataPortaMonete = strDataOdierna;
                Preferences.Set("Monete", strMonete);
                Preferences.Set("Carta", strCarta);
                Preferences.Set("Targa", strTarga);
                Preferences.Set("Chilometri", strChilometri);
                Preferences.Set("Rifornimento", strRifornimento);
                Preferences.Set("Note", strNote);
                Preferences.Set("dataPortaMonete", strDataOdierna);
            }

            txtMonete.Text = strMonete;
            txtCarta.Text = strCarta;
            txtTarga.Text = strTarga;
            txtKm.Text = strChilometri;
            txtRifornimento.Text = strRifornimento;
            txtNote.Text = strNote;

        }
        private void btnSalvaPortaMonete_Click(object sender, EventArgs e)
        {
            dataPortaMonete = DateTime.Now.ToString("yyyy-MM-dd");
            txtTarga.Text = txtTarga.Text.ToUpper();
            Preferences.Set("Monete", txtMonete.Text);
            Preferences.Set("Carta", txtCarta.Text);
            Preferences.Set("Targa", txtTarga.Text);
            Preferences.Set("Chilometri", txtKm.Text);
            Preferences.Set("Rifornimento", txtRifornimento.Text);
            Preferences.Set("Note", txtNote.Text);

            Preferences.Set("dataPortaMonete", dataPortaMonete);
            lblEsitoPortamonete.Text = "Salvataggio eseguito alle " + DateTime.Now.ToString("HH:mm:ss");
            inviaPortaMonete(dataPortaMonete);
        }
        async public void inviaPortaMonete(string dataPortaMonete)
        {
            tParametriOpPlus datiOp = new tParametriOpPlus();
            tEsitoLetturaD esitoLetturaD = new tEsitoLetturaD();
            string strMsgSend;
            string tmpUser, strMonete = "0", strCarta = "0", strChilometri = "0", strRifornimento = "0";
            Single monete, carta,  rifornimento;
            int km;

            strMonete = txtMonete.Text;
            if (!Single.TryParse(strMonete, out monete))
            {
                strMonete = "0";
            }

            strCarta = txtCarta.Text;
            if (!Single.TryParse(strCarta, out carta))
            {
                strCarta = "0";
            }

            strChilometri = txtKm.Text;
            if (!int.TryParse(strChilometri, out km))
            {
                strChilometri = "0";
            }

            strRifornimento = txtRifornimento.Text;
            if (!Single.TryParse(strRifornimento, out rifornimento))
            {
                strRifornimento = "0";
            }

            tmpUser = Preferences.Get("mbUser", "");
            
            datiOp.email = tmpUser;
            datiOp.dataIni = dataPortaMonete;
            datiOp.dataFin = dataPortaMonete;
            datiOp.monete = Single.Parse(strMonete);
            datiOp.carta = Single.Parse(strCarta);
            datiOp.targa = txtTarga.Text;
            datiOp.km = int.Parse(strChilometri);
            datiOp.rifornimento = Single.Parse(strRifornimento);
            datiOp.note = txtNote.Text;

            esito.messaggio = "";
            esito.esito = false;

            _client = new HttpClient();
            strMsgSend = JsonConvert.SerializeObject(datiOp);
            var request = new HttpRequestMessage
            {
                Method = HttpMethod.Post,
                RequestUri = new Uri(costanti.uri + "avviaAggiornamentoInfoIncassi"),
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