using System;
using System.Collections.Generic;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace moneySmart.Pagine
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class paginaMaster : ContentPage
    {
        public class voceMenu
        {
            public string Testo { get; set; }
            public string Icona { get; set; }
            public string Comando { get; set; }

            public override string ToString()
            {
                return Testo;
            }
        }

        public IList<voceMenu> vociMenu { get; private set; }

        public paginaMaster()
        {
            InitializeComponent();

            vociMenu = new List<voceMenu>();
            vociMenu.Add(new voceMenu
            {
                Testo = "Cassa",
                Icona = "cassa128.png",
                Comando = "paginaCassa"
            });

            vociMenu.Add(new voceMenu
            {
                Testo = "Sospesi",
                Icona = "sospesi128.png",
                Comando = "paginaStore"
            });

            vociMenu.Add(new voceMenu
            {
                Testo = "Vista",
                Icona = "calendario128.png",
                Comando = "paginaVisualizza"
            });

            vociMenu.Add(new voceMenu
            {
                Testo = "Cambia password",
                Icona = "password128.png",
                Comando = "paginaCambiaPassword"
            });

            vociMenu.Add(new voceMenu
            {
                Testo = "Fondo cassa giornaliero",
                Icona = "wallet128.png",
                Comando = "paginaPortamonete"
            });

            vociMenu.Add(new voceMenu
            {
                Testo = "Strumenti",
                Icona = "tools128.png",
                Comando = "paginaStrumenti"
            });

            BindingContext = this;
        }

        private void cambiaPagina(string comando)
        {
            var masterDetail = App.Current.MainPage as MasterDetailPage;
            switch (comando)
            {
                case "paginaCassa":
                    masterDetail.Detail = new NavigationPage(new Pagine.paginaCassa());
                    masterDetail.IsPresented = false;
                    break;
                case "paginaStore":
                    masterDetail.Detail = new NavigationPage(new Pagine.paginaStore());
                    masterDetail.IsPresented = false;
                    break;
                case "paginaVisualizza":
                    masterDetail.Detail = new NavigationPage(new Pagine.paginaVisualizza());
                    masterDetail.IsPresented = false;
                    break;
                case "paginaCambiaPassword":
                    masterDetail.Detail = new NavigationPage(new Pagine.paginaCambiaPassword());
                    masterDetail.IsPresented = false;
                    break;
                case "paginaPortamonete":
                    masterDetail.Detail = new NavigationPage(new Pagine.paginaPortamonete());
                    masterDetail.IsPresented = false;
                    break;
                case "paginaStrumenti":
                    masterDetail.Detail = new NavigationPage(new Pagine.paginaStrumenti());
                    masterDetail.IsPresented = false;
                    break;
            }
        }


        private void OnItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            voceMenu selectedItem = e.SelectedItem as voceMenu;
            cambiaPagina(selectedItem.Comando);
        }

        private void OnItemTapped(object sender, ItemTappedEventArgs e)
        {
            voceMenu tappedItem = e.Item as voceMenu;
            cambiaPagina(tappedItem.Comando);
        }

        private void btnLogout_Clicked(object sender, EventArgs e)
        {
            ShowExitDialog();
        }

        private async void ShowExitDialog()
        {
            var risposta = await DisplayAlert("Attenzione!", "Vuoi davvero uscire?", "Si", "No");
            if (risposta)
            {
                //System.Environment.Exit(0);
                System.Diagnostics.Process.GetCurrentProcess().CloseMainWindow();
            }
        }

    }
}