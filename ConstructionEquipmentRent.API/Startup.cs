using ConstructionEquipmentRent.API.Options;
using ConstructionEquipmentRent.API.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace ConstructionEquipmentRent.API
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
            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_2);

            services.AddCors();

            services.AddMemoryCache();

            services.AddOptions();
            services.Configure<FileStockRepositoryOptions>(Configuration.GetSection("FileStockRepository"));
            services.Configure<FileRentalFeesProviderOptions>(Configuration.GetSection("FileRentalFeesProvider"));

            services.AddSingleton<IStockRepository, FileStockRepository>();
            services.AddSingleton<IOrderRepository, InMemoryOrderRepository>();
            services.AddSingleton<IRentalFeesProvider, FileRentalFeesProvider>();
            services.AddSingleton<IOrderItemPriceCalculator, OrderItemPriceCalculator>();
            services.AddSingleton<IOrderItemBonusCalculator, OrderItemBonusCalculator>();
            services.AddSingleton<IInvoiceGenerator, InvoiceGenerator>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseCors(options => options
                .AllowAnyOrigin()
                .AllowAnyHeader()
                .AllowAnyMethod());

            app.UseMvc();
        }
    }
}
