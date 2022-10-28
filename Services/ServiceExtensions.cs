using Microsoft.Extensions.DependencyInjection;

namespace TenantAPI.Services
{
    public static class ServiceExtensions
    {
        public static void ConfigureServices(this IServiceCollection services)
        {
            services.AddSingleton<ILoggerManager, LoggerManager>();
            services.AddScoped<IAddressBuilder, AddressBuilder>();
            services.AddScoped<IAddressTypeBuilder, AddressTypeBuilder>();
            services.AddScoped<IAuthProviderBuilder, AuthProviderBuilder>();
            services.AddScoped<IInfoBuilder, InfoBuilder>();
            services.AddScoped<IRoleBuilder, RoleBuilder>();
            services.AddScoped<IStateBuilder, StateBuilder>();
            services.AddScoped<ITenantBuilder, TenantBuilder>();
            services.AddScoped<IUserBuilder, UserBuilder>();
            services.AddScoped<IUserRoleBuilder, UserRoleBuilder>();
            services.AddScoped<IUserTitleBuilder, UserTitleBuilder>();
        }
    }
}
