using System;
using System.Collections.Generic;
using System.Text;
using SQLite;
using System.IO;

namespace moneySmart.Pagine
{
    [Table("Operazioni")]
    public class cassa
    {
        [PrimaryKey, AutoIncrement, Column("Id")]
        public int Id { get; set; }
        public long codiceLocale { get; set; }
        public string emailUtente { get; set; }
        public float acconto { get; set; }
        public float recupero { get; set; }
        public float daRiportare { get; set; }
        public DateTime dataOperazione { get; set; }
    }

    class cDatabase
    {
        cCostanti costanti = new cCostanti();
        SQLiteConnection db;
        cGlobali globale = new cGlobali();

        public void apriDB()
        {
            if (!File.Exists(costanti.dbPath))
            {
                db = new SQLiteConnection(costanti.dbPath);
                creaTabella();
            }
            else
            {
                db = new SQLiteConnection(costanti.dbPath);
            }
        }
        public void creaTabella()
        {
            db.CreateTable<cassa>();
        }

        public Boolean aggiungOperazione(long codiceLocale, string emailUtente, float acconto, float recupero, float daRiportare)
        {
            cassa operazione = new cassa();
            Boolean esito = false;
            try
            {

                operazione.codiceLocale = codiceLocale;
                operazione.emailUtente = emailUtente;
                operazione.acconto = acconto;
                operazione.recupero = recupero;
                operazione.daRiportare = daRiportare;
                operazione.dataOperazione = DateTime.Now;
                db.Insert(operazione);
                esito = true;
            }
            catch
            {
                esito = false;
            }
            return esito;
        }

        public void estraiTutteLeOperazioni(string emailUtente, List<cGlobali.tOperazione> lista)
        {
            cGlobali.tOperazione operazione;
            var tabella = db.Query<cassa>("SELECT * FROM Operazioni where emailUtente = ? order by Id ", emailUtente);
            foreach (var riga in tabella)
            {
                operazione = new cGlobali.tOperazione();

                operazione.codiceLocale = riga.codiceLocale;
                operazione.acconto= riga.acconto;
                operazione.recupero= riga.recupero;
                operazione.daRiportare = riga.daRiportare;
                operazione.dataOperazione = riga.dataOperazione;
                operazione.idOperazione = riga.Id;
                operazione.idTmp = "";
                lista.Add(operazione);
            }
        }

        public int contaOperazioniInSospeso(string emailUtente)
        {
            int totOperazioni = 0;
            apriDB();
            totOperazioni = db.ExecuteScalar<int> ("SELECT count(*) FROM Operazioni where emailUtente = ? ", emailUtente);
            return totOperazioni;
        }


        public void eliminaOperazione(int idOperazione)
        {
            var tuttoLeOperazioni = db.Execute("Delete  FROM Operazioni where Id = ?", idOperazione);
        }

        public bool estraiTestaOperazione(ref long codiceLocale, ref string emailUtente, ref float acconto, ref float recupero, ref float daRiportare, ref DateTime dataOperazione)
        {
            Boolean esito = false;
            cassa operazione = new cassa();
            var tabella = db.Query<cassa>("SELECT * FROM Operazioni where emailUtente = ?  ORDER BY Id LIMIT 1 ", emailUtente);
            
            try
            {
                operazione = tabella[0];
                codiceLocale = operazione.codiceLocale;
                emailUtente = operazione.emailUtente;
                acconto = operazione.acconto;
                recupero = operazione.recupero;
                daRiportare = operazione.daRiportare;
                dataOperazione=operazione.dataOperazione;
                esito = true;
            }
            catch
            {
                esito = false;
            }
            return esito;
        }
    }
}