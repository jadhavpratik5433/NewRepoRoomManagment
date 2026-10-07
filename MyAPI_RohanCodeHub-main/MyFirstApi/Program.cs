using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using MyFirstApi.Data;
using MyFirstApi.IService;
using MyFirstApi.Services;
using System.Security.Claims;
using System.Text;

var builder = WebApplication.CreateBuilder(args);


// ======================================================
// DATABASE
// ======================================================

builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(
        builder.Configuration.GetConnectionString("DefaultConnection")
    )
);


// ======================================================
// CORS - REACT FRONTEND
// ======================================================

builder.Services.AddCors(options =>
{
    options.AddPolicy("ReactPolicy", policy =>
    {
        policy
            .AllowAnyOrigin()
            .AllowAnyHeader()
            .AllowAnyMethod();
    });
});


// ======================================================
// JWT AUTHENTICATION
// ======================================================

builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme =
        JwtBearerDefaults.AuthenticationScheme;

    options.DefaultChallengeScheme =
        JwtBearerDefaults.AuthenticationScheme;

})
.AddJwtBearer(options =>
{
    options.RequireHttpsMetadata = false;

    options.SaveToken = true;


    // JWT Validation
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


    // ==================================================
    // TOKEN VALIDATED
    // ==================================================

    options.Events = new JwtBearerEvents
    {
        OnTokenValidated = async context =>
        {
            var userId =
                context.Principal?
                    .FindFirst(ClaimTypes.NameIdentifier)?
                    .Value;


            var sessionId =
                context.Principal?
                    .FindFirst("session_id")?
                    .Value;


            // Check Claims
            if (string.IsNullOrEmpty(userId) ||
                string.IsNullOrEmpty(sessionId))
            {
                context.Fail("Invalid Request");

                return;
            }


            // Get DB Context
            var db =
                context.HttpContext
                    .RequestServices
                    .GetRequiredService<AppDbContext>();


            // Check Guid
            if (!Guid.TryParse(
                    userId,
                    out Guid guidUserId))
            {
                context.Fail("Invalid User Id");

                return;
            }


            // Find User
            var user =
                await db.Users
                    .FirstOrDefaultAsync(
                        x => x.Id == guidUserId
                    );


            if (user == null)
            {
                context.Fail("Invalid User");

                return;
            }


            // Check Session
            if (user.SessionId.ToString() != sessionId)
            {
                context.Fail("Session Expired");

                return;
            }
        },


        // ==================================================
        // UNAUTHORIZED RESPONSE
        // ==================================================

        OnChallenge = async context =>
        {
            context.HandleResponse();


            context.Response.StatusCode =
                StatusCodes.Status401Unauthorized;


            context.Response.ContentType =
                "application/json";


            var response = new
            {
                success = false,

                message =
                    "Unauthorized. Please provide a valid access token."
            };


            await context.Response
                .WriteAsJsonAsync(response);
        }
    };
});


// ======================================================
// AUTHORIZATION
// ======================================================

builder.Services.AddAuthorization();


// ======================================================
// CONTROLLERS
// ======================================================

builder.Services.AddControllers();


// ======================================================
// SWAGGER
// ======================================================

builder.Services.AddEndpointsApiExplorer();

builder.Services.AddSwaggerGen(options =>
{
    // JWT Bearer Definition
    options.AddSecurityDefinition(
        "Bearer",
        new OpenApiSecurityScheme
        {
            Name = "Authorization",

            Type = SecuritySchemeType.Http,

            Scheme = "bearer",

            BearerFormat = "JWT",

            In = ParameterLocation.Header,

            Description =
                "Enter JWT token. Example: Bearer {your token}"
        }
    );


    // JWT Security Requirement
    options.AddSecurityRequirement(
        new OpenApiSecurityRequirement
        {
            {
                new OpenApiSecurityScheme
                {
                    Reference =
                        new OpenApiReference
                        {
                            Type =
                                ReferenceType.SecurityScheme,

                            Id = "Bearer"
                        }
                },

                Array.Empty<string>()
            }
        }
    );
});


// ======================================================
// SERVICES - DEPENDENCY INJECTION
// ======================================================

builder.Services.AddScoped<IAuthService, AuthService>();
builder.Services.AddScoped<ISalaryService, SalaryServices>(); 
builder.Services.AddScoped<IEmployeeService, EmployeeService>();


// ======================================================
// BUILD APP
// ======================================================

var app = builder.Build();


// ======================================================
// DATABASE MIGRATION
// ======================================================

using (var scope = app.Services.CreateScope())
{
    var dbContext =
        scope.ServiceProvider
            .GetRequiredService<AppDbContext>();


    try
    {
        dbContext.Database.Migrate();
    }
    catch (Exception ex)
    {
        Console.WriteLine(
            "Database Migration Error:"
        );

        Console.WriteLine(ex);
    }
}


// ======================================================
// SWAGGER
// ======================================================

app.UseSwagger();

app.UseSwaggerUI();


// ======================================================
// HTTPS
// ======================================================

app.UseHttpsRedirection();


// ======================================================
// CORS
// ======================================================

app.UseCors("ReactPolicy");


// ======================================================
// AUTHENTICATION
// ======================================================

app.UseAuthentication();


// ======================================================
// AUTHORIZATION
// ======================================================

app.UseAuthorization();


// ======================================================
// CONTROLLERS
// ======================================================

app.MapControllers();


// ======================================================
// RUN
// ======================================================

app.Run();