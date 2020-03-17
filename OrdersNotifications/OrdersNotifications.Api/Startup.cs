using System;
using System.IO;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using OrdersNotifications.Api.Services;
using OrdersNotifications.Library;

namespace OrdersNotifications.Api
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
            services.AddControllers();

            services.AddDbContext<OrdersContext>(options => 
            {
                var rootDirectory = $@"{Environment.GetFolderPath(
                    Environment.SpecialFolder.UserProfile,
                    Environment.SpecialFolderOption.DoNotVerify)}";
                var connectionString = $"DataSource={Path.Combine(rootDirectory, "DBs", "orders.db")}";
                options.UseSqlite(connectionString);
            });
            
            services.AddAzureQueue(Configuration["AzureWebJobsStorage"]);

            services.Configure<EmailSettings>(options => Configuration
                .GetSection("EmailSettings")
                .Bind(options));
            
            services.AddScoped<IOrdersService, OrdersService>();
            services.AddScoped<INotificationsService, NotificationsService>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, 
            IWebHostEnvironment env, 
            OrdersContext ordersContext)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints => { endpoints.MapControllers(); });
        }
    }
}