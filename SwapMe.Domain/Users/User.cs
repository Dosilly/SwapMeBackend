using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SwapMe.Domain.Users;

public record User(string Login, string Password, string Salt)
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public long UserId { get; set; }

    public UserContact? UserContact { get; set; }
}