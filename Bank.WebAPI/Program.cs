using Microsoft.AspNetCore.Hosting;

namespace Bank.WebAPI
{
    public class Program
    {
        public static void Main(string[] args)
        {
            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
             Host
            .CreateDefaultBuilder(args)
            .ConfigureWebHostDefaults(webBuilder =>
            {
#if DEBUG || QA
                webBuilder.UseEnvironment("Development");
#endif
                webBuilder.UseKestrel();
                webBuilder.UseContentRoot(Directory.GetCurrentDirectory());
                webBuilder.UseStartup<Startup>();
                webBuilder.UseIISIntegration();
            })
            .ConfigureLogging(logging =>
            {
                logging.ClearProviders();
                logging.SetMinimumLevel(LogLevel.Trace);
            });
    }
}