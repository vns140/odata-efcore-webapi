using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNet.OData.Builder;
using Microsoft.AspNet.OData.Extensions;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using ODataWebApp.Data;
using ODataWebApp.OData;

namespace ODataWebApp
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
            // the database filename is stored in the appsettings.json
            var connectionString = Configuration.GetConnectionString(nameof(ApplicationDbContext));
            services.AddDbContext<ApplicationDbContext>(options =>
                options.UseSqlServer(Configuration.GetConnectionString("DefaultConnection")));

            // declare OWIN middleware as a service
            services.AddSingleton<ODataQueryStringFixer>();

            services.AddOData();

            services.AddMvc();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app,
            IHostingEnvironment env, IServiceProvider serviceProvider)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            // create and populate DB
            serviceProvider.InitializeDb();

            // anchor OWIN middleware in the pipe
            app.UseODataQueryStringFixer();

            // configure OData model
            var builder = new ODataConventionModelBuilder(serviceProvider);
            builder.EntitySet<Person>("Persons").EntityType
                .OrderBy(
                    nameof(Person.Name),
                    nameof(Person.Birthday))
                .Filter(
                    nameof(Person.Name));

            // configure OData routing
            app.UseMvc(routeBuilder =>
                routeBuilder.MapODataServiceRoute("OData", "odata", builder.GetEdmModel()));
        }
    }
}
