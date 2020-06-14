using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using System.Net.Http;
using Newtonsoft.Json;
using Xamarin.Essentials;

namespace moneySmart.Pagine
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class paginaLogin : ContentPage
    {

        cCostanti costanti = new cCostanti();
        HttpClient _client;
        cCrittografia critto = new cCrittografia();
        cDatabase archivio = new cDatabase();
        struct tDatiLogin
        {
            public string ruolo;
            public string email;
            public string password;
        }
        struct tEsitoLoginD
        {
            public string d;
        }

        struct tEsistoLogin
        {
            public Boolean esito;
        }

        tDatiLogin datiLogin = new tDatiLogin();
        public paginaLogin()
        {
            bool statochkSalva = false;
            string tmpUser;
            string tmpPassword;
            InitializeComponent();

            App.loginOk = false;
            statochkSalva = Preferences.Get("mbSalvaPassword", false);
            tmpUser = Preferences.Get("mbUser", "");
            tmpPassword = Preferences.Get("mbPassword", "");
            if (statochkSalva==true)
            {
                txtUserName.Text = tmpUser;
                txtPasswd.Text = tmpPassword;
                chkSalva.IsChecked=true;
            }
            else
            {
                txtUserName.Text = "";
                txtPasswd.Text = "";
                chkSalva.IsChecked = false;
            }
        }

        private void btnLogin_Click(object sender, EventArgs e)
        {
            if (Connectivity.NetworkAccess==NetworkAccess.Internet )
            {
                loginRemoto();
            }
            else
            {
                loginLocale();
            }
        }


        async private void loginLocale()
        {
            string mbUserMd5;
            string mbPasswordMd5;
            bool risposta;
            mbUserMd5=Preferences.Get("mbUserMd5", "");
            mbPasswordMd5=Preferences.Get("mbPasswordMd5", "");

            if (mbUserMd5 == critto.creaMD5(txtUserName.Text) && mbPasswordMd5==critto.creaMD5(txtPasswd.Text))
            {
                risposta = await DisplayAlert("Attenzione!", "Connessione assente, vuoi procedere comunque?", "Si", "No");
                if (risposta)
                {
                    App.loginOk = true;
                    App.ConnessioneOk = false;
                    await Navigation.PopModalAsync();
                }
                else
                {
                    System.Environment.Exit(0);
                }
            }
        }


        async void loginRemoto()
        {
            bool procedi = false;
            string strMsgSend;
            string messaggio = "";
            tEsitoLoginD esitoLoginD = new tEsitoLoginD();
            tEsistoLogin esitoLogin = new tEsistoLogin();
            _client = new HttpClient();
            datiLogin.email = txtUserName.Text;
            datiLogin.password = txtPasswd.Text;
            datiLogin.ruolo = "user";

            strMsgSend = JsonConvert.SerializeObject(datiLogin);

            var request = new HttpRequestMessage
            {
                Method = HttpMethod.Post,
                RequestUri = new Uri(costanti.uri + "esisteUtente"),
                Content = new StringContent(strMsgSend, Encoding.UTF8, "application/json"),
            };

            try
            {
                HttpResponseMessage response = await _client.SendAsync(request);
                if (response.IsSuccessStatusCode)
                {
                    string content = await response.Content.ReadAsStringAsync();
                    esitoLoginD = JsonConvert.DeserializeObject<tEsitoLoginD>(content);
                    esitoLogin = JsonConvert.DeserializeObject<tEsistoLogin>(esitoLoginD.d);
                    if (esitoLogin.esito == true)
                    {
                        if (chkSalva.IsChecked)
                        {
                            Preferences.Set("mbUser", txtUserName.Text);
                            Preferences.Set("mbPassword", txtPasswd.Text);
                            Preferences.Set("mbSalvaPassword", true);
                        }
                        else
                        {
                            Preferences.Set("mbUser", "");
                            Preferences.Set("mbPassword", "");
                            Preferences.Set("mbSalvaPassword", false);
                        }
                        Preferences.Set("mbUserMd5", critto.creaMD5(txtUserName.Text));
                        Preferences.Set("mbPasswordMd5", critto.creaMD5(txtPasswd.Text));

                        procedi = true;
                    }
                    else
                    {
                        messaggio = "Utente o password errati!";
                    }
                }
            }
            catch (Exception ex)
            {
                messaggio = ex.Message;
            }
            lblRisultato.Text = messaggio;
            if (procedi)
            {
                if (archivio.contaOperazioniInSospeso(txtUserName.Text) >0)
                {
                    await DisplayAlert("Attenzione!", "Ci sono operazioni in sospeso", "Ok");
                }

                App.loginOk = true;
                App.ConnessioneOk = true;
                await Navigation.PopModalAsync();
            }
        }

    }
}