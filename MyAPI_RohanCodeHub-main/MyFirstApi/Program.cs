using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using MyFirstApi.AdminApi.IService;
using MyFirstApi.AdminApi.Services;
using MyFirstApi.Data;
using MyFirstApi.IService;
using MyFirstApi.Services;
using System.Security.Claims;
using System.Text;
using Microsoft.EntityFrameworkCore;
using MyFirstApi.AdminApi.Entities;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(
        builder.Configuration.GetConnectionString("DefaultConnection")
    )
);


builder.Services.AddCors(options =>
{
    options.AddPolicy("ReactPolicy", policy =>
    {
        policy
            .WithOrigins("http://localhost:5173")
            .AllowAnyHeader()
            .AllowAnyMethod()
            .AllowCredentials();
    });
});


builder.Services
    .AddAuthentication(options =>
    {
        // Existing JWT remains the default scheme.
        options.DefaultAuthenticateScheme =
            JwtBearerDefaults.AuthenticationScheme;

        options.DefaultChallengeScheme =
            JwtBearerDefaults.AuthenticationScheme;
    })

    
    .AddJwtBearer(options =>
    {
        options.RequireHttpsMetadata = false;
        options.SaveToken = true;

        options.TokenValidationParameters =
            new TokenValidationParameters
            {
                ValidateIssuer = true,
                ValidateAudience = true,
                ValidateLifetime = true,
                ValidateIssuerSigningKey = true,

                ValidIssuer =
                    builder.Configuration["JWT:Issuer"],

                ValidAudience =
                    builder.Configuration["JWT:Audience"],

                IssuerSigningKey =
                    new SymmetricSecurityKey(
                        Encoding.UTF8.GetBytes(
                            builder.Configuration["JWT:Key"]!
                        )
                    )
            };

        options.Events = new JwtBearerEvents
        {
            OnTokenValidated = async context =>
            {
                var userId = context.Principal?
                    .FindFirst(ClaimTypes.NameIdentifier)?.Value;

                var sessionId = context.Principal?
                    .FindFirst("session_id")?.Value;

                if (string.IsNullOrEmpty(userId) ||
                    string.IsNullOrEmpty(sessionId))
                {
                    context.Fail("Invalid Request");
                    return;
                }

                var db = context.HttpContext.RequestServices
                    .GetRequiredService<AppDbContext>();

                if (!Guid.TryParse(userId, out Guid guidUserId))
                {
                    context.Fail("Invalid User Id");
                    return;
                }

                var user = await db.Users
                    .FirstOrDefaultAsync(x => x.Id == guidUserId);

                if (user == null)
                {
                    context.Fail("Invalid User");
                    return;
                }

                if (user.SessionId != sessionId)
                {
                    context.Fail("Session Expired");
                    return;
                }
            },

            OnChallenge = async context =>
            {
                context.HandleResponse();

                context.Response.StatusCode =
                    StatusCodes.Status401Unauthorized;

                context.Response.ContentType = "application/json";

                await context.Response.WriteAsJsonAsync(new
                {
                    success = false,
                    message =
                        "Unauthorized. Please provide a valid access token."
                });
            }
        };
    })

        .AddJwtBearer("AdminJwt", options =>
        {
            options.RequireHttpsMetadata = false;
            options.SaveToken = true;

            options.TokenValidationParameters =
                new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,

                    ValidIssuer =
                        builder.Configuration["AdminJWT:Issuer"],

                    ValidAudience =
                        builder.Configuration["AdminJWT:Audience"],

                    IssuerSigningKey =
                        new SymmetricSecurityKey(
                            Encoding.UTF8.GetBytes(
                                builder.Configuration["AdminJWT:Key"]!
                            )
                        ),

                    NameClaimType = ClaimTypes.Name,
                    RoleClaimType = ClaimTypes.Role
                };
        })

    .AddCookie("CourseCookie", options =>
    {
        options.Cookie.Name = "CourseManagement.Auth";
        options.Cookie.HttpOnly = true;
        options.Cookie.SecurePolicy = CookieSecurePolicy.Always;
        options.Cookie.SameSite = SameSiteMode.None;

        options.ExpireTimeSpan = TimeSpan.FromMinutes(30);
        options.SlidingExpiration = true;

        options.Events.OnRedirectToLogin = context =>
        {
            context.Response.StatusCode =
                StatusCodes.Status401Unauthorized;

            return context.Response.WriteAsJsonAsync(new
            {
                success = false,
                message = "Please log in first."
            });
        };

        options.Events.OnRedirectToAccessDenied = context =>
        {
            context.Response.StatusCode =
                StatusCodes.Status403Forbidden;

            return context.Response.WriteAsJsonAsync(new
            {
                success = false,
                message = "You do not have permission to access this resource."
            });
        };
    });


builder.Services.AddAuthorization();

builder.Services.AddControllers();

builder.Services.AddEndpointsApiExplorer();

builder.Services.AddSwaggerGen(options =>
{
 
    options.AddSecurityDefinition(
        "Bearer",
        new OpenApiSecurityScheme
        {
            Name = "Authorization",
            Type = SecuritySchemeType.Http,
            Scheme = "bearer",
            BearerFormat = "JWT",
            In = ParameterLocation.Header,
            Description = "Enter JWT token. Example: Bearer {your token}"
        }
    );

    options.AddSecurityRequirement(
        new OpenApiSecurityRequirement
        {
            {
                new OpenApiSecurityScheme
                {
                    Reference = new OpenApiReference
                    {
                        Type = ReferenceType.SecurityScheme,
                        Id = "Bearer"
                    }
                },
                Array.Empty<string>()
            }
        }
    );
});

builder.Services.AddScoped<IAuthService, AuthService>();
builder.Services.AddScoped<ISalaryService, SalaryServices>();
builder.Services.AddScoped<IEmployeeService, EmployeeService>();
builder.Services.AddScoped<ICourseService, CourseService>();

builder.Services.AddDbContext<AdminDbContext>(options =>
    options.UseSqlServer(
        builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddScoped<IAdminService, AdminService>();


var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var dbContext = scope.ServiceProvider
        .GetRequiredService<AppDbContext>();

    try
    {
        dbContext.Database.Migrate();
    }
    catch (Exception ex)
    {
        Console.WriteLine("Database Migration Error:");
        Console.WriteLine(ex);
    }
}

app.UseSwagger();
app.UseSwaggerUI();

app.UseHttpsRedirection();

app.UseCors("ReactPolicy");

app.UseAuthentication();

app.UseAuthorization();

app.MapControllers();

app.Run();
