using System;
using System.Collections.Generic;
using System.Text;
using System.Security.Cryptography;

namespace MMFT.DB
{
    public class RsaHelfer
    {
        // Verschlüsselt Text / Für die eigene DB mit eigenem Public Key verschlüsseln und mit eigenem Private entschlüsseln
        // Zum senden mit dem Public Key der anderen Person verschlüsseln ins TCP Paket
        // Zum eigenen abspeichern aber stattdessen mit eigenem Public Key verschlüsseln damit es so in der DB ist
        // Alle nachrichten in der DB können dann it dem eigene Private Key entschlüsselt werden
        public static string? VerschluesselText(string klartext, string key)
        {
            if (string.IsNullOrEmpty(klartext))
                return null;
            // klartext in bytes umwandeln
            byte[] klartextBytes = Encoding.UTF8.GetBytes(klartext);
            // verschlüsselmethode wird mit key und klartextbytes aufgerufen
            byte[] verschluesselt = VerschluesselBytes(klartextBytes, key);
            if (verschluesselt == null)
                return null;
            return Convert.ToBase64String(verschluesselt);
        }

        //Entschlüsselt Text it dem eigenen Private Key
        public static string? EntschluesselText(string verschluesselterText, string privateKey)
        {
            if (string.IsNullOrEmpty(verschluesselterText))
                return null;

            byte[] verschluesselteBytes = Convert.FromBase64String(verschluesselterText);
            // entschlüsselmethode wird aufgerufen it key und den verschlüsseltem bytes
            byte[] klartextBytes = EntschluesselBytes(verschluesselteBytes, privateKey);
            if (klartextBytes == null)
                return null;
            // Bytes in Text
            return Encoding.UTF8.GetString(klartextBytes);
        }

        public static byte[]? VerschluesselBytes(byte[] daten, string key)
        {
            if (daten == null)
                return null;

            using RSA rsa = RSA.Create();
            // Public Key in Bytes umwandeln
            rsa.ImportRSAPublicKey(Convert.FromBase64String(key), out _);
            // verschlüsseln
            return rsa.Encrypt(daten, RSAEncryptionPadding.OaepSHA256);
        }

        // Bytes werden entschluesselt
        public static byte[]? EntschluesselBytes(byte[] verschluesselteDaten, string privateKey)
        {
            if (verschluesselteDaten == null)
                return null;

            using RSA rsa = RSA.Create();
            rsa.ImportRSAPrivateKey(Convert.FromBase64String(privateKey), out _);
            return rsa.Decrypt(verschluesselteDaten, RSAEncryptionPadding.OaepSHA256);
        }
    }
}
