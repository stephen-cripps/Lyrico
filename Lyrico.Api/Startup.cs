using AutoMapper;
using Lyrico.Application;
using Lyrico.Application.Services;
using Lyrico.Lyricsovh;
using Lyrico.MusicBrainz;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace Lyrico.Api
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

            var assembly = typeof(GetLyricStats).Assembly;
            services.AddMediatR(assembly);

            services.AddTransient<IArtistService, MusicBrainzService>()
                .AddTransient<ILyricService, LyricsOvhService>();

            services.Configure<MusicBrainz.DependencyInjection.Options>(Configuration.GetSection("MusicBrainz"));
            services.Configure<Lyricsovh.Options>(Configuration.GetSection("LyricsOvh"));

            services.AddAutoMapper(assembly);
            var mappingConfig = new MapperConfiguration(mc =>
            {
                mc.AddProfile(new MusicBrainz.DependencyInjection.MappingProfile());
            });
            services.AddSingleton(mappingConfig.CreateMapper());
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
