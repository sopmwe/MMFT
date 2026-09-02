using System.Net.Sockets;

namespace Verarbeiter
{
    public class Verarbeiter
    {
        public static void VerarbeiteTyp1(TcpClient client, string typ)
        {
            //hier kommt noch rein was mit den Daten passiereb soll
            client.Close(); //Verbindung zum Client am Ende schließen
        }
        public static void VerarbeiteTyp2(TcpClient client, string typ)
        {
            //auch die Logik noch
            client.Close();
        }
        public static void VerarbeiteTyp3(TcpClient client, string typ)
        {
            //ja hier auch Logik nh
            client.Close();
        }
    }
}