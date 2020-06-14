using System;
using System.Collections.Generic;
using System.Text;

namespace moneySmart
{
    class cGlobali
    {
        public struct tOperazione
        {
            public int idOperazione;
            public long codiceLocale;
            public string emailUtente;
            public float acconto;
            public float recupero;
            public float daRiportare;
            public DateTime dataOperazione;
            public string idTmp;
        }

        public static List<tOperazione> listaOperazioni = new List<tOperazione>();
        public tOperazione operazione = new tOperazione();

    }
}
