using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using NgrokAspNetCore;

namespace VOU.Web.Host.Startup
{
    public class Program
    {
        public static void Main(string[] args)
        {
            //BuildWebHost(args).Run();
            var host = BuildWebHost(args);
            //host.StartNgrokAsync();
            host.Run();
        }

        public static IWebHost BuildWebHost(string[] args)
        {
            return WebHost.CreateDefaultBuilder(args)
                .UseStartup<Startup>()
                .Build();
        }
    }
}
