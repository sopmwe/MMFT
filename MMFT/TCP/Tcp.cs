using System.Net.Sockets;
using System.Net;
using System.Text.Json;
using System.Text;


namespace MMFT.TCP
{
    public partial class Tcp
    {
        private TcpListener tcpListener;
        private int Port = 50001; //ein Port für alles 

        private bool istAn = false;

        private void BtnStartClick(object sender, EventArgs e)
        {
            if (!istAn)
            {
                istAn = true;
                Thread t = new Thread(VerwalteVerbindungsEingang);
                t.IsBackground = true; //Thread wird beendet wenn die Anwendung geschlossen wird
                t.Start();
            }
        }

        public void VerwalteVerbindungsEingang()
        {
            //Listener erstellen
            tcpListener = new TcpListener(IPAddress.Any, Port);

            //Listener aktiv schalten
            tcpListener.Start();
            while (istAn == true)
            {
                try
                {
                    TcpClient client = tcpListener.AcceptTcpClient();

                    //Verarbeiter entscheidet anhand des Json headers, was passiert
                    //Thread t = new Thread(Verarbeiter.VerarbeiteEingehendesPaket(client));
                    Thread t = new Thread(() => Verarbeiter.VerarbeiteEingehendesPaket(client));
                    t.IsBackground = true;
                    t.Start();
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Fehler beim Verbindungsaufbau");
                }
            }
        }

        public static async Task SendePaket(string zielIp, object paket)
        {
            try
            {
                //Objekt erstmal in JSON umwandeln
                string jsonText = JsonSerializer.Serialize(paket);
                byte[] daten = Encoding.UTF8.GetBytes(jsonText);

                //Verbindung zum Client herstellen:
                using (TcpClient client = new TcpClient())
                {
                    await client.ConnectAsync(zielIp, 50001);
                    using (NetworkStream stream = client.GetStream())
                    {
                        await stream.WriteAsync(daten, 0, daten.Length);
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Fehler beim Senden an {zielIp}");
            }
        }

    }
}
