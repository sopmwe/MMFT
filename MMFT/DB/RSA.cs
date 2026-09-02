using System;
using System.Collections.Generic;
using System.Text;
using System.Security.Cryptography;

namespace dotnetcore.DB
{
    public class RSA_Vershl
    {
        public void generiereRSA()
        {
            using RSA rsa = RSA.Create(3072);

            string publicKeyPem = rsa.ExportRSAPublicKeyPem();
            string privateKeyPem = rsa.ExportRSAPrivateKeyPem();

            System.Diagnostics.Debug.WriteLine(publicKeyPem);
            System.Diagnostics.Debug.WriteLine(privateKeyPem);
        }
    }
}
