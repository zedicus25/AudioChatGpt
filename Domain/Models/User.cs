using System;
using System.Collections.Generic;

namespace Domain.Models;

public class User
{
    public int Id { get; set; }

    public string IdentityId { get; set; }

    public int CountRequests { get; set; }

    public DateTime? LastRequest { get; set; }
    public bool IsBanned { get; set; }
    public DateTime? UnbanTime { get; set; }

    public int SubscriptionId { get; set; }

    public virtual ICollection<History> Histories { get; } = new List<History>();

    public virtual Subscription Subscription { get; set; } = null!;
}
