using System;
using System.Collections.Generic;

namespace Domain.Models;

public class Request
{
    public int Id { get; set; }

    public string RequestText { get; set; } = null!;

    public DateTime RequestTime { get; set; }

    public virtual ICollection<History> Histories { get; } = new List<History>();
}
