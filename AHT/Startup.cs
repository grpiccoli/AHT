using System;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using AHT.Areas.Identity;
using AHT.Data;
using AHT.Services;
using AHT.Services.Interfaces;
using WebEssentials.AspNetCore.Pwa;
using AHT.Models;
using Microsoft.Net.Http.Headers;
using Microsoft.AspNetCore.StaticFiles;
using Microsoft.AspNetCore.Identity;

namespace AHT
{
    public class Startup
    {
        private readonly string _os;
        //private const string defaultCulture = "es";
        //private readonly CultureInfo[] supportedCultures;
        public Startup(IConfiguration configuration)
        {
            //supportedCultures = new[]
            //    {
            //        new CultureInfo(defaultCulture)
            //    };
            Configuration = configuration;
            _os = Environment.OSVersion.Platform.ToString();
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddDbContext<ApplicationDbContext>(options =>
                options.UseSqlServer(
                    Configuration.GetConnectionString($"{_os}Connection"),
                    sqlServerOptions => sqlServerOptions.CommandTimeout(10000)));
            services.AddIdentity<ApplicationUser, ApplicationRole>(options => 
            options.SignIn.RequireConfirmedAccount = true)
                .AddEntityFrameworkStores<ApplicationDbContext>()
                .AddDefaultTokenProviders()
                .AddDefaultUI()
                .AddErrorDescriber<SpanishIdentityErrorDescriber>();
            services.AddRazorPages();
            services.AddControllersWithViews();
            services.AddAntiforgery(options =>
            {
                options.FormFieldName = "__RequestVerificationToken";
                options.HeaderName = "X-CSRF-TOKEN";
            });
            services.Configure<EmailSettings>(o => o.SendGridKey = Configuration["SG_KEY"]);
            services.Configure<FlowSettings>(o =>
            {
                var dev = true;
                var flowEnv = dev ? "Sandbox" : "Production";
                var preffix = dev ? "sandbox" : "www";
                o.ApiKey = Configuration[$"Flow:{flowEnv}:ApiKey"];
                o.SecretKey = Configuration[$"Flow:{flowEnv}:SecretKey"];
                o.Currency = "UF";
                o.EndPoint = new Uri($"https://{preffix}.flow.cl/api");
            });
            //services.AddScoped<IFlow, FlowService>();

            //services.AddTransient<IEmailSender, EmailSender>();
            services.AddSingleton<IFlow, FlowService>();
            services.AddSingleton<IEmailSender, EmailSender>();
            services.AddSingleton<IViewRenderService, ViewRenderService>();
            services.AddServerSideBlazor();
            services.AddScoped<AuthenticationStateProvider, RevalidatingIdentityAuthenticationStateProvider<ApplicationUser>>();
            services.AddSingleton<WeatherForecastService>();
            services.AddProgressiveWebApp(new PwaOptions { EnableCspNonce = true });
            //services.AddCors();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseDatabaseErrorPage();
            }
            else
            {
                app.UseExceptionHandler("/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }
            FileExtensionContentTypeProvider provider = new FileExtensionContentTypeProvider();
            provider.Mappings[".webmanifest"] = "application/manifest+json";

            //app.UseSitemapMiddleware();
            //app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();
            //app.UseCors();
            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
                endpoints.MapBlazorHub();
                endpoints.MapFallbackToPage("/_Host");
            });
        }
    }
}
