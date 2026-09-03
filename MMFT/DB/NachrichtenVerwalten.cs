using MMFT.DB.Models;
using Modelle;
using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using System.Windows;

namespace MMFT.DB
{
    public class NachrichtenVerwalten
    {
        // Hier werden die Nachrichten zum Senden gespeichert, der TCP dings kann die dann nehmen und senden i guess wenn das so geht
        // EmfängerUUID, TInhalt und DInhalt müssen der Methode übergeben werden 
        public static void SpeichereNachrichtSenden(string euuid, string tInhalt, byte[] dInhalt)
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
            string tInhaltDb = RsaHelfer.VerschluesselText(tInhalt, eigenerNutzer.PublicKey);
            byte[] dInhaltDb = RsaHelfer.VerschluesselBytes(dInhalt, eigenerNutzer.PublicKey);


            long zeitstempel = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds();

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

            // Verschluesselung für TCP
            string tInhaltTcp = RsaHelfer.VerschluesselText(tInhalt, empfaenger.PublicKey);
            byte[] dInhaltTcp = RsaHelfer.VerschluesselBytes(dInhalt, empfaenger.PublicKey);


            var paket = new PaketTyp2
            {
                EUuid = euuid,
                SUuid = suuid,
                Zeitstempel = zeitstempel,
                TInhalt = tInhaltTcp,
                DInhalt = dInhaltTcp
            };

            // sendeNachrichtTyp2TCP(paket);

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
