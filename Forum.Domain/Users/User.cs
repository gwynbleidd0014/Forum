using Forum.Domain.Comments;
using Forum.Domain.Images;
using Forum.Domain.Interfaces;
using Forum.Domain.Topics;
using Microsoft.AspNetCore.Identity;

namespace Forum.Domain.Users;

public class User : IdentityUser<int>, IEntity, IBanable
{
    public string? FirstName { get; set; }
    public string? LastName { get; set; }
    public Image Image { get; set; }
    public List<Comment> Comments { get; set; }
    public List<Topic> Topics { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime ModifiedAt { get; set; }
    public bool IsBanned { get; set; }
    public DateTime? BannedUntil { get; set; }
}
