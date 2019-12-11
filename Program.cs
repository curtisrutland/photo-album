using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.DependencyInjection;
using PhotoAlbum.Services;
using Microsoft.Extensions.Logging;

namespace PhotoAlbum
{
  public class Program
    {
        public static async Task Main(string[] args)
        { 
            var host = CreateHostBuilder(args).Build();
            using(var serviceScope = host.Services.CreateScope())
            {
                var albumService = serviceScope.ServiceProvider.GetRequiredService<IAlbumService>();
                var logger = serviceScope.ServiceProvider.GetRequiredService<ILogger<Program>>();
                logger.LogInformation("Preloading image hashes...");
                albumService.GetAlbums();
            }
            await host.RunAsync();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                });
    }
}
