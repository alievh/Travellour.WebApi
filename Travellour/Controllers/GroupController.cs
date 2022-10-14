using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Travellour.Business.DTOs.GroupDTO;
using Travellour.Business.DTOs.PostDTO;
using Travellour.Business.DTOs.StatusCode;
using Travellour.Business.Interfaces;

namespace Travellour.Controllers;

[Authorize(AuthenticationSchemes = "Bearer")]
[Route("api/[controller]")]
[ApiController]
public class GroupController : Controller
{
    private readonly IUnitOfWorkService _unitOfWorkService;

    public GroupController(IUnitOfWorkService unitOfWorkService)
    {
        _unitOfWorkService = unitOfWorkService;
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<GroupGetDto>> GroupGetAsync(int id)
    {
        try
        {
            return Ok(await _unitOfWorkService.GroupService.GetAsync(id));
        }
        catch (Exception ex)
        {
            return StatusCode(StatusCodes.Status502BadGateway, new Response { Status = "Error", Message = ex.ToString() });
        }
    }

    [HttpGet("GroupGetAll")]
    public async Task<ActionResult<List<GroupGetDto>>> GroupGetAllAsync()
    {
        try
        {
            return Ok(await _unitOfWorkService.GroupService.GetAllAsyn());
        }
        catch (Exception ex)
        {
            return StatusCode(StatusCodes.Status502BadGateway, new Response { Status = "Error", Message = ex.ToString() });
        }
    }

    [HttpGet("MyGroupGet/{id}")]
    public async Task<ActionResult<List<GroupGetDto>>> MyGroupGetAsync(string id)
    {
        try
        {
            return Ok(await _unitOfWorkService.GroupService.GetMyGroupsAsync(id));
        }
        catch (Exception ex)
        {
            return StatusCode(StatusCodes.Status502BadGateway, new Response { Status = "Error", Message = ex.ToString() });
        }
    }

    [HttpPost("GroupCreate")]
    public async Task<ActionResult> GroupCreateAsync(GroupCreateDto groupCreateDto)
    {
        try
        {
            await _unitOfWorkService.GroupService.CreateAsync(groupCreateDto);
            return StatusCode(StatusCodes.Status200OK, new Response { Status = "Success", Message = "Group created succesfully!" });
        }
        catch (Exception ex)
        {
            return StatusCode(StatusCodes.Status502BadGateway, new Response { Status = "Error", Message = ex.ToString() });
        }
    }

    [HttpGet("GroupProfile/{id}")]
    public async Task<ActionResult<GroupProfileDto>> GroupProfileGetAsync(int id)
    {
        try
        {
            return Ok(await _unitOfWorkService.GroupService.GetGroupProfileAsync(id));
        }
        catch (Exception ex)
        {
            return StatusCode(StatusCodes.Status502BadGateway, new Response { Status = "Error", Message = ex.ToString() });
        }
    }

    [HttpGet("GroupPostGetAll/{id}")]
    public async Task<ActionResult<List<PostGetDto>>> GroupPostGetAllAsync(int id)
    {
        try
        {
            return Ok(await _unitOfWorkService.GroupService.GetAllGroupPostAsync(id));
        }
        catch (Exception ex)
        {
            return StatusCode(StatusCodes.Status502BadGateway, new Response { Status = "Error", Message = ex.ToString() });
        }
    }

    [HttpGet("GroupSearch/{groupName}")]
    public async Task<ActionResult<List<PostGetDto>>> GroupSearchByName(string groupName)
    {
        try
        {
            return Ok(await _unitOfWorkService.GroupService.SearchGroupByName(groupName));
        }
        catch (Exception ex)
        {
            return StatusCode(StatusCodes.Status502BadGateway, new Response { Status = "Error", Message = ex.ToString() });
        }
    }

    [HttpPost("GroupJoin/{id}")]
    public async Task<ActionResult> GroupJoinAsync(int id)
    {
        try
        {
            await _unitOfWorkService.GroupService.JoinGroupAsync(id);
            return StatusCode(StatusCodes.Status200OK, new Response { Status = "Success", Message = "Group created succesfully!" });
        }
        catch (Exception ex)
        {
            return StatusCode(StatusCodes.Status502BadGateway, new Response { Status = "Error", Message = ex.ToString() });
        }
    }

    [HttpPost("GroupLeave/{id}")]
    public async Task<ActionResult> GroupLeaveAsync(int id)
    {
        try
        {
            await _unitOfWorkService.GroupService.LeaveGroupAsync(id);
            return StatusCode(StatusCodes.Status200OK, new Response { Status = "Success", Message = "Group created succesfully!" });
        }
        catch (Exception ex)
        {
            return StatusCode(StatusCodes.Status502BadGateway, new Response { Status = "Error", Message = ex.ToString() });
        }
    }

    [HttpPost("GroupKick/{userId}/{groupId}")]
    public async Task<ActionResult> GroupLeaveAsync(string userId, int groupId)
    {
        try
        {
            await _unitOfWorkService.GroupService.KickUserFromGroupAsync(userId, groupId);
            return StatusCode(StatusCodes.Status200OK, new Response { Status = "Success", Message = "Group created succesfully!" });
        }
        catch (Exception ex)
        {
            return StatusCode(StatusCodes.Status502BadGateway, new Response { Status = "Error", Message = ex.ToString() });
        }
    }

    [HttpPost("changeGroupPhoto/{id}")]
    public async Task<ActionResult> ChangeProfilePhoto(int id,[FromForm] GroupPhotoDto groupPhotoDto)
    {
        try
        {
            await _unitOfWorkService.GroupService.ChangeGroupPhotoAsync(id, groupPhotoDto);
            return StatusCode(StatusCodes.Status200OK, new Response { Status = "Success", Message = "Image updated succesfully!" });
        }
        catch (Exception ex)
        {
            return StatusCode(StatusCodes.Status502BadGateway, new Response { Status = "Error", Message = ex.ToString() });
        }
    }

    [HttpPost("changeGroupCover/{id}")]
    public async Task<ActionResult> ChangeCoverPhoto(int id, [FromForm] GroupCoverDto groupCoverDto)
    {
        try
        {
            await _unitOfWorkService.GroupService.ChangeGroupCoverAsync(id, groupCoverDto);
            return StatusCode(StatusCodes.Status200OK, new Response { Status = "Success", Message = "Image updated succesfully!" });
        }
        catch (Exception ex)
        {
            return StatusCode(StatusCodes.Status502BadGateway, new Response { Status = "Error", Message = ex.ToString() });
        }
    }

    [HttpPost("GroupChange")]
    public async Task<ActionResult> GroupUpdateAsync(GroupUpdateDto groupUpdateDto)
    {
        try
        {
            await _unitOfWorkService.GroupService.ChangeGroupAsync(groupUpdateDto);
            return StatusCode(StatusCodes.Status200OK, new Response { Status = "Success", Message = "Image updated succesfully!" });
        }
        catch (Exception ex)
        {
            return StatusCode(StatusCodes.Status502BadGateway, new Response { Status = "Error", Message = ex.ToString() });
        }
    }
}
