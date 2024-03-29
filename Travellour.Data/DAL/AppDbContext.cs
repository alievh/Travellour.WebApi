﻿using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Travellour.Core.Entities;

namespace Travellour.Data.DAL;

public class AppDbContext : IdentityDbContext<AppUser>
{
#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
    public AppDbContext(DbContextOptions options) : base(options) { }
#pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.

    public DbSet<Image> Images { get; set; }
    public DbSet<Post> Posts { get; set; }
    public DbSet<Like> Likes { get; set; }
    public DbSet<Comment> Comments { get; set; }
    public DbSet<UserFriend> UserFriends { get; set; }
    public DbSet<Forum> Forums { get; set; }
    public DbSet<Group> Groups { get; set; }
    public DbSet<Event> Events { get; set; }
    public DbSet<Notification> Notifications { get; set; }
    public DbSet<Message> Messages { get; set; }
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<AppUser>().HasMany(u => u.UserFriends).WithOne(uf => uf.User).HasForeignKey(uf => uf.UserId);
        modelBuilder.Entity<AppUser>().HasMany(u => u.Notifications).WithOne(uf => uf.Receiver).HasForeignKey(uf => uf.ReceiverId);
        modelBuilder.Entity<AppUser>().HasMany(u => u.OwnedEvents).WithOne(uf => uf.EventCreator).HasForeignKey(uf => uf.EventCreatorId);
        modelBuilder.Entity<AppUser>().HasMany(u => u.JoinedEvents).WithMany(uf => uf.EventMembers);
        modelBuilder.Entity<AppUser>().HasMany(u => u.OwnedGroups).WithOne(uf => uf.GroupAdmin).HasForeignKey(uf => uf.GroupAdminId);
        modelBuilder.Entity<AppUser>().HasMany(u => u.JoinedGroups).WithMany(uf => uf.GroupMembers);
        base.OnModelCreating(modelBuilder);
    }
}
