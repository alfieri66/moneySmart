using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace moneySmart
{
    // Learn more about making custom code visible in the Xamarin.Forms previewer
    // by visiting https://aka.ms/xamarinforms-previewer
    [DesignTimeVisible(false)]
    public partial class MainPage : MasterDetailPage
    {

        public MainPage()
        {
            InitializeComponent();
            Master = new Pagine.paginaMaster();
            Detail = new NavigationPage(new Pagine.paginaCassa());
            Navigation.PushModalAsync(new Pagine.paginaLogin());
        }
    }
}
