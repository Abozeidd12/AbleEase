using AbleEaseCore.IServices;
using AbleEaseCore.Services;
using AbleEaseDomain.IRepositeries;
using AbleEaseInfrastructure.Data;
using AbleEaseInfrastructure.Identity;
using AbleEaseInfrastructure.Repositeries;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Serilog;
using System;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddDbContext<AbleEaseDbContext>(options =>

options.UseSqlServer(builder.Configuration["ConnectionStrings:DefaultConnection"]));

builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
builder.Services.AddScoped<ITokenService, TokenService>();
builder.Services.AddScoped<IAccountService, AccountService>();
builder.Services.AddScoped<IPatientService, PatientService>();
builder.Services.AddScoped<IDisabilityService, DisabilityService>();
<<<<<<< HEAD
<<<<<<< HEAD


=======
builder.Services.AddScoped<IFinancialAidService, FinancialAidService>();
>>>>>>> ef0ff50cf9e66b0acc7107f0d475ee1e07b189d6
=======
builder.Services.AddScoped<IFinancialAidService, FinancialAidService>();
>>>>>>> ef0ff50cf9e66b0acc7107f0d475ee1e07b189d6







builder.Services.AddScoped<IPatientDisabilityService, PatientDisabilityService>();

builder.Services.AddDefaultIdentity<ApplicationUser>(options =>

{
    options.SignIn.RequireConfirmedEmail = false;
    options.Tokens.EmailConfirmationTokenProvider = TokenOptions.DefaultProvider;
    options.Password.RequireNonAlphanumeric = true;
    options.Password.RequireDigit = true;
    options.Password.RequiredUniqueChars = 1;
    options.Password.RequiredLength = 8;



}
).AddRoles<IdentityRole>().AddEntityFrameworkStores<AbleEaseDbContext>();

builder.Services.AddAuthentication(options =>


{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
}).AddJwtBearer(options =>
{
    options.SaveToken = true;
    options.TokenValidationParameters = new Microsoft.IdentityModel.Tokens.TokenValidationParameters()
    {
        ValidateAudience = true,
        ValidateIssuer = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        RequireExpirationTime = true,
        ValidAudience = builder.Configuration["JWT:Audience"],
        ValidIssuer = builder.Configuration["JWT:Issuer"],
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["JWT:SecretKey"]!))
    };


});


Log.Logger = new LoggerConfiguration()

    .WriteTo.Console()
    .WriteTo.File("logs/log-1", rollingInterval: RollingInterval.Day)
    .CreateLogger();

builder.Host.UseSerilog();
Log.Logger.Information("Application is building...........");

var app = builder.Build();

try
{
    // Configure the HTTP request pipeline.
    if (app.Environment.IsDevelopment())
    {
        app.UseSwagger();
        app.UseSwaggerUI();
    }
    app.UseSerilogRequestLogging();


    app.UseHttpsRedirection();

    app.UseAuthorization();

    app.MapControllers();
    Log.Logger.Information("Application is Running...........");


    app.Run();
}
catch (Exception ex)
{
    Log.Logger.Error($"error occured {ex.Message}");


}
finally
{
    Log.CloseAndFlush();
}
