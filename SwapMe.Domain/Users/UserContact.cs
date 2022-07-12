using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SwapMe.Domain.Users;

public record UserContact(string FirstName, string LastName, string Email, long PhoneNumber, string City, string State)
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public long ContactId { get; set; }
    
    [ForeignKey(nameof(User.UserId))]
    public long UserId { get; set; }
}