using Forum.Domain.Interfaces;
using Microsoft.AspNetCore.Identity;

namespace Forum.Domain.Users;

public class Role : IdentityRole<int>, IEntity
{
    public DateTime CreatedAt { get; set; }
    public DateTime ModifiedAt { get; set; }
}
