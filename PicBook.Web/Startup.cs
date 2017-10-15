﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Rewrite;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace PicBook.Web
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.Configure<MvcOptions>(options =>
            {
                options.Filters.Add(new RequireHttpsAttribute());
            });

            services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
                .AddCookie(o =>
                {
                    o.LoginPath = new PathString("/Account/Login");
                    o.LogoutPath = new PathString("/Home/Index");
                })
                .AddFacebook(o =>
                    {
                        o.AppId = Configuration["Authentication:Facebook:AppId"];
                        o.AppSecret = Configuration["Authentication:Facebook:AppSecret"];
                        o.Scope.Add("email");
                        o.Fields.Add("name");
                        o.Fields.Add("email");
                        o.SaveTokens = true;
                    })
                .AddGoogle(o =>
                {
                    o.ClientId = Configuration["Authentication:Google:ClientId"];
                    o.ClientSecret = Configuration["Authentication:Google:ClientSecret"];
                    //o.Scope.Add("email");
                    //o.Fields.Add("name");
                    //o.Fields.Add("email");
                    o.SaveTokens = true;
                })
                .AddMicrosoftAccount(o =>
                {
                    o.ClientId = Configuration["Authentication:MicrosoftAccount:ClientId"];
                    o.ClientSecret = Configuration["Authentication:MicrosoftAccount:ClientSecret"];
                    o.SaveTokens = true;
                });

            services.AddMvc();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            var options = new RewriteOptions().AddRedirectToHttps(301, 44301);
            app.UseRewriter(options);
            app.UseAuthentication();

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
            }

            app.UseStaticFiles();

            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=Index}/{id?}");
            });
        }
    }
}
