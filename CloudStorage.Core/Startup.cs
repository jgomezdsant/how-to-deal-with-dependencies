
using CloudStorage.Core.Services;
using CloudStorage.Core.StorageManagers;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace CloudStorage.Core
{
    public static class Startup
    {
        public static IServiceCollection RegisterCloudStorageCore(this IServiceCollection services, string connectionString) => services
            .AddDbContext<DbCloudStorageContext>(
                            o => o.UseSqlite(connectionString,
                            x => x.MigrationsAssembly("CloudStorage.Core")))
            .AddTransient<IPokemonRepository, PokemonSQLiteRepository>()
            .AddTransient<IPokemonService, PokemonService>()
            .AddTransient<IAzureStorageManager, AzureStorageManager>();
    }
}

