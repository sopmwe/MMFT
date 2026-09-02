using System;
using System.Collections.Generic;
using System.Text;
using System.Net;
using System.Net.Sockets;
using System.Security.Cryptography;
using MMFT.DB.Models;
using System.Windows;

namespace MMFT.DB
{
    public class NutzerVerwalten
    {
        // Erzeugt RSA Keys
        private (string publicKey, string privateKey) generiereRSA()
        {
            using RSA rsa = RSA.Create(3072);

            string publicKeyPem = rsa.ExportRSAPublicKeyPem();
            string privateKeyPem = rsa.ExportRSAPrivateKeyPem();

            return (publicKeyPem, privateKeyPem);
        }
        // Ermittelt die Eigene IPV4 Adresse
        private string EigeneIP()
        {
            var host = Dns.GetHostEntry(Dns.GetHostName());
            foreach (var ip in host.AddressList)
            {
                if (ip.AddressFamily == AddressFamily.InterNetwork)
                {
                    return ip.ToString();
                }
            }
            throw new Exception("Keine IP-Adresse gefunden");
        }

        public void NutzerAnlegen()
        {
            var keys = generiereRSA();
            string publicKey = keys.publicKey;
            string privateKey = keys.privateKey;

            string ip = EigeneIP();

            string uuid = Guid.CreateVersion7().ToString();

            string name = "Test";

            using (var db = new MessengerDbContext())
            {
                try
                {
                    var nutzer = new Nutzer
                    {
                        Uuid = uuid,
                        PublicKey = publicKey,
                        Name = name,
                        Ip = ip,
                        //Pbild = profilbild,
                        //Status = status
                    };

                    var pNutzer = new PNutzer
                    {
                        Uuid = uuid,
                        PrivateKey = privateKey
                    };
                    
                    db.Nutzers.Add(nutzer);
                    db.PNutzers.Add(pNutzer);
                    db.SaveChanges();

                }
                catch (Exception ex)
                {
                    MessageBox.Show("Fehler beim Speichern" + ex.Message);
                }
            }
        }
    }
}
