using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using Xamarin.Essentials;

namespace moneySmart.Pagine
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class paginaPortamonete : ContentPage
    {
        string strMonete, strCarta, dataPortaMonete;
        string strMoneteIeri, strCartaIeri, dataPortaMoneteIeri;
        string strDataOdierna, strDataIeri;
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

            dataPortaMoneteIeri = Preferences.Get("dataPortaMoneteIeri", "");
            strMoneteIeri = Preferences.Get("MoneteIeri", "");
            strCartaIeri = Preferences.Get("CartaIeri", "");

            strDataOdierna = DateTime.Now.ToString("dd-MM-yyyy");
            strDataIeri = DateTime.Now.AddDays(-1).ToString("dd-MM-yyyy");

            if (dataPortaMonete==strDataIeri)
            {
                strMoneteIeri = strMonete;
                strCarta = strCartaIeri;
                dataPortaMoneteIeri = strDataIeri;
                Preferences.Set("MoneteIeri", strMoneteIeri);
                Preferences.Set("CartaIeri", strCartaIeri);
                Preferences.Set("dataPortaMoneteIeri", strDataIeri);
            }
            if (dataPortaMoneteIeri != strDataIeri)
            {
                strMoneteIeri = "";
                strCarta = "";
                dataPortaMoneteIeri = strDataIeri;
                Preferences.Set("MoneteIeri", strMoneteIeri);
                Preferences.Set("CartaIeri", strCartaIeri);
                Preferences.Set("dataPortaMoneteIeri", strDataIeri);
            }

            if (strDataOdierna!=dataPortaMonete)
            {
                strMonete = "";
                strCarta = "";
                dataPortaMonete = strDataOdierna;
                Preferences.Set("Monete", strMonete);
                Preferences.Set("Carta", strCarta);
                Preferences.Set("dataPortaMonete", strDataOdierna);
            }
            txtMonete.Text = strMonete;
            txtCarta.Text = strCarta;
        }
        private void btnSalvaPortaMonete_Click(object sender, EventArgs e)
        {
            dataPortaMonete = DateTime.Now.ToString("dd-MM-yyyy");
            Preferences.Set("Monete", txtMonete.Text);
            Preferences.Set("Carta", txtCarta.Text);
            Preferences.Set("dataPortaMonete", dataPortaMonete);
            lblEsitoPortamonete.Text = "Salvataggio eseguito alle " + DateTime.Now.ToString("HH:mm:ss");
        }
    }
}