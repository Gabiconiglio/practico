using FluentValidation.AspNetCore;
using MediatR;
using Microsoft.EntityFrameworkCore;
using parcialt1.Business.Docentes;
using parcialt1.Data;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddEntityFrameworkNpgsql()
    .AddDbContext<ContextDb>(options => options.UseNpgsql(builder.Configuration.GetConnectionString("ConexionDatabase")));

builder.Services.AddControllers().AddFluentValidation(cfg => cfg.RegisterValidatorsFromAssemblyContaining<GetDocenteBusiness>());


builder.Services.AddMediatR(typeof(GetDocenteBusiness.Manejador).Assembly);

builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(
        policy =>
        {
            policy.AllowAnyHeader()
                .AllowAnyMethod()
                .AllowAnyOrigin();
        });
});

var app = builder.Build();
AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehavior", true);

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();
app.UseCors();

app.MapControllers();

app.Run();
