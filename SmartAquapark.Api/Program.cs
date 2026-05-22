using Microsoft.EntityFrameworkCore;
using SmartAquapark.Infrastructure.Persistence;
using FluentValidation;
using FluentValidation.AspNetCore;
using SmartAquapark.Application.Validators;
using SmartAquapark.Application.Interfaces;
using SmartAquapark.Infrastructure.Services;
var builder = WebApplication.CreateBuilder(args);

builder.Services.AddFluentValidationAutoValidation();

builder.Services.AddValidatorsFromAssemblyContaining<CreateZoneDtoValidator>();

builder.Services.AddControllers();

builder.Services.AddDbContext<AquaparkDbContext>(options =>
    options.UseNpgsql(
        builder.Configuration.GetConnectionString("DefaultConnection")));
builder.Services.AddScoped<IZoneService, ZoneService>();
builder.Services.AddScoped<IAuthService, AuthService>();
builder.Services.AddEndpointsApiExplorer();

builder.Services.AddSwaggerGen();

var app = builder.Build();

app.UseSwagger();

app.UseSwaggerUI();

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();