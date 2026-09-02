using Microsoft.Data.Sqlite;
using Modelle;
using System.Drawing.Text;
using System.IO;
using System.Net.Sockets;
using System.Text;
using System.Text.Json;

namespace Verarbeiter
{
    public class Verarbeiter
    {
        private static string connectionString = "";
        
        //Methode, die den Header liest und weiterleitet
        public static void VerarbeiteEingehendesPaket(TcpClient client)
        {
            try
            {
                string jsonText = LiesJsonAusStream(client);
                HeaderPaket header = JsonSerializer.Deserialize<HeaderPaket>(jsonText);
                switch (header.Typ)
                {
                    case 1:
                        SpeicherTyp1(jsonText);
                        break;
                    case 2:
                        SpeicherTyp2(jsonText);
                        break;
                    case 3:
                        VerarbeiteTyp3(jsonText);
                        break;
                    default:
                        Console.WriteLine("Unbekannter Paket-Typ");
                        break;
                }

            }
            catch(Exception ex) 
            {
                Console.WriteLine("Fehler beim Verarbeiten der eingehenden Pakete");
            }
            finally 
            {
                client.Close();
            }            
        }

        //Hilfsmethode um Json auszulesen
        private static string LiesJsonAusStream(TcpClient client)
        {
            using (NetworkStream stream = client.GetStream())
            using (MemoryStream ms = new MemoryStream())
            {
                byte[] puffer = new byte[8192];
                int bytesRead;

                while ((bytesRead = stream.Read(puffer, 0, puffer.Length)) > 0)
                {
                    ms.Write(puffer, 0, bytesRead);
                }
                return Encoding.UTF8.GetString(ms.ToArray());
            }
        }

        private static void SpeicherTyp1(string jsonText)
        {
            //Json wird in ein Objekt umgewandelt:
            PaketTyp1 paket = JsonSerializer.Deserialize<PaketTyp1>(jsonText);
            //Datenbank Verbindung öffnen

            using (SqliteConnection con = new SqliteConnection(connectionString))
            {
                con.Open();
                string sql = @"Insert into Nutzer (UUID, Public_Key, Name, IP, P_Bild) values
                                (@uuid, @pubkey, @name, @ip, @pBild)";
                using (SqliteCommand cmd = new SqliteCommand(sql, con))
                {
                    cmd.Parameters.AddWithValue("@uuid", paket.Uuid);
                    cmd.Parameters.AddWithValue("@pubkey", paket.PublicKey);
                    cmd.Parameters.AddWithValue("@name", paket.Name);
                    cmd.Parameters.AddWithValue("@ip", paket.Ip);
                    cmd.Parameters.AddWithValue("@pBild", (object)paket.PBild ?? DBNull.Value);
                    cmd.ExecuteNonQuery();
                }
            }
            Console.WriteLine($"Kontakt {paket.Name} gespeichert!");
        }

        private static void SpeicherTyp2(string jsonText) {
            //Json in Objekt umwandeln
            PaketTyp2 paket = JsonSerializer.Deserialize<PaketTyp2>(jsonText);

            using (SqliteConnection con = new SqliteConnection(connectionString))
            {
                con.Open();
                string sql = @"Insert into Nachrichten (E_UUID, S_UUID, Zeitstempel, T_Inhalt, D_Inhalt)
                              values (@eUuid, @sUuid, @zeitstempel, @tInhalt, @dInhalt";
                using (SqliteCommand cmd = new SqliteCommand(sql, con))
                {
                    cmd.Parameters.AddWithValue("@eUuid", paket.EUuid);
                    cmd.Parameters.AddWithValue("@sUuid", paket.SUuid);
                    cmd.Parameters.AddWithValue("@zeitstempel", paket.Zeitstempel);
                    cmd.Parameters.AddWithValue("@tInhalt", (object)paket.TInhalt ?? DBNull.Value); //Wenn nichts geschickt wird, soll 
                    cmd.Parameters.AddWithValue("@dInhalt", (object)paket.DInhalt ?? DBNull.Value); //"Null" in die DB geschrieben werden
                    cmd.ExecuteNonQuery();
                }
            }
            Console.WriteLine("Nachricht gespeichert!");
        }
        private static void VerarbeiteTyp3(string jsonText) {
            //Json in Objekt umwandeln
            PaketTyp3 paket = JsonSerializer.Deserialize<PaketTyp3>(jsonText);
            Console.WriteLine($"Sync-Anfrage von UUID {paket.Uuid} erhalten");
        }
    }
}