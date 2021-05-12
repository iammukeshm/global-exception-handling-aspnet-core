using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;

namespace GlobalExceptionHandling.WebApi
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
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "GlobalExceptionHandling.WebApi", Version = "v1" });
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "GlobalExceptionHandling.WebApi v1"));
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();
            app.UseMiddleware<ErrorHandlerMiddleware>();
            //app.UseExceptionHandler(
            //    options =>
            //    {
            //        options.Run(
            //            async context =>
            //            {
            //                context.Response.StatusCode = (int)HttpStatusCode.BadRequest;
            //                context.Response.ContentType = "text/html";
            //                var exceptionObject = context.Features.Get<IExceptionHandlerFeature>();
            //                if (null != exceptionObject)
            //                {
            //                    var errorMessage = $"{exceptionObject.Error.Message}";
            //                    await context.Response.WriteAsync(errorMessage).ConfigureAwait(false);
            //                }
            //            });
            //    }
            //);
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}