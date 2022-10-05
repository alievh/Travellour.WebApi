using Travellour.Core.Entities;

namespace Travellour.Business.DTOs.GroupDTO;

public class GroupProfileDto
{
    public int Id { get; set; }
    public string? GroupName { get; set; }
    public string? GroupDescription { get; set; }
    public string? GroupImage { get; set; }
    public int MemberCount { get; set; }
    public int PostCount { get; set; }
    public AppUser? GroupAdmin { get; set; }
}
