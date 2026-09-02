using System.Net.Sockets;
using System.Net;


namespace Tcp
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
                 //   Thread t = new Thread(Verarbeiter.Verarbeiter.VerarbeiteEingehendesPaket(client));
                   // t.IsBackground = true;
                    //t.Start();
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Fehler beim Verbindungsaufbau");
                }
            }
        }
    }
}
