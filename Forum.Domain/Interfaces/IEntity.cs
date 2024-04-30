namespace Forum.Domain.Interfaces;

public interface IEntity
{
    DateTime CreatedAt { get; set; }
    DateTime ModifiedAt { get; set; }
}
