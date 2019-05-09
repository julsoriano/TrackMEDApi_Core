using Microsoft.Extensions.DependencyInjection;

using TrackMEDApi.Models;
using TrackMEDApi.Services;

namespace TrackMEDApi.Extensions
{
    public static class ServiceExtensions
    {
        public static IServiceCollection RegisterServices(
            this IServiceCollection services)
        {
            // Add application services.
            services.AddTransient<IEmailSender, AuthMessageSender>();
            services.AddTransient<ISmsSender, AuthMessageSender>();

            services.AddSingleton<IEntityRepository<ActivityType>, EntityRepository<ActivityType>>();
            services.AddSingleton<IEntityRepository<Component>, EntityRepository<Component>>();
            services.AddSingleton<IEntityRepository<Deployment>, EntityRepository<Deployment>>();
            services.AddSingleton<IEntityRepository<Description>, EntityRepository<Description>>();
            services.AddSingleton<IEntityRepository<EquipmentActivity>, EntityRepository<EquipmentActivity>>();
            services.AddSingleton<IEntityRepository<Location>, EntityRepository<Location>>();
            services.AddSingleton<IEntityRepository<Model_Manufacturer>, EntityRepository<Model_Manufacturer>>();
            services.AddSingleton<IEntityRepository<Owner>, EntityRepository<Owner>>();
            services.AddSingleton<IEntityRepository<ProviderOfService>, EntityRepository<ProviderOfService>>();
            services.AddSingleton<IEntityRepository<Status>, EntityRepository<Status>>();
            services.AddSingleton<IEntityRepository<SystemsDescription>, EntityRepository<SystemsDescription>>();
            services.AddSingleton<IEntityRepository<SystemTab>, EntityRepository<SystemTab>>();

            // Add all other services here.
            return services;
        }
    }
}
