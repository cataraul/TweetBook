global using TweetBook.Data;
global using TweetBook.Services;
global using Microsoft.EntityFrameworkCore;
using TweetBook.Options;
using Microsoft.AspNetCore.Identity;
using FluentValidation.AspNetCore;
using TweetBook.Filters;
using Swashbuckle.AspNetCore.Filters;
using TweetBook.Installers;

var builder = WebApplication.CreateBuilder(args);

builder.InstallServicesInAssembly();

builder.Services.AddScoped<IIdentityService, IdentityService>();
builder.Services.AddControllers();
builder.Services.AddMvc(options =>
{
    options.Filters.Add<ValidationFilter>();
})
    .AddFluentValidation(mvcConfiguration=>mvcConfiguration.RegisterValidatorsFromAssemblyContaining<Program>());

//Bearer Token Configuration
var jwtSettings = new JwtSettings();
builder.Configuration.Bind(key:nameof(jwtSettings),jwtSettings);

//Automapper
builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

builder.Services.AddEndpointsApiExplorer();

builder.Services.AddSwaggerExamplesFromAssemblyOf<Program>();
var app = builder.Build();

//Adding Roles
var serviceScope = app.Services.CreateScope();

var dbContext = serviceScope.ServiceProvider.GetRequiredService<DataContext>();
await dbContext.Database.MigrateAsync();
var roleManager = serviceScope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();

if (!await roleManager.RoleExistsAsync("Admin"))
{
    var adminRole = new IdentityRole("Admin");
    await roleManager.CreateAsync(adminRole);
}

if (!await roleManager.RoleExistsAsync("Poster"))
{
    var posterRole = new IdentityRole("Poster");
    await roleManager.CreateAsync(posterRole);
}

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();
app.UseAuthentication();

app.MapControllers();

app.Run();
