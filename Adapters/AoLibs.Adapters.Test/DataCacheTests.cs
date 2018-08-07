using System;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using AoLibs.Adapters.Core;
using AoLibs.Adapters.Core.Excpetions;
using AoLibs.Adapters.Core.Interfaces;
using Moq;
using Newtonsoft.Json;
using Xunit;
using Xunit.Sdk;

namespace AoLibs.Adapters.Test
{
    public class DataCacheTests
    {
        class TestData
        {
            public string Hello { get; set; } = "Test";
        }

        [Fact]
        public async Task TestDataCacheDataSavingAndRetrieving()
        {
            //Arrange
            var filestorageMock = new Mock<IFileStorageProvider>();
            var savedJson = string.Empty;
            filestorageMock.Setup(provider => provider.WriteText(It.IsAny<string>(), It.IsAny<string>()))
                .Callback<string, string>((s, s1) => savedJson = s1);
            filestorageMock.Setup(provider => provider.ReadTextAsync(It.IsAny<string>()))
                .Returns(() => Task.FromResult(savedJson));

            var dataCache = new DataCache(filestorageMock.Object);
            var testSaveData = new TestData { Hello = "Test" };
            //Act
            dataCache.SaveData("test.json", testSaveData);
            var retrievedSaveTestData = await dataCache.RetrieveData<TestData>("test.json");

            //Assert
            filestorageMock.Verify(provider => provider
                .WriteText(
                    It.Is<string>(s => s == "test.json"),
                    It.IsAny<string>()));

            filestorageMock.Verify(provider => provider
                .ReadTextAsync(
                    It.Is<string>(s => s == "test.json")));

            Assert.Equal(testSaveData.Hello, retrievedSaveTestData.Hello);
        }

        [Fact]
        public async Task TestDataCacheExpiredDataRetrieveing()
        {
            //Arrange
            var filestorageMock = new Mock<IFileStorageProvider>();
            var savedJson = string.Empty;
            filestorageMock.Setup(provider => provider.WriteText(It.IsAny<string>(), It.IsAny<string>()))
                .Callback<string, string>((s, s1) => savedJson = s1);
            filestorageMock.Setup(provider => provider.ReadTextAsync(It.IsAny<string>()))
                .Returns(() => Task.FromResult(savedJson));

            var dataCache = new DataCache(filestorageMock.Object);
            var testSaveData = new TestData { Hello = "Test" };

            //Act
            dataCache.SaveData("test.json", testSaveData);
            await Task.Delay(10);

            //Assert
            await Assert.ThrowsAsync<DataExpiredException>(async () =>
                await dataCache.RetrieveData<TestData>("test.json", TimeSpan.FromMilliseconds(1)));
        }
    }
}
