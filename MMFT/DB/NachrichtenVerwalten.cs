using MMFT.DB.Models;
using Modelle;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;

namespace MMFT.DB
{
    public class NachrichtenVerwalten
    {
        // Hier werden die Nachrichten zum Senden gespeichert, der TCP dings kann die dann nehmen und senden i guess wenn das so geht
        public static void SpeichereNachrichtSenden(string euuid, string tInhalt, byte[] dInhalt)
        {
            using var db = new MessengerDbContext();

            var suuid = db.PNutzers.Select(p => p.Uuid).FirstOrDefault();
            if (suuid == null)
            {
                MessageBox.Show("Eigene UUID nicht gefunden");
                return;
            }

            long zeitstempel = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds();

            var nachricht = new Nachrichten
            {

                EUuid = euuid,
                SUuid = suuid,
                Zeitstempel = zeitstempel,
                TInhalt = tInhalt,
                DInhalt = dInhalt

            };

            db.Nachrichtens.Add(nachricht);
            db.SaveChanges();
        }
        // Typ 2 Nachrichten werden hier in der DB gespeichert. Werden direkt von dem TCP Dings geholt
        public static void SpeichereNachrichtEmpfangen(PaketTyp2 paket)
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
        }
    }
}
