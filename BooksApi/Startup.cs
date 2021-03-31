using BooksApi.Context;
using BooksApi.Entities;
using BooksApi.Helpers;
using BooksApi.Models;
using BooksApi.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;



namespace BooksApi
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
            services.AddTransient<IWriterFile, WriterFile>();
            services.AddTransient<IHostedService, WriteToFileHostedService>();
            services.AddResponseCaching(); // Para guardar en cach�
            services.AddScoped<MyActionFilter>();
            services.AddAutoMapper(config =>
                    {
                        config.CreateMap<Autor, AutorDTO>();
                        config.CreateMap<Libro, LibroDTO>();
                        config.CreateMap<AutorCreationDTO, Autor>();
                    },
                typeof(Startup)) ;
            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer();
            services.AddTransient<IEmailService, EmalService>(); //Se genera una nueva instancia de la clase
            services.AddControllers().AddNewtonsoftJson(options => options.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore);
            services.AddDbContext<ApplicationDBContext>(options => options.UseSqlServer(Configuration.GetConnectionString("DefaultConnection")));
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseHttpsRedirection();

            app.UseResponseCaching();//Para acpetar caching
            app.UseAuthentication();
            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
