using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using Xamarin.Essentials;
using Newtonsoft.Json;

namespace moneySmart.Pagine
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class paginaPortamonete : ContentPage
    {
        string strMonete, strCarta, strTarga, dataPortaMonete;
        string strMoneteIeri, strCartaIeri, strTargaIeri, dataPortaMoneteIeri;
        string strDataOdierna, strDataIeri;
        string strJson;

        public paginaPortamonete()
        {
            InitializeComponent();
            caricaPortaMonete();
        }

        private void caricaPortaMonete()
        {
            dataPortaMonete = Preferences.Get("dataPortaMonete", "");
            strMonete = Preferences.Get("Monete", "");
            strCarta= Preferences.Get("Carta", "");
            strTarga= Preferences.Get("Targa", "");

            strJson = Preferences.Get("Targhe", "[]");

            dataPortaMoneteIeri = Preferences.Get("dataPortaMoneteIeri", "");
            strMoneteIeri = Preferences.Get("MoneteIeri", "");
            strCartaIeri = Preferences.Get("CartaIeri", "");
            strTargaIeri = Preferences.Get("TargaIeri", "");

            strDataOdierna = DateTime.Now.ToString("dd-MM-yyyy");
            strDataIeri = DateTime.Now.AddDays(-1).ToString("dd-MM-yyyy");

            if (dataPortaMonete==strDataIeri)
            {
                strMoneteIeri = strMonete;
                strCarta = strCartaIeri;
                dataPortaMoneteIeri = strDataIeri;
                Preferences.Set("MoneteIeri", strMoneteIeri);
                Preferences.Set("CartaIeri", strCartaIeri);
                Preferences.Set("TargaIeri", strTargaIeri);

                Preferences.Set("dataPortaMoneteIeri", strDataIeri);
            }
            if (dataPortaMoneteIeri != strDataIeri)
            {
                strMoneteIeri = "";
                strCarta = "";
                dataPortaMoneteIeri = strDataIeri;
                Preferences.Set("MoneteIeri", strMoneteIeri);
                Preferences.Set("CartaIeri", strCartaIeri);
                Preferences.Set("TargaIeri", strTargaIeri);

                Preferences.Set("dataPortaMoneteIeri", strDataIeri);
            }

            if (strDataOdierna!=dataPortaMonete)
            {
                strMonete = "";
                strCarta = "";
                dataPortaMonete = strDataOdierna;
                Preferences.Set("Monete", strMonete);
                Preferences.Set("Carta", strCarta);
                Preferences.Set("Targa", strTarga);

                Preferences.Set("dataPortaMonete", strDataOdierna);
            }
            txtMonete.Text = strMonete;
            txtCarta.Text = strCarta;
            txtTarga.Text = strTarga;
        }
        private void btnSalvaPortaMonete_Click(object sender, EventArgs e)
        {
            dataPortaMonete = DateTime.Now.ToString("dd-MM-yyyy");
            txtTarga.Text = txtTarga.Text.ToUpper();
            Preferences.Set("Monete", txtMonete.Text);
            Preferences.Set("Carta", txtCarta.Text);
            Preferences.Set("Targa", txtTarga.Text);

            Preferences.Set("dataPortaMonete", dataPortaMonete);
            lblEsitoPortamonete.Text = "Salvataggio eseguito alle " + DateTime.Now.ToString("HH:mm:ss");
        }
    }
}