global using TweetBook.Data;
global using TweetBook.Services;
global using Microsoft.EntityFrameworkCore;
using TweetBook.Options;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Microsoft.OpenApi.Models;
using Microsoft.AspNetCore.Identity;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddScoped<IPostService, PostService>();

builder.Services.AddControllers();
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddDbContext<DataContext>(options =>options.UseSqlite(connectionString));

builder.Services.AddScoped<IIdentityService, IdentityService>();
builder.Services.AddMvc();
builder.Services.AddIdentityCore<IdentityUser>().AddEntityFrameworkStores<DataContext>();
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

builder.Services.AddAuthorization(options=>{
    options.AddPolicy("TagViewer", builder => builder.RequireClaim("tags.view", "true"));
});

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(x =>
{
    var security = new Dictionary<string, IEnumerable<string>>
    {
        {"Bearer",new string[0]}
    };
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
});

var app = builder.Build();

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
