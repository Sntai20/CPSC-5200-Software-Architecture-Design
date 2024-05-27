namespace eShop.Ordering.Infrastructure.Idempotency;
using System.ComponentModel.DataAnnotations;

public class ClientRequest
{
    public Guid Id { get; set; }
    [Required]
    public string Name { get; set; }
    public DateTime Time { get; set; }
}
