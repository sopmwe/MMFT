using System;
using System.Collections.Generic;

namespace dotnetcore.DB.Models;

public partial class PNutzer
{
    public int? KeyId { get; set; }

    public string Uuid { get; set; }

    public string PrivateKey { get; set; }

    public virtual Nutzer Uu { get; set; }
}
