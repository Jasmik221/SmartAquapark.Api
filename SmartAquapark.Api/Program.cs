using Microsoft.EntityFrameworkCore;
using SmartAquapark.Infrastructure.Persistence;
using FluentValidation;
using FluentValidation.AspNetCore;
using SmartAquapark.Application.Validators;
var builder = WebApplication.CreateBuilder(args);

builder.Services.AddFluentValidationAutoValidation();

builder.Services.AddValidatorsFromAssemblyContaining<CreateZoneDtoValidator>();

builder.Services.AddControllers();

builder.Services.AddDbContext<AquaparkDbContext>(options =>
    options.UseNpgsql(
        builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddEndpointsApiExplorer();

builder.Services.AddSwaggerGen();

var app = builder.Build();

app.UseSwagger();

app.UseSwaggerUI();

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();