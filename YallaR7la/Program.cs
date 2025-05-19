using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using YallaR7la.Data;
using YallaR7la.Data.Models;
using YallaR7la.Extintions;
using YallaR7la.Services;
using YallaR7la.Settings;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container
builder.Services.AddControllers();
builder.Services.AddMemoryCache();
builder.Services.AddSignalR();
builder.Services.Configure<EmailSetings>(builder.Configuration.GetSection("MailSettings"));
builder.Services.AddTransient<IEmailServices, EmailServices>();

// Database context
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("ConnectionYallaR7la")));

// Identity configuration (optional if you use Identity)
builder.Services.AddIdentity<User, IdentityRole>()
    .AddEntityFrameworkStores<AppDbContext>()
    .AddDefaultTokenProviders();

// JWT configuration
var jwtKey = Encoding.UTF8.GetBytes(builder.Configuration["JWT:SecretKey"]);

builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    options.RequireHttpsMetadata = true;
    options.SaveToken = true;
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        ValidIssuer = builder.Configuration["JWT:Issuer"],
        ValidAudience = builder.Configuration["JWT:Audience"],
        IssuerSigningKey = new SymmetricSecurityKey(jwtKey)
    };
});

// Authorization policies
builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("AdminOnly", policy => policy.RequireClaim("UserType", "Admin"));
    options.AddPolicy("OwnerOnly", policy => policy.RequireClaim("UserType", "Owner"));
    options.AddPolicy("UserOnly", policy => policy.RequireClaim("UserType", "User"));
});

// Swagger (API documentation)
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwagerGenJWTAuth();

var app = builder.Build();

// Configure the HTTP request pipeline
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthentication(); // Must be before UseAuthorization
app.UseAuthorization();
app.MapControllers();

app.Run();
