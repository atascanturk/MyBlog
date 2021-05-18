using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using ProgrammersBlog.Entities.Concrete;
using ProgrammersBlog.Mvc.AutoMapper.Profiles;
using ProgrammersBlog.Mvc.Filters;
using ProgrammersBlog.Mvc.Helpers.Abstract;
using ProgrammersBlog.Mvc.Helpers.Concrete;
using ProgrammersBlog.Services.AutoMapper.Profiles;
using ProgrammersBlog.Services.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace ProgrammersBlog.Mvc
{
    public class Startup
    {
        public IConfiguration _configuration;

        public Startup(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            services.Configure<AboutUsPageInfo>(_configuration.GetSection("AboutUsPageInfo")); //appsettings json'dan veri okumak i�in gerekli komut.
            services.Configure<WebsiteInfo>(_configuration.GetSection("WebsiteInfo")); //appsettings json'dan veri okumak i�in gerekli komut.
            services.AddControllersWithViews(options =>
            {
                options.ModelBindingMessageProvider.SetValueMustNotBeNullAccessor(value => "Bu alan bo� ge�ilmemelidir.");
                options.Filters.Add<MvcExceptionFilter>(); //filtre eklemek i�in gerekli komut.            


            }).AddRazorRuntimeCompilation().AddJsonOptions(opt=> {
                opt.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter()); //resultstatus "success" gibi al�nacaksa jsonnamingpolicy de kullan�labilir 
                opt.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.Preserve;
            }).AddNToastNotifyToastr();
            //mvc yap�s� i�in //razor runtime nugetten indirildi,
            //sayfadaki de�i�iklikleri anl�k olarak g�r�nt�lemek i�in (razorruntime)
            services.AddSession();
            services.AddAutoMapper(typeof(CategoryProfile),typeof(ArticleProfile),typeof(UserProfile),typeof(ViewModelsProfile),typeof(CommentProfile));
            services.LoadMyServices(connectionString:_configuration.GetConnectionString("LocalDB"));
            services.AddScoped<IImageHelper, ImageHelper>();
            services.ConfigureApplicationCookie(options =>
            {
                options.LoginPath = new PathString("/Admin/Auth/Login");
                options.LogoutPath = new PathString("/Admin/Auth/Logout");
                options.Cookie = new CookieBuilder
                {
                    Name = "ProgrammersBlog",
                    HttpOnly = true,
                    SameSite = SameSiteMode.Strict,
                    SecurePolicy = CookieSecurePolicy.SameAsRequest //Always
                };
                options.SlidingExpiration = true;
                options.ExpireTimeSpan = System.TimeSpan.FromDays(7);
                options.AccessDeniedPath = new PathString("/Admin/Auth/AccessDenied");

            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseStatusCodePages(); // 404 hatas� i�in
            }

            app.UseSession();
            app.UseStaticFiles();
            app.UseRouting();
            app.UseAuthentication(); // Kimlik kontrol�
            app.UseAuthorization(); // Yetki kontrol�
            app.UseNToastNotify();
            app.UseEndpoints(endpoints =>
            {
                 endpoints.MapAreaControllerRoute( //tek area kullan�laca�� i�in ba�ka uygulama olmayacaksa mapcontrollerroute
                        name: "Admin",
                        areaName: "Admin",
                        pattern: "Admin/{controller=Home}/{action=Index}/{id?}"
                        );
                    endpoints.MapDefaultControllerRoute(); // home/index'e gitsin diye.
                   
                });
           
        }
    }
}
