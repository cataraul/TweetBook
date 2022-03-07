using Microsoft.AspNetCore.Identity;
using TweetBook.Domain;

namespace TweetBook.Installers
{
    public class DbInstaller : IInstaller
    {
        public void InstallServices(WebApplicationBuilder builder)
        {
            var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
            builder.Services.AddDbContext<DataContext>(options => options.UseSqlite(connectionString));

            builder.Services.AddIdentityCore<IdentityUser>()
                .AddRoles<IdentityRole>()
                .AddEntityFrameworkStores<DataContext>();

            builder.Services.AddScoped<IPostService, PostService>();
            builder.Services.AddScoped<ITagsService<Tag, string>, TagService>();
        }
    }
}
