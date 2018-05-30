using System;
using System.Threading.Tasks;
using AoLibs.Adapters.Core.Excpetions;
using AoLibs.Adapters.Core.Interfaces;
using Newtonsoft.Json;

namespace AoLibs.Adapters.Core
{
    public class DataCache : IDataCache
    {
        class TimedHolder<T>
        {
            public T Value { get; set; }
            public DateTime CreatedAt { get; set; }
        }

        private readonly IFileStorageProvider _fileStorageProvider;

        public DataCache(IFileStorageProvider fileStorageProvider)
        {
            _fileStorageProvider = fileStorageProvider;
        }

        public async Task<T> RetrieveData<T>(string path, TimeSpan? expiration = null)
        {
            try
            {
                var json = await _fileStorageProvider.ReadTextAsync(path);
                var holder = JsonConvert.DeserializeObject<TimedHolder<T>>(json);

                if (expiration != null && DateTime.UtcNow - holder.CreatedAt > expiration)
                    throw new DataExpiredException($"Data stored in {path} is expired as per provided expiration time {expiration}");

                return holder.Value;
            }
            catch (Exception e) when(!(e is DataExpiredException)) //file not exists or malformed
            {
                return default(T);
            }
        }

        public async void SaveData<T>(string path, T data)
        {
            await SaveDataAsync(path, data);
        }

        public async Task SaveDataAsync<T>(string path, T data)
        {
            var json = JsonConvert.SerializeObject(new TimedHolder<T>
            {
                CreatedAt = DateTime.UtcNow,
                Value = data
            });
            _fileStorageProvider.WriteText(path, json);
        }

        public async Task Clear(string path)
        {
            _fileStorageProvider.RemoveFile(path);
        }
    }
}
