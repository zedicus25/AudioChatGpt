using System;
using System.Collections.Generic;

namespace Domain.Models;

public class Responce
{
    public int Id { get; set; }

    public string ResponceText { get; set; } = null!;

    public virtual ICollection<History> Histories { get; } = new List<History>();
}
