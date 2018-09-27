using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FinalWorkshop.Context;
using FinalWorkshop.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace FinalWorkshop
{
	public class Startup
	{
		protected IConfigurationRoot Configuration;
		public Startup()
		{
			var configurationBuilder = new ConfigurationBuilder();
			configurationBuilder.AddXmlFile("appsettings.xml");
			Configuration = configurationBuilder.Build();
		}
		// This method gets called by the runtime. Use this method to add services to the container.
		// For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
		public void ConfigureServices(IServiceCollection services)
		{

			services.AddDbContext<EFCContext>(builder =>
			{
				var config = Configuration["ConnectionString"];
				builder.UseSqlServer(config);

			}, ServiceLifetime.Transient);

			services.AddIdentity<IdentityUser, IdentityRole>()
				.AddEntityFrameworkStores<EFCContext>();
			services.AddTransient<CustomerDbService>();
			services.AddTransient<MailService>();
			services.AddMvc();
		}

		// This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
		public void Configure(IApplicationBuilder app, IHostingEnvironment env)
		{
			if (env.IsDevelopment())
			{
				app.UseDeveloperExceptionPage();
			}

			app.UseStaticFiles();
			app.UseAuthentication();
			app.UseMvc(routes =>
			{
				routes.MapRoute(
					name: "something", template: "{controller=Main}/{action=Index}/{id?}");
			});
		}
	}
}
