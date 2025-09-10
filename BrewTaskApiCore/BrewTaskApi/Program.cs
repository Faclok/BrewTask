using Asp.Versioning;
using BrewTaskApi.Database.Contexts;
using BrewTaskApi.Database.Extensions;
using BrewTaskApi.JWT;
using BrewTaskApi.Swagger;
using BrewTaskApi.V1.Services.Extensions;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace BrewTaskApi
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddControllers();

            // context
            builder.Services.AddDbContext<BrewTaskContext>(options => {
                options.UseNpgsql(builder.Configuration["CONNECTION_DB"]);
            });

            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddSwaggerOptionsBrewTask();

            // Jwt
            builder.Services.AddJWTokenBrewTask(builder.Configuration);

            // Versioning
            builder.Services.AddApiVersioning(options =>
            {
                options.DefaultApiVersion = new ApiVersion(1, 0);
                options.AssumeDefaultVersionWhenUnspecified = true;
                options.ReportApiVersions = true;
            }).AddApiExplorer(options =>
            {
                options.GroupNameFormat = "'v'VVV";
                options.SubstituteApiVersionInUrl = true;
            });

            // Add Business Services
            builder.Services.AddBusinessServicesV1();

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger(c =>
                {
                    c.OpenApiVersion = Microsoft.OpenApi.OpenApiSpecVersion.OpenApi2_0;
                });
                app.UseSwaggerUI(options =>
                {
                    var descriptions = app.DescribeApiVersions();

                    // build a swagger endpoint for each discovered API version
                    foreach (var description in descriptions)
                    {
                        var url = $"/swagger/{description.GroupName}/swagger.json";
                        var name = description.GroupName.ToUpperInvariant();
                        options.SwaggerEndpoint(url, name);
                    }
                });
            }

            var installMigration = builder.Configuration["INSTALL_MIGRATION"];
            if (!string.IsNullOrWhiteSpace(installMigration) && installMigration == "true")
                await app.MigrateDatabaseAsync<BrewTaskContext>();

            app.UseMiddleware<JWTMiddleware>();

            app.UseHttpsRedirection();
            app.UseAuthorization();
            app.MapControllers();

         

            await app.RunAsync();
        }
    }
}
