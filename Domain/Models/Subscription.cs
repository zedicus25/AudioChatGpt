using System;
using System.Collections.Generic;

namespace Domain.Models;

public class Subscription
{
    public int Id { get; set; }

    public string Title { get; set; } = null!;

    public int MaxRequests { get; set; }

    public virtual ICollection<User> Users { get; } = new List<User>();
}
