using System;
using System.Collections.Generic;

namespace MMFT.DB.Models;

public partial class Nachrichten
{
    public string EUuid { get; set; }

    public string SUuid { get; set; }

    public int Zeitstempel { get; set; }

    public string TInhalt { get; set; }

    public byte[] DInhalt { get; set; }

    public virtual Nutzer EUu { get; set; }

    public virtual Nutzer SUu { get; set; }
}
