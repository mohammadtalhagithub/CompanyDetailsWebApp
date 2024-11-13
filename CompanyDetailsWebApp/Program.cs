using Microsoft.AspNetCore.Authentication.Cookies;

namespace CompanyDetailsWebApp
{
    public class Program
    {
        public static void Main(string[] args)
        {
            // The builder object allows you to configure several parts of the application before it starts running:
            // The builder.Services property allows you to register services into the Dependency Injection (DI) container.  
            var builder = WebApplication.CreateBuilder(args);
#if true
            var selfhosted = builder.Configuration["SelfHosted"];

            if (string.Equals(selfhosted, "true", StringComparison.OrdinalIgnoreCase))
            {
                builder.WebHost.ConfigureKestrel(opts =>
                {
                    try
                    {
                        var httpport = builder.Configuration.GetSection("HostingServer:Http:Port");
                        opts.ListenAnyIP(int.Parse(httpport.Value));
                        var httpsport = builder.Configuration.GetSection("HostingServer:Https:Port");
                        opts.ListenAnyIP(int.Parse(httpsport.Value), listenOptions =>
                        {
                            listenOptions.Protocols = Microsoft.AspNetCore.Server.Kestrel.Core.HttpProtocols.Http1; // HTTPS port
                        });
                        //opts.ListenAnyIP(int.Parse(httpsport.Value), opts => opts.UseHttps());


                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"Kestrel configuration error: {ex.Message}");
                    }
                });
            }
            else
            {
                builder.WebHost.UseIISIntegration();
            }
#endif
            // Add services to the container.
            builder.Services.AddControllersWithViews();
            builder.Services.AddAuthentication(
                CookieAuthenticationDefaults.AuthenticationScheme)
                .AddCookie(option =>
                {
                    option.LoginPath = "/Access/Login";
                    option.ExpireTimeSpan = TimeSpan.FromSeconds(10);
                });

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Home/Error");
            }
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.MapControllerRoute(
                //  (For Conventional Routing)
                name: "default",
                pattern: "{controller=Access}/{action=Login}/{companyId?}"
                );
            //pattern: "{controller=CompanyInfo}/{action=CompanyDetails}/{companyId=32}");


            //app.MapControllers(); // (For Attribute Routing)


            app.Run();
        }
    }
}
