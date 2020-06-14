using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace moneySmart
{
    public partial class App : Application
    {
        public static Boolean loginOk { get; set; } = false;
        public static Boolean ConnessioneOk { get; set; } = false;

        public App()
        {
            InitializeComponent();

            MainPage = new MainPage();
        }

        protected override void OnStart()
        {
        }

        protected override void OnSleep()
        {
        }

        protected override void OnResume()
        {
        }
    }
}
