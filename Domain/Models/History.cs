

namespace Domain.Models;

public class History
{
    public int Id { get; set; }

    public int UserId { get; set; }

    public int RequestId { get; set; }

    public int ResponceId { get; set; }

    public virtual Request Request { get; set; } = null!;

    public virtual Responce Responce { get; set; } = null!;

    public virtual User User { get; set; } = null!;
}
