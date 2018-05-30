using System;
using System.Threading.Tasks;
using AoLibs.Adapters.Core.Excpetions;

namespace AoLibs.Adapters.Core.Interfaces
{
    public interface IDataCache
    {
        /// <summary>
        /// Reads file and deserializes given data.
        /// Returns default value of <see cref="T"/> when file doesn't exist.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="path">Path delimeted with "/" with filename at the end. Root is default platfrom's AppData folder.</param>
        /// <param name="expiration">If data was stored for longer than given period <see cref="DataExpiredException"/> will be thrown.</param>
        /// <exception cref="DataExpiredException"></exception>
        /// <returns></returns>
        Task<T> RetrieveData<T>(string path, TimeSpan? expiration = null);
        /// <summary>
        /// Serializes data and stores in in file.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="path">Path delimeted with "/" with filename at the end. Root is default platfrom's AppData folder.</param>
        /// <param name="data">Data to store.</param>
        void SaveData<T>(string path, T data);
        /// <summary>
        /// Serializes data and stores in in file.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="path">Path delimeted with "/" with filename at the end. Root is default platfrom's AppData folder.</param>
        /// <param name="data">Data to store.</param>
        Task SaveDataAsync<T>(string path, T data);
        /// <summary>
        /// Remove file.
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        Task Clear(string path);
    }
}
