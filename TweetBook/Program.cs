global using Microsoft.EntityFrameworkCore;
global using TweetBook.Data;
global using TweetBook.Services;
using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.AspNetCore.Identity;
using Newtonsoft.Json;
using Swashbuckle.AspNetCore.Filters;
using Tweetbook.Contracts.HealthChecks;
using TweetBook.Filters;
using TweetBook.Installers;
using TweetBook.Options;

var builder = WebApplication.CreateBuilder(args);

builder.InstallServicesInAssembly();

static void JwtConfiguration(WebApplicationBuilder? builder)
{
    var jwtSettings = new JwtSettings();

    builder.Configuration.Bind(key: nameof(jwtSettings), jwtSettings);
}

builder.Services.AddScoped<IIdentityService, IdentityService>();
builder.Services.AddMvc(options =>
{
    options.Filters.Add<ValidationFilter>();
})
.AddFluentValidation(mvcConfiguration => mvcConfiguration.RegisterValidatorsFromAssemblyContaining<Program>());


builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
builder.Services.AddSwaggerExamplesFromAssemblyOf<Program>();
var app = builder.Build();

//Adding Roles
static async Task AddRolesAsync(WebApplication? app)
{
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
}


// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
app.UseHealthChecks("/health", new HealthCheckOptions
{
    ResponseWriter = async (context, report) =>
    {
        context.Response.ContentType = "application/json";

        var response = new HealthCheckResponse
        {
            Status = report.Status.ToString(),
            Checks = report.Entries.Select(x => new HealthCheck
            {
                Component = x.Key,
                Status = x.Value.Status.ToString(),
                Description = x.Value.Description,
            }),
            Duration = report.TotalDuration
        };

        await context.Response.WriteAsync(JsonConvert.SerializeObject(response));
    }
});
app.UseHttpsRedirection();

app.UseAuthorization();
app.UseAuthentication();

app.MapControllers();

app.Run();

// Make the implicit Program class public so test projects can access it
public partial class Program { }
