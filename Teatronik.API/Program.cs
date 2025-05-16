using Microsoft.EntityFrameworkCore;
using Teatronik.Application;
using Teatronik.Core.Enums;
using Teatronik.Core.Interfaces;
using Teatronik.Infrastructure;
using Teatronik.Infrastructure.Entities;
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

builder.Services.AddControllers()
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.Converters.Add(new DateOnlyJsonConverter());
    });

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    var dbContext = services.GetRequiredService<TeatronikDbContext>();
    
    var included = await dbContext.Roles.Select(e => e.RoleName).ToListAsync();
    var allRoles = Enum.GetValues(typeof(RoleType)).Cast<RoleType>();

    foreach (var role in allRoles)
    {
        if (included.Contains(role.ToString()))
            continue;
        await dbContext.Roles.AddAsync(new RoleEntity
        {
            Id = (int)role,
            RoleName = role.ToString(),
        });
    }

    dbContext.SaveChanges();
}

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