﻿using Microsoft.AspNetCore.Mvc;
using Travellour.Business.DTOs.StatusCode;
using Travellour.Business.DTOs.User;
using Travellour.Business.Interfaces;

namespace Travellour.Controllers;

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
}