using Microsoft.AspNetCore.Http;

namespace Travellour.Business.DTOs.GroupDTO;

public class GroupCreateDto
{
    public string? GroupName { get; set; }
    public string? GroupDescription { get; set; }
}
