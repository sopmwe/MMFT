using System.Net.Sockets;
using System.Net;
using System.Text;
using System.Threading;
using Verarbeiter;

namespace Tcp
{
    public partial class Tcp
    {
        private TcpListener paketeTyp1;
        private TcpListener paketeTyp2;
        private TcpListener paketeTyp3;
        private TcpListener paketeTyp1B;
        private TcpListener paketeTyp2B;
        private TcpListener paketeTyp3B;

        private bool istAn = false;

        private void BtnStartClick(object sender, EventArgs e)
        {
            if (!istAn)
            {
                istAn = true;
                Thread t = new Thread(VerwalteVerbindung);
                t.IsBackground = true; //Thread wird beendet wenn die Anwendung geschlossen wird
                t.Start();
            }
        }

        public void VerwalteVerbindung()
        {
            //Alle Listener erstellen
            paketeTyp1 = new TcpListener(IPAddress.Any, 50001);
            paketeTyp2 = new TcpListener(IPAddress.Any, 50002);
            paketeTyp3 = new TcpListener(IPAddress.Any, 50003);
            paketeTyp1B = new TcpListener(IPAddress.Any, 60001);
            paketeTyp2B = new TcpListener(IPAddress.Any, 60002);
            paketeTyp3B = new TcpListener(IPAddress.Any, 60003);

            //Alle Listener aktiv schalten
            paketeTyp1.Start();
            paketeTyp2.Start();
            paketeTyp3.Start();
            paketeTyp1B.Start();
            paketeTyp2B.Start();
            paketeTyp3B.Start();

            //für jeden Port einen neuen Thread starten, damit sie sich nicht Blockieren
            Thread t1 = new Thread(LauscheTyp1);
            t1.IsBackground = true;
            t1.Start();
            Thread t2 = new Thread(LauscheTyp2);
            t2.IsBackground = true;
            t2.Start();
            Thread t3 = new Thread(LauscheTyp3);
            t3.IsBackground = true;
            t3.Start();
            Thread t1B = new Thread(LauscheTyp1B);
            t1B.IsBackground = true;
            t1B.Start();
            Thread t2B = new Thread(LauscheTyp2B);
            t2B.IsBackground = true;
            t2B.Start();
            Thread t3B = new Thread(LauscheTyp3B);
            t3B.IsBackground = true;
            t3B.Start();
        }
        //Lausch-Methoden, die die Threads ausführen sollen
        private void LauscheTyp1()
        {
            while (istAn = true)
            {
                TcpClient client = paketeTyp1.AcceptTcpClient();
                Verarbeiter.Verarbeiter.VerarbeiteTyp1(client, "Typ 1");
            }
        }
        private void LauscheTyp2()
        {
            while (istAn = true)
            {
                TcpClient client = paketeTyp2.AcceptTcpClient();
                Verarbeiter.Verarbeiter.VerarbeiteTyp2(client, "Typ 2");
            }
        }
        private void LauscheTyp3()
        {
            while (istAn = true)
            {
                TcpClient client = paketeTyp3.AcceptTcpClient();
                Verarbeiter.Verarbeiter.VerarbeiteTyp3(client, "Typ 3");
            }
        }
        private void LauscheTyp1B()
        {
            while (istAn = true)
            {
                TcpClient client = paketeTyp1B.AcceptTcpClient();
                Verarbeiter.Verarbeiter.VerarbeiteTyp1(client, "Typ 1B");
            }
        }
        private void LauscheTyp2B()
        {
            while (istAn = true)
            {
                TcpClient client = paketeTyp2B.AcceptTcpClient();
                Verarbeiter.Verarbeiter.VerarbeiteTyp2(client, "Typ 2B");
            }
        }
        private void LauscheTyp3B()
        {
            while (istAn = true)
            {
                TcpClient client = paketeTyp3B.AcceptTcpClient();
                Verarbeiter.Verarbeiter.VerarbeiteTyp3(client, "Typ 3B");
            }
        }

    }
}
