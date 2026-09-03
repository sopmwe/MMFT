using Microsoft.Data.Sqlite;
using Modelle;
using System.Drawing.Text;
using System.IO;
using System.Net.Sockets;
using System.Text;
using System.Text.Json;
using MMFT.DB;
using MMFT.DB.Models;


namespace Verarbeiter
{
    public class Verarbeiter
    {
        
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
            //Maximal 50 MB in Bytes = 50 * 1024 * 1024
            const long maxByteGröße = 50 * 1024 * 1024;
            long gesamtBytesEmpfangen = 0;
            //Netzwerk-Stream vom Client holen & Memory STream im Ram anlegen, wo die Daten kurz abgelegt werden
            using (NetworkStream stream = client.GetStream())
            using (MemoryStream ms = new MemoryStream())
            {
                byte[] puffer = new byte[8192];
                int bytesRead;
                //Daten werden stückweise aus dem Netz gelesen und im Ram gesammelt
                while ((bytesRead = stream.Read(puffer, 0, puffer.Length)) > 0)
                {
                    gesamtBytesEmpfangen += bytesRead;

                    //damit nicht mehr als 50 MB
                    if(gesamtBytesEmpfangen > maxByteGröße)
                    {
                        throw new InvalidOperationException("Paket ist zu groß!");
                    }
                    ms.Write(puffer, 0, bytesRead);
                }
                //Alle Bytes in einen lesbaren String ausgeben
                return Encoding.UTF8.GetString(ms.ToArray());
            }
        }

        private static void SpeicherTyp1(string jsonText)
        {
            //Json wird in ein Objekt umgewandelt:
            PaketTyp1 paket = JsonSerializer.Deserialize<PaketTyp1>(jsonText);
            //Datenbank Verbindung öffnen
            using var db = new MessengerDbContext();

            
            
        }

        private static void SpeicherTyp2(string jsonText) {
            //Json in Objekt umwandeln
            PaketTyp2 paket = JsonSerializer.Deserialize<PaketTyp2>(jsonText);

            
        }
        private static void VerarbeiteTyp3(string jsonText) {
            //Json in Objekt umwandeln
            PaketTyp3 paket = JsonSerializer.Deserialize<PaketTyp3>(jsonText);
            
        }
    }
}