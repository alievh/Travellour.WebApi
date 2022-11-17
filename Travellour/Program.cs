using FluentValidation;
using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.FileProviders;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Travellour.Business.DTOs.AuthenticationDTO;
using Travellour.Business.DTOs.EventDTO;
using Travellour.Business.DTOs.ForumDTO;
using Travellour.Business.DTOs.GroupDTO;
using Travellour.Business.DTOs.PostDTO;
using Travellour.Business.Hubs;
using Travellour.Business.Implementations;
using Travellour.Business.Interfaces;
using Travellour.Business.Profiles;
using Travellour.Business.Token.Implementations;
using Travellour.Business.Token.Interfaces;
using Travellour.Business.Validator.Authentication;
using Travellour.Business.Validator.Event;
using Travellour.Business.Validator.Forum;
using Travellour.Business.Validator.Group;
using Travellour.Business.Validator.Post;
using Travellour.Core;
using Travellour.Core.Entities;
using Travellour.Data;
using Travellour.Data.DAL;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddControllers().AddNewtonsoftJson(options =>
             options.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore);

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowOrigins",
        builder => builder.WithOrigins("http://localhost:3000").WithMethods("PUT", "DELETE", "GET"));
});

builder.Services.AddAuthentication(options =>
{
    options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
}).AddJwtBearer(option =>
{
    option.TokenValidationParameters = new TokenValidationParameters
    {
        ValidAudience = builder.Configuration.GetSection("Jwt:audience").Value,
        ValidIssuer = builder.Configuration.GetSection("Jwt:issuer").Value,
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration.GetSection("Jwt:securityKey").Value)),
    };
});

builder.Services.AddDbContext<AppDbContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("default"));
});

builder.Services.AddIdentity<AppUser, IdentityRole>()
    .AddEntityFrameworkStores<AppDbContext>()
    .AddDefaultTokenProviders();

builder.Services.Configure<IdentityOptions>(options =>
{
    options.Lockout.MaxFailedAccessAttempts = 5;
    options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(3);

    options.Password.RequiredLength = 8;
    options.Password.RequireDigit = true;
    options.Password.RequireUppercase = true;
    options.Password.RequireLowercase = true;

    options.User.RequireUniqueEmail = true;
    options.SignIn.RequireConfirmedEmail = true;
});

#pragma warning disable CS0618 // Type or member is obsolete
builder.Services.AddControllers().AddNewtonsoftJson(options =>
             options.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore)
                .AddFluentValidation(fl => fl.RegisterValidatorsFromAssemblyContaining<Program>());
#pragma warning restore CS0618 // Type or member is obsolete
builder.Services.AddTransient<IValidator<Register>, RegisterValidator>();
builder.Services.AddTransient<IValidator<Login>, LoginValidator>();
builder.Services.AddTransient<IValidator<PostCreateDto>, PostCreateValidator>();
builder.Services.AddTransient<IValidator<GroupCreateDto>, GroupCreateValidator>();
builder.Services.AddTransient<IValidator<EventCreateDto>, EventCreateValidator>();
builder.Services.AddTransient<IValidator<ForumCreateDto>, ForumCreateValidator>();

builder.Services.AddScoped<IJwtService, JwtService>();
builder.Services.AddMapperService();

builder.Services.AddScoped<IUnitOfWorkService, UnitOfWorkService>();
builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();

builder.Services.AddTransient<Hub<IChatClient>, ChatHub>();
builder.Services.AddSignalR();

builder.Services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
builder.Services.AddHttpContextAccessor();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseStaticFiles(new StaticFileOptions()
{
    FileProvider = new PhysicalFileProvider(Path.Combine(app.Environment.ContentRootPath, "images")),
    RequestPath = "/img"
});

app.UseRouting();

app.UseCors(x => x
    .WithOrigins("http://localhost:3000")
    .AllowAnyMethod()
    .AllowAnyHeader()
    .AllowCredentials()
    .SetIsOriginAllowed(origin => true)
);

app.UseHttpsRedirection();

app.UseAuthentication();

app.UseAuthorization();

app.UseEndpoints(endpoints =>
{
    endpoints.MapHub<OnlineHub>("/onlinehub");
    endpoints.MapHub<ChatHub>("/chathub");
});

app.MapControllers();

app.Run();
