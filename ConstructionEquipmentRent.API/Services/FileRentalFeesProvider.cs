using ConstructionEquipmentRent.API.Models;
using ConstructionEquipmentRent.API.Options;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;

namespace ConstructionEquipmentRent.API.Services
{
    public class FileRentalFeesProvider : IRentalFeesProvider
    {
        private readonly string jsonFilePath;
        private readonly TimeSpan cacheDuration;
        private readonly IMemoryCache cache;
        private readonly ILogger logger;
        
        public FileRentalFeesProvider(
            IOptions<FileRentalFeesProviderOptions> options,
            IMemoryCache cache,
            IHostingEnvironment env,
            ILogger<FileStockRepository> logger)
        {
            jsonFilePath = Path.Join(env.ContentRootPath, options.Value.JsonFilePath);
            cacheDuration = TimeSpan.FromSeconds(options.Value.CacheDuration);

            this.cache = cache;
            this.logger = logger;
        }

        public Task<RentalFees> Get()
        {
            return cache.GetOrCreateAsync(nameof(LoadRentalFeesFromFile), cacheEntry =>
            {
                logger.LogInformation("Invalidating rental fees cache");
                cacheEntry.AbsoluteExpirationRelativeToNow = cacheDuration;
                return LoadRentalFeesFromFile();
            });
        }
        
        private async Task<RentalFees> LoadRentalFeesFromFile()
        {
            var jsonContent = await LoadRentalFeesJsonFromFile();
            return ParseRentalFeesJson(jsonContent);
        }

        private async Task<string> LoadRentalFeesJsonFromFile()
        {
            try
            {
                return await File.ReadAllTextAsync(jsonFilePath);
            }
            catch (Exception e)
            {
                logger.LogError(e, "Unable to read rental fees file!");
                return null;
            }
        }

        private RentalFees ParseRentalFeesJson(string jsonContent)
        {
            try
            {
                // TODO: test NULL
                return JsonConvert.DeserializeObject<RentalFees>(jsonContent);
            }
            catch (Exception e)
            {
                logger.LogError(e, "Unable to parse rental fees file content!");
                return null;
            }
        }

    }
}
