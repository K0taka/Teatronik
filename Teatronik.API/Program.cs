using Microsoft.EntityFrameworkCore;
using Teatronik.Application;
using Teatronik.Core.Interfaces;
using Teatronik.Infrastructure;
using Teatronik.Infrastructure.Repositories;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddDbContext<TeatronikDbContext>(
    options =>
    {
        options.UseNpgsql(builder.Configuration.GetConnectionString("DataBase"));
    });

builder.Services.AddScoped<IComponentModelService, ComponentModelService>();
builder.Services.AddScoped<IComponentModelRepository, ComponentModelRepository>();

builder.Services.AddScoped<IComponentService, ComponentService>();
builder.Services.AddScoped<IComponentRepository, ComponentRepository>();

builder.Services.AddScoped<IEventService, EventService>();
builder.Services.AddScoped<IEventRepository, EventRepository>();

builder.Services.AddScoped<IKindService, KindService>();
builder.Services.AddScoped<IKindRepository, KindRepository>();

builder.Services.AddScoped<IPropService, PropService>();
builder.Services.AddScoped<IPropRepository, PropRepository>();

builder.Services.AddScoped<IPropSchemaService, PropSchemaService>();
builder.Services.AddScoped<IPropSchemaRepository, PropSchemaRepository>();

builder.Services.AddScoped<IRoleService, RoleService>();
builder.Services.AddScoped<IRoleRepository, RoleRepository>();

builder.Services.AddScoped<ISeasonService, SeasonService>();
builder.Services.AddScoped<ISeasonRepository, SeasonRepository>();

builder.Services.AddScoped<ITypeService, TypeService>();
builder.Services.AddScoped<ITypeRepository, TypeRepository>();

builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<IUserRepository, UserRepository>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
