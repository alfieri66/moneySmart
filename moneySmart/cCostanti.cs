using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace moneySmart
{
    class cCostanti
    {
        //public string uri = "https://www.dolcemare.eu/InterrogaDB.asmx/";
        public string uri = "https://www.moneysmart.cloud/InterrogaDB.asmx/";
        public string dbPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Personal), "store.db3");
    }
}
