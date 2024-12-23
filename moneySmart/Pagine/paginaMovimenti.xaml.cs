using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace moneySmart.Pagine
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class paginaBilancio : ContentPage
	{
		public paginaBilancio ()
		{
			InitializeComponent ();
		}

		async private void btnCercaMovimenti_Click(object sender, EventArgs e)
		{
            await DisplayAlert("Attenzione!", "Connessione non presente ", "Ok");
        }

    }
}