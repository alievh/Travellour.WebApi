﻿using Microsoft.AspNetCore.Http;

namespace Travellour.Business.DTOs.EventDTO;

public class EventCreateDto
{
    public string? EventTitle { get; set; }
    public string? EventDescription { get; set; }
    public List<IFormFile>? ImageFiles { get; set; }
}
