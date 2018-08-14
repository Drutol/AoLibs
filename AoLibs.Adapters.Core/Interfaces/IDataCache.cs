using System;
using System.Threading.Tasks;
using AoLibs.Adapters.Core.Excpetions;

namespace AoLibs.Adapters.Core.Interfaces
{
    /// <summary>
    /// Defines interface of utlity allowing to easily store serialized data in form of JSON files.
    /// </summary>
    public interface IDataCache
    {
        /// <summary>
        /// Reads file and deserializes given data.
        /// Returns default value of <see cref="T"/> when file doesn't exist.
        /// </summary>
        /// <typeparam name="T">The type of data to retieve.</typeparam>
        /// <param name="path">Path delimeted with "/" with filename at the end. Root is default platfrom's AppData folder.</param>
        /// <param name="expiration">If data was stored for longer than given period <see cref="DataExpiredException"/> will be thrown.</param>
        /// <exception cref="DataExpiredException">Thrown when expiration exceeds specified time.</exception>
        /// <returns>Deserialized data.</returns>
        Task<T> RetrieveData<T>(string path, TimeSpan? expiration = null);

        /// <summary>
        /// Serializes data and stores in in file.
        /// </summary>
        /// <typeparam name="T">Type of data to save.</typeparam>
        /// <param name="path">Path delimeted with "/" with filename at the end. Root is default platfrom's AppData folder.</param>
        /// <param name="data">Data to store.</param>
        void SaveData<T>(string path, T data);

        /// <summary>
        /// Serializes data and stores in in file.
        /// </summary>
        /// <typeparam name="T">Type of data to save.</typeparam>
        /// <param name="path">Path delimeted with "/" with filename at the end. Root is default platfrom's AppData folder.</param>
        /// <param name="data">Data to store.</param>
        Task SaveDataAsync<T>(string path, T data);

        /// <summary>
        /// Remove file.
        /// </summary>
        /// <param name="path">The path of the file.</param>
        Task Clear(string path);
    }
}
