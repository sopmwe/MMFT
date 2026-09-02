using System;
using System.Collections.Generic;

namespace MMFT.DB.Models;

public partial class Nutzer
{
    public string Uuid { get; set; }

    public string PublicKey { get; set; }

    public string Name { get; set; }

    public string Ip { get; set; }

    public byte[]? PBild { get; set; }

    public string? Status { get; set; }

    public virtual ICollection<Nachrichten> NachrichtenEUus { get; set; } = new List<Nachrichten>();

    public virtual ICollection<Nachrichten> NachrichtenSUus { get; set; } = new List<Nachrichten>();
}
