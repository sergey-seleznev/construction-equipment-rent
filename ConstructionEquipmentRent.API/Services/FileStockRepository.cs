using ConstructionEquipmentRent.API.Models;
using ConstructionEquipmentRent.API.Options;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace ConstructionEquipmentRent.API.Services
{
    public class FileStockRepository : IStockRepository
    {
        private readonly string jsonFilePath;
        private readonly TimeSpan cacheDuration;
        private readonly IMemoryCache cache;
        private readonly ILogger logger;
        
        public FileStockRepository(
            IOptions<FileStockRepositoryOptions> options,
            IMemoryCache cache,
            IHostingEnvironment env,
            ILogger<FileStockRepository> logger)
        {
            jsonFilePath = Path.Join(env.ContentRootPath, options.Value.JsonFilePath);
            cacheDuration = TimeSpan.FromSeconds(options.Value.CacheDuration);

            this.cache = cache;
            this.logger = logger;
        }

        public Task<IEnumerable<StockItem>> GetAll()
        {
            return LoadStockItems();
        }

        public async Task<StockItem> GetById(int id)
        {
            var stockItems = await LoadStockItems();
            return stockItems?.FirstOrDefault(i => i.Id == id);
        }

        private Task<IEnumerable<StockItem>> LoadStockItems()
        {
            return cache.GetOrCreateAsync(nameof(LoadStockItemsFromFile), cacheEntry =>
            {
                logger.LogInformation("Invalidating stock item cache");
                cacheEntry.AbsoluteExpirationRelativeToNow = cacheDuration;
                return LoadStockItemsFromFile();
            });
        }

        private async Task<IEnumerable<StockItem>> LoadStockItemsFromFile()
        {
            var jsonContent = await LoadStockItemsJsonFromFile();
            return ParseStockItemsJson(jsonContent);
        }

        private async Task<string> LoadStockItemsJsonFromFile()
        {
            try
            {
                return await File.ReadAllTextAsync(jsonFilePath);
            }
            catch (Exception e)
            {
                logger.LogError(e, "Unable to read stock items file!");
                return null;
            }
        }

        private IEnumerable<StockItem> ParseStockItemsJson(string jsonContent)
        {
            try
            {
                // TODO: test NULL
                return JsonConvert.DeserializeObject<IEnumerable<StockItem>>(jsonContent);
            }
            catch (Exception e)
            {
                logger.LogError(e, "Unable to parse stock items file content!");
                return null;
            }
        }

    }
}
