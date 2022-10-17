using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Travellour.Business.DTOs.StatusCode;
using Travellour.Business.DTOs.UserDTO;
using Travellour.Business.Interfaces;

namespace Travellour.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize(AuthenticationSchemes = "Bearer")]
public class FriendController : Controller
{
    private readonly IUnitOfWorkService _unitOfWorkService;

    public FriendController(IUnitOfWorkService unitOfWorkService)
    {
        _unitOfWorkService = unitOfWorkService;
    }

    [HttpPost("addFriend/{id}")]
    public async Task<ActionResult> AddFriendAsync(string? id)
    {
        try
        {
            await _unitOfWorkService.FriendService.FriendAddAsync(id);
            return StatusCode(StatusCodes.Status200OK, new Response { Status = "Success", Message = "Requested succesfully!" });
        }
        catch (Exception ex)
        {
            return StatusCode(StatusCodes.Status502BadGateway, new Response { Status = "Error", Message = ex.ToString() });
        }
    }

    [HttpPost("acceptFriend/{id}")]
    public async Task<ActionResult> AcceptFriendAsync(string? id)
    {
        try
        {
            await _unitOfWorkService.FriendService.FriendAcceptAsync(id);
            return StatusCode(StatusCodes.Status200OK, new Response { Status = "Success", Message = "Accepted succesfully!" });
        }
        catch (Exception ex)
        {
            return StatusCode(StatusCodes.Status502BadGateway, new Response { Status = "Error", Message = ex.ToString() });
        }
    }

    [HttpPut("deleteFriend/{id}")]
    public async Task<ActionResult> RemoveFriendAsync(string? id)
    {
        try
        {
            await _unitOfWorkService.FriendService.FriendRemoveAsync(id);
            return StatusCode(StatusCodes.Status200OK, new Response { Status = "Success", Message = "Requested succesfully!" });
        }
        catch (Exception ex)
        {
            return StatusCode(StatusCodes.Status502BadGateway, new Response { Status = "Error", Message = ex.ToString() });
        }
    }

    [HttpPut("rejectFriend/{id}")]
    public async Task<ActionResult> RejectFriendAsync(string? id)
    {
        try
        {
            await _unitOfWorkService.FriendService.FriendRejectAsync(id);
            return StatusCode(StatusCodes.Status200OK, new Response { Status = "Success", Message = "Requested succesfully!" });
        }
        catch (Exception ex)
        {
            return StatusCode(StatusCodes.Status502BadGateway, new Response { Status = "Error", Message = ex.ToString() });
        }
    }

    [HttpGet("friendRequests")]
    public async Task<ActionResult> FriendRequestAsync()
    {
        try
        {
            return Ok(await _unitOfWorkService.FriendService.GetFriendRequestAsync());
        }
        catch (Exception ex)
        {
            return StatusCode(StatusCodes.Status502BadGateway, new Response { Status = "Error", Message = ex.ToString() });
        }
    }

    [HttpGet("friendRequestsPagination")]
    public async Task<ActionResult> FriendRequestPaginationAsync()
    {
        try
        {
            return Ok(await _unitOfWorkService.FriendService.GetPaginationFriendRequestAsync());
        }
        catch (Exception ex)
        {
            return StatusCode(StatusCodes.Status502BadGateway, new Response { Status = "Error", Message = ex.ToString() });
        }
    }

    [HttpGet("friendSuggestion")]
    public async Task<ActionResult> GetFriendSuggestionAsync()
    {
        try
        {
            return Ok(await _unitOfWorkService.FriendService.GetFriendSuggestionAsync());
        }
        catch (Exception ex)
        {
            return StatusCode(StatusCodes.Status502BadGateway, new Response { Status = "Error", Message = ex.ToString() });
        }
    }

    [HttpGet("getAllFriend")]
    public async Task<ActionResult<List<UserGetDto>>> GetAllFriendAsync()
    {
        try
        {
            return Ok(await _unitOfWorkService.FriendService.FriendGetAllAsync());
        }
        catch (Exception ex)
        {
            return StatusCode(StatusCodes.Status502BadGateway, new Response { Status = "Error", Message = ex.ToString() });
        }
    }

    [HttpPut("FriendRequestCancel/{id}")]
    public async Task<ActionResult> FriendRequestCancel(string? friendId)
    {
        try
        {
            await _unitOfWorkService.FriendService.CancelFriendRequestAsync(friendId);
            return StatusCode(StatusCodes.Status200OK, new Response { Status = "Success", Message = "Requested succesfully!" });
        }
        catch (Exception ex)
        {
            return StatusCode(StatusCodes.Status502BadGateway, new Response { Status = "Error", Message = ex.ToString() });
        }
    }

    [HttpGet("FriendSearch/{username}")]
    public async Task<ActionResult<List<UserGetDto>>> FriendSearchByUsernameAsync(string username)
    {
        try
        {
            return Ok(await _unitOfWorkService.FriendService.SearchFriendByUsernameAsync(username));
        }
        catch (Exception ex)
        {
            return StatusCode(StatusCodes.Status502BadGateway, new Response { Status = "Error", Message = ex.ToString() });
        }
    }
}
