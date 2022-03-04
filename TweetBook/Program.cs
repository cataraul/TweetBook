global using TweetBook.Data;
global using TweetBook.Services;
global using Microsoft.EntityFrameworkCore;
using TweetBook.Options;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Microsoft.OpenApi.Models;
using Microsoft.AspNetCore.Identity;
using TweetBook.Authorization;
using Microsoft.AspNetCore.Authorization;
using TweetBook.Domain;
using FluentValidation.AspNetCore;
using System.Reflection;
using TweetBook.Filters;
using Swashbuckle.AspNetCore.Filters;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddScoped<IPostService, PostService>();

var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddDbContext<DataContext>(options =>options.UseSqlite(connectionString));

builder.Services.AddScoped<IIdentityService, IdentityService>();
builder.Services.AddControllers();
builder.Services.AddMvc(options =>
{
    options.Filters.Add<ValidationFilter>();
})
    .AddFluentValidation(mvcConfiguration=>mvcConfiguration.RegisterValidatorsFromAssemblyContaining<Program>());

builder.Services.AddIdentityCore<IdentityUser>()
    .AddRoles<IdentityRole>()
    .AddEntityFrameworkStores<DataContext>();

builder.Services.AddScoped<ITagsService<Tag,string>, TagService>();

//Bearer Token Configuration
var jwtSettings = new JwtSettings();
builder.Configuration.Bind(key:nameof(jwtSettings),jwtSettings);
builder.Services.AddSingleton(jwtSettings);

var tokenValidationParameters = new TokenValidationParameters
{
    ValidateIssuerSigningKey = true,
    IssuerSigningKey = new SymmetricSecurityKey(key: Encoding.ASCII.GetBytes(jwtSettings.Secret)),
    ValidateIssuer = false,
    ValidateAudience = false,
    RequireExpirationTime = false,
    ValidateLifetime = true
};

builder.Services.AddSingleton(tokenValidationParameters);

builder.Services.AddAuthentication(configureOptions: x =>
{
    x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    x.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
    x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
    .AddJwtBearer(x =>
    {
        x.SaveToken = true;
        x.TokenValidationParameters = tokenValidationParameters;
    });

builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("MustWorkForRaul", policy =>
    {
        policy.AddRequirements(new WorksForCompanyRequirement("rauls.com"));
    });
});

builder.Services.AddSingleton<IAuthorizationHandler, WorksForCompanyHandler>();

//Automapper
builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());


builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(x =>
{
    var security = new Dictionary<string, IEnumerable<string>>
    {
        {"Bearer",new string[0]}
    };

    x.ExampleFilters();

    x.AddSecurityDefinition(name: "Bearer", new OpenApiSecurityScheme
    {
        Description= "JWT Authorization header using the bearer scheme",
        Name = "Authorization",
        In=ParameterLocation.Header,
        Type = SecuritySchemeType.ApiKey
    });
    x.AddSecurityRequirement(new OpenApiSecurityRequirement
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
    });

    var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
    var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
    x.IncludeXmlComments(xmlPath);

});

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
