﻿using Travellour.Core.Entities.Base;

namespace Travellour.Core.Entities;

public class Event : BaseEntity, IEntity
{
    public string? EventTitle { get; set; }
    public string? EventDescription { get; set; }
    public int UserId { get; set; }
    public AppUser? EventCreator { get; set; }
    public ICollection<AppUser>? EventMembers { get; set; }

}
