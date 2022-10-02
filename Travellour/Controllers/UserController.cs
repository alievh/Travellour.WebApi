using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Travellour.Business.DTOs.StatusCode;
using Travellour.Business.DTOs.UserDTO;
using Travellour.Business.Interfaces;

namespace Travellour.Controllers;

[Authorize(AuthenticationSchemes = "Bearer")]
[Route("api/[controller]")]
[ApiController]
public class UserController : Controller
{
    private readonly IUnitOfWorkService _unitOfWorkService;

    public UserController(IUnitOfWorkService unitOfWorkService)
    {
        _unitOfWorkService = unitOfWorkService;
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<UserGetDto>> GetUserAsync(string id)
    {
        try
        {
            return Ok(await _unitOfWorkService.UserService.GetAsync(id));
        }
        catch (Exception ex)
        {
            return StatusCode(StatusCodes.Status502BadGateway, new Response { Status = "Error", Message = ex.ToString() });
        }
    }

    [HttpPost("userUpdate")]
    public async Task<ActionResult> UserUpdateAsyncs(UserUpdateDto userUpdateDto)
    {
        try
        {
            await _unitOfWorkService.UserService.UpdateAsync(userUpdateDto);
            return StatusCode(StatusCodes.Status200OK, new Response { Status = "Success", Message = "Account updated succesfully!" });
        }
        catch (Exception ex)
        {
            return StatusCode(StatusCodes.Status502BadGateway, new Response { Status = "Error", Message = ex.ToString() });
        }
    }

    [HttpPost("changeProfilePhoto")]
    public async Task<ActionResult> ChangeProfilePhoto([FromForm] ProfilePhotoDto profilePhotoDto)
    {
        try
        {
            await _unitOfWorkService.UserService.ChangeProfilePhotoAsync(profilePhotoDto);
            return StatusCode(StatusCodes.Status200OK, new Response { Status = "Success", Message = "Image updated succesfully!" });
        }
        catch (Exception ex)
        {
            return StatusCode(StatusCodes.Status502BadGateway, new Response { Status = "Error", Message = ex.ToString() });
        }
    }

    [HttpPost("changeCoverPhoto")]
    public async Task<ActionResult> ChangeCoverPhoto([FromForm] CoverPhotoDto coverPhotoDto)
    {
        try
        {
            await _unitOfWorkService.UserService.ChangeCoverPhotoAsync(coverPhotoDto);
            return StatusCode(StatusCodes.Status200OK, new Response { Status = "Success", Message = "Image updated succesfully!" });
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
            return Ok(await _unitOfWorkService.UserService.GetFriendSuggestionAsync());
        }
        catch (Exception ex)
        {
            return StatusCode(StatusCodes.Status502BadGateway, new Response { Status = "Error", Message = ex.ToString() });
        }
    }

    [HttpPost("changePassword")]
    public async Task<ActionResult> ChangePasswordAsync([FromBody] PasswordChangeDto passwordChangeDto)
    {
        try
        {
            await _unitOfWorkService.UserService.ChangeUserPasswordAsync(passwordChangeDto);
            return StatusCode(StatusCodes.Status200OK, new Response { Status = "Success", Message = "Password updated succesfully!" });
        }
        catch (Exception ex)
        {
            return StatusCode(StatusCodes.Status502BadGateway, new Response { Status = "Error", Message = ex.ToString() });
        }
    }

    [HttpGet("userProfile/{id}")]
    public async Task<ActionResult> UserProfileAsync(string? id)
    {
        try
        {
            return Ok(await _unitOfWorkService.UserService.GetUserProfileAsync(id));
        }
        catch (Exception ex)
        {
            return StatusCode(StatusCodes.Status502BadGateway, new Response { Status = "Error", Message = ex.ToString() });
        }
    }
}
