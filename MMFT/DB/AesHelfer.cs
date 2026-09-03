using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;

namespace MMFT.DB
{
    public class AesHelfer
    {
        // Hasht das eingegebene Passwort in 32Bit was für AES benötigt wird. Der Hash ist dann der Schlüssel
        private static byte[] SchluesselErstellen(string passwort)
        {
            using var sha256 = SHA256.Create();
            return sha256.ComputeHash(Encoding.UTF8.GetBytes(passwort));

        }
        // Verschlüsselt den Private Key mit dem Nutzer-Passwort
        public static byte[] VerschluesselPrivateKey(string privateKeyBase64, string passwort)
        {
            // Schluessel holen
            byte[] key = SchluesselErstellen(passwort);

            using Aes aes = Aes.Create();

            // Schluessel dem Aes Object zuweisen
            aes.Key = key;
            // Initialisierungsverktor, macht dann das gleiche nachrichten trotzdem unterschiedlich aussehen in der db
            aes.GenerateIV();

            // private key in Bytes umwandeln
            byte[] privateKeyBytes = Encoding.UTF8.GetBytes(privateKeyBase64);

            using var encryptor = aes.CreateEncryptor();

            // verschluesselt den private key
            byte[] verschluesselterPrivateKey = encryptor.TransformFinalBlock(privateKeyBytes, 0, privateKeyBytes.Length);

            // iv und verschluesselter privat key werden in ergebnis gespeichert
            byte[] verschluesselterPrivateKeyIV = new byte[aes.IV.Length + verschluesselterPrivateKey.Length];
            Buffer.BlockCopy(aes.IV, 0, verschluesselterPrivateKeyIV, 0, aes.IV.Length);
            Buffer.BlockCopy(verschluesselterPrivateKey, 0, verschluesselterPrivateKeyIV, aes.IV.Length, verschluesselterPrivateKey.Length);

            return verschluesselterPrivateKeyIV;
        }


        public static string EntschluesselPrivateKey(byte[] verschluesselterPrivateKey, string passwort)
        {
            // Schluessel wieder ausm passwort holen
            byte[] key = SchluesselErstellen(passwort);

            // Leeres IV 
            byte[] iv = new byte[16];
            byte[] ciphertext = new byte[verschluesselterPrivateKey.Length - 16];
            // IV in iv array
            Buffer.BlockCopy(verschluesselterPrivateKey, 0, iv, 0, 16);
            // Daten ohne iv in ciphertext
            Buffer.BlockCopy(verschluesselterPrivateKey, 16, ciphertext, 0, ciphertext.Length);

            using Aes aes = Aes.Create();
            aes.Key = key;
            aes.IV = iv;

            // entschlüsselung
            using var decryptor = aes.CreateDecryptor();
            byte[] klartextBytes = decryptor.TransformFinalBlock(ciphertext, 0, ciphertext.Length);
            // Bytes in lesbaren string
            return Encoding.UTF8.GetString(klartextBytes);
        }
    }
}
