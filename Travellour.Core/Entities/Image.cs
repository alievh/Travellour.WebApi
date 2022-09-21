using Travellour.Core.Entities.Base;

namespace Travellour.Core.Entities;

public class Image : IEntity
{
    public int Id { get; set; }
    public string? ImageUrl { get; set; }
}
