using MMFT.DB.Models;
using MMFT.TCP;
using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using System.Windows;
using System.Threading.Tasks;


namespace MMFT.DB
{
    public class NachrichtenVerwalten
    {
        // Hier werden die Nachrichten zum Senden gespeichert, der TCP dings kann die dann nehmen und senden i guess wenn das so geht
        // EmfängerUUID, TInhalt und DInhalt müssen der Methode übergeben werden 
        public static async Task SpeichereNachrichtSenden(string euuid, string tInhalt, byte[] dInhalt)
        {
            using var db = new MessengerDbContext();

            var suuid = db.PNutzers.Select(p => p.Uuid).FirstOrDefault();
            if (suuid == null)
            {
                MessageBox.Show("Eigene UUID nicht gefunden");
                return;
            }

            // Eigner Nutzer Datensatz
            var eigenerNutzer = db.Nutzers.FirstOrDefault(n => n.Uuid == suuid);
            if (eigenerNutzer == null)
            {
                MessageBox.Show("Eigener Nutzer nicht gefunden");
                return;
            }

            // Empfaenger Datensatz
            var empfaenger = db.Nutzers.FirstOrDefault(n => n.Uuid == euuid);
            if (empfaenger == null)
            {
                MessageBox.Show("Empfänger nicht gefunden");
                return;
            }

            // Verschluesselung für DB
            string? tInhaltDb;
            byte[]? dInhaltDb;

            try
            {
                tInhaltDb = RsaHelfer.VerschluesselText(tInhalt, eigenerNutzer.PublicKey);
                dInhaltDb = RsaHelfer.VerschluesselBytes(dInhalt, eigenerNutzer.PublicKey);
            }
            catch(Exception ex)
            {
                string fehlerText = ex.Message;
                if (ex.InnerException != null)
                {
                    fehlerText += "\n\nInnerException: " + ex.InnerException.Message;
                }
                MessageBox.Show(fehlerText);
                return;
            }

            long zeitstempel = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds();
            try
            {
                var nachricht = new Nachrichten
                {

                    EUuid = euuid,
                    SUuid = suuid,
                    Zeitstempel = zeitstempel,
                    TInhalt = tInhaltDb,
                    DInhalt = dInhaltDb

                };

                db.Nachrichtens.Add(nachricht);
                db.SaveChanges();
            }
            catch (Exception ex)
            {
                string fehlerText = ex.Message;
                if (ex.InnerException != null)
                {
                    fehlerText += "\n\nInnerException: " + ex.InnerException.Message;
                }
                MessageBox.Show(fehlerText);
                return;
            }

            // Verschluesselung für TCP
            string? tInhaltTcp = RsaHelfer.VerschluesselText(tInhalt, empfaenger.PublicKey);
            byte[]? dInhaltTcp = RsaHelfer.VerschluesselBytes(dInhalt, empfaenger.PublicKey);

            if (tInhaltTcp == null || dInhaltTcp == null)
            {
                MessageBox.Show("Inhalt darf leer aber nicht null sein");
                return;
            }

            var paket = new PaketTyp2
            {
                EUuid = euuid,
                SUuid = suuid,
                Zeitstempel = zeitstempel,
                TInhalt = tInhaltTcp,
                DInhalt = dInhaltTcp
            };

            await Tcp.SendePaket(empfaenger.Ip, paket);

        }
        // Typ 2 Nachrichten werden hier in der DB gespeichert. Werden direkt von dem TCP Dings geholt
        /*public static void SpeichereNachrichtEmpfangen(PaketTyp2 paket)
        {
            using var db = new MessengerDbContext();

            var nachricht = new Nachrichten
            {
                EUuid = paket.EUuid,
                SUuid = paket.SUuid,
                Zeitstempel = paket.Zeitstempel,
                TInhalt = paket.TInhalt,
                DInhalt = paket.DInhalt
            };

            db.Nachrichtens.Add(nachricht);
            db.SaveChanges();
        }*/
    }
}
