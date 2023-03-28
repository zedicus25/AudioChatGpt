using System;
using System.Collections.Generic;

namespace Domain.Models;

public class User
{
    public int Id { get; set; }

    public string Login { get; set; } = null!;

    public string Password { get; set; } = null!;

    public int CountRequests { get; set; }

    public DateTime? LastRequest { get; set; }

    public int SubscriptionId { get; set; }

    public virtual ICollection<History> Histories { get; } = new List<History>();

    public virtual Subscription Subscription { get; set; } = null!;
}
