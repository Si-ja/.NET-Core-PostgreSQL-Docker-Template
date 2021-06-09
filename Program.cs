using System;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;


// Tutorials followed to make this project:
// https://medium.com/@agavatar/webapi-with-net-core-and-postgres-in-visual-studio-code-8b3587d12823
// https://github.com/rajvirtual/docker-aspnetcore-postgresql
// https://zetcode.com/csharp/postgresql/

// Run test build of docker for PostgreSQL with [docker run --name some-postgres -e POSTGRES_PASSWORD=mysecretpassword -d postgres]

namespace dockerapi
{
    public class Program
    {
        public static void Main(string[] args)
        {
            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                });
    }
}
