using AuthenService.Application.Interfaces;
using AuthenService.Application.Services;
using AuthenService.Application.Utils;
using AuthenService.Domain.Entities;
using AuthenService.Infrastructure.Data;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using Serilog;
using Serilog.Templates;
using Serilog.Templates.Themes;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

Log.Logger = new LoggerConfiguration()
    .WriteTo.Console()
    .CreateBootstrapLogger();

Log.Information("Starting up!");

builder.Services.AddSerilog((services, lc) => lc
.ReadFrom.Configuration(builder.Configuration)
.ReadFrom.Services(services)
.Enrich.FromLogContext()
.WriteTo.Async(a => a.File("logs/log.txt", rollingInterval: RollingInterval.Day))
.WriteTo.Async(a => a.Console(new ExpressionTemplate(
// Include trace and span ids when present.
"[{@t:HH:mm:ss} {@l:u3}{#if @tr is not null} ({substring(@tr,0,4)}:{substring(@sp,0,4)}){#end}] {@m}\n{@x}",
theme: TemplateTheme.Code)))
);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddDbContext<AppDbContext>(
  options => options.UseSqlServer(builder.Configuration.GetConnectionString("Default"),
  p =>
  {
      p.EnableRetryOnFailure();
      p.MaxBatchSize(150);
  }));

builder.Services.AddIdentity<User, Role>(
             options =>
             {
                 // Password settings.
                 options.Password.RequireDigit = false;
                 options.Password.RequireLowercase = false;
                 options.Password.RequireUppercase = false;
                 options.Password.RequireNonAlphanumeric = false;
                 options.Password.RequiredLength = 6;
                 options.Password.RequiredUniqueChars = 1;
                 options.Lockout.AllowedForNewUsers = true;
                 options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(10);
                 options.Lockout.MaxFailedAccessAttempts = 3;
                 options.User.RequireUniqueEmail = true;


             })
    .AddEntityFrameworkStores<AppDbContext>()
    .AddDefaultTokenProviders();

//set JWT
var jwtSettingsSection = builder.Configuration.GetSection(nameof(JwtSettings));
builder.Services.Configure<JwtSettings>(jwtSettingsSection);

var jwtSettings = jwtSettingsSection.Get<JwtSettings>() ?? throw new ArgumentNullException(nameof(builder.Configuration), "JWT settings section is not configured properly.");

//Encode Secret Key
var secretKey = Encoding.UTF8.GetBytes(jwtSettings.Key);
builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
    .AddJwtBearer(o =>
    {
        o.RequireHttpsMetadata = false;
        o.SaveToken = true;
        o.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuerSigningKey = true,
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ClockSkew = TimeSpan.Zero,
            ValidIssuer = jwtSettings.Issuer,
            ValidAudience = jwtSettings.Audience,
            IssuerSigningKey = new SymmetricSecurityKey(secretKey)
        };
    });




builder.Services.AddScoped<IAuthService, AuthService>();
builder.Services.AddScoped<IUserService, UserService>();


var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();
app.UseSerilogRequestLogging();

app.MapControllers();

app.Run();
