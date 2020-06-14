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
    public partial class paginaCambiaPassword : ContentPage
    {
        public paginaCambiaPassword()
        {
            InitializeComponent();
            if (App.loginOk == false)
            {
                System.Environment.Exit(0);
            }

        }
    }
}