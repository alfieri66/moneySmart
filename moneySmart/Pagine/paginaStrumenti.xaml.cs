using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using Xamarin.Essentials;

namespace moneySmart.Pagine
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class paginaStrumenti : ContentPage
    {
        cCostanti costanti = new cCostanti();
        cDatabase archivio = new cDatabase();
        public paginaStrumenti()
        {
            InitializeComponent();
        }

        private void btnCreaDatabase_Click(object sender, EventArgs e)
        {
            File.Delete(costanti.dbPath);
            archivio.apriDB();
        }

        private void btnCreaValoriDiProva_Click(object sender, EventArgs e)
        {
            Boolean esito = false;
            string tmpUser;
            tmpUser = Preferences.Get("mbUser", "");
            if (tmpUser.Trim().Length > 0)
            {
                archivio.apriDB();
                esito = archivio.aggiungOperazione(100100101, tmpUser, 51, 51, 51);
                esito = archivio.aggiungOperazione(100100101, tmpUser, 101, 101, 101);
                esito = archivio.aggiungOperazione(100100101, tmpUser, 151, 151, 151);
            }
        }
    }
}