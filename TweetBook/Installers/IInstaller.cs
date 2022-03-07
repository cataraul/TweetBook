namespace TweetBook.Installers
{
    public interface IInstaller
    {
        void InstallServices(WebApplicationBuilder builder);
    }
}
