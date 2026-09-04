using System;
using System.Collections.Generic;
using System.Text;

namespace MMFT.TCP
{
    //Header type 
    public class HeaderPaket
    {
        public int Typ {  get; set; }
    }

    //Kontakt Paket
    public class PaketTyp1
    {
        public int Typ = 1;
        public bool Antwort;
        public string Uuid { get; set; }
        public string PublicKey { get; set; }
        public string Name { get; set; }
        public string Ip {  get; set; }
        public byte[] PBild { get; set; } //Ich speicher das BLOB hier als byte array
    }

    //Nachricht Paket
    public class PaketTyp2
    {
        public int Typ = 2;
        public string EUuid { get; set; }
        public string SUuid { get; set; }
        public long Zeitstempel { get; set; }
        public string TInhalt { get; set; }
        public byte[] DInhalt { get; set; } //wieder BLOB = byte[]
    }

    //Sync Paket
    public class PaketTyp3
    {
        public int Typ = 3;
        public long Zeitstempel { get; set; }
        public string Uuid { get; set; }
    }
}
