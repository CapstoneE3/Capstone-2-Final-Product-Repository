using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using PantryBackEnd.JwtGenerator;
using Microsoft.EntityFrameworkCore;
using PantryBackEnd.Models;
using PantryBackEnd.Repositories;
using PantryBackEnd.Notification;
namespace PantryBackEnd
{
    public class Startup
    {
        public IConfiguration Configuration { get; }
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }


        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddApplicationInsightsTelemetry();
            services.AddControllersWithViews().AddNewtonsoftJson(options =>
            options.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore);//handles json views
            services.AddDbContext<pantryContext>(options =>
            {
                options.UseNpgsql(Configuration.GetConnectionString("DefaultConnection"));
            });
            // , optionsLifetime: ServiceLifetime.Transient);//connect to database
            services.AddCors();
            services.AddScoped<SendNotification>();
            services.AddScoped<IShoppingList, ShoppingListRepo>();
            services.AddScoped<INotification, NotificationRepo>();
            services.AddScoped<IProduct, ProductRepo>();
            services.AddScoped<IInventoryRepo, InventoryRepo>();
            services.AddScoped<IUserRepo, UserRepo>();
            services.AddScoped<JwtService>();
            services.AddScoped<IRecipe, RecipeRepo>();
            //services.AddHostedService<PushNotfication>();
            services.AddControllers();
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "PantryBackEnd", Version = "v1" });
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "PantryBackEnd v1"));

            }
            app.UseSwagger();
            app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "PantryBackEnd v1"));

            app.UseHttpsRedirection();
            app.UseCors(policy => policy
                .AllowAnyHeader()
                .AllowAnyMethod().SetIsOriginAllowed(origin =>
                {
                    if (string.IsNullOrWhiteSpace(origin)) return false;
                    // Only add this to allow testing with localhost, remove this line in production!
                    if (origin.ToLower().StartsWith("http://localhost")) return true;
                    // Insert your production domain here.
                    if (origin.ToLower().StartsWith("https://handypantry.azurewebsites.net")) return true;
                    return false;
                })
                .AllowCredentials()
            );
            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
