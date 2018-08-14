using System;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using AoLibs.Adapters.Core.Excpetions;
using AoLibs.Adapters.Core.Interfaces;
using Newtonsoft.Json;

[assembly: InternalsVisibleTo("AoLibs.Adapters.Test")]
namespace AoLibs.Adapters.Core
{
    /// <summary>
    /// Utility class allowing to quickly store data within filesystem.
    /// </summary>
    public class DataCache : IDataCache
    {
        internal class TimedHolder<T>
        {
            public T Value { get; set; }
            public DateTime CreatedAt { get; set; }
        }

        private readonly IFileStorageProvider _fileStorageProvider;

        public DataCache(IFileStorageProvider fileStorageProvider)
        {
            _fileStorageProvider = fileStorageProvider;
        }

        /// <summary>
        /// Reads the data from given path and deserializes it taking given expirationTime in consideration.
        /// </summary>
        /// <typeparam name="T">Data Type to retireve.</typeparam>
        /// <param name="path">Path to the file. Can be just filename.</param>
        /// <param name="expiration">Specifies how much time could have passed since last write.</param>
        /// <returns>Deserialized data or default if file does not exist or is malformed.</returns>
        /// <exception cref="DataExpiredException"> Thrown when data is expired.</exception>
        public async Task<T> RetrieveData<T>(string path, TimeSpan? expiration = null)
        {
            try
            {
                var json = await _fileStorageProvider.ReadTextAsync(path);
                var holder = JsonConvert.DeserializeObject<TimedHolder<T>>(json);

                if (expiration != null && DateTime.UtcNow - holder.CreatedAt > expiration)
                    throw new DataExpiredException($"Data stored in {path} is expired as per provided expiration time {expiration}");

                return holder.Value;
            } // file not exists or malformed
            catch (Exception e) when(!(e is DataExpiredException)) 
            {
                return default;
            }
        }

        /// <summary>
        /// Saves data in fire and forget manner.
        /// </summary>
        /// <typeparam name="T">Type of data to save.</typeparam>
        /// <param name="path">Path to the file. Can be just filename.</param>
        /// <param name="data">The data to store.</param>
        public async void SaveData<T>(string path, T data)
        {
            await SaveDataAsync(path, data);
        }

        /// <summary>
        /// Saves data in asynchronous matter.
        /// </summary>
        /// <typeparam name="T">Type of data to save.</typeparam>
        /// <param name="path">Path to the file. Can be just filename.</param>
        /// <param name="data">The data to store.</param>
        public async Task SaveDataAsync<T>(string path, T data)
        {
            var json = JsonConvert.SerializeObject(new TimedHolder<T>
            {
                CreatedAt = DateTime.UtcNow,
                Value = data
            });
            _fileStorageProvider.WriteText(path, json);
        }

        /// <summary>
        /// Clears storage.
        /// </summary>
        /// <param name="path">File to remove.</param>
        public async Task Clear(string path)
        {
            _fileStorageProvider.RemoveFile(path);
        }
    }
}
