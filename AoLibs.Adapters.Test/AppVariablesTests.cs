using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using AoLibs.Adapters.Core;
using AoLibs.Adapters.Core.Interfaces;
using GalaSoft.MvvmLight.Helpers;
using Moq;
using Xunit;

namespace AoLibs.Adapters.Test
{
    public class AppVariablesTests
    {
        private class TestData
        {
            public string Hello { get; set; } = "Test";
        }

        private class TestAppVariables : AppVariablesBase
        {
            public TestAppVariables(ISettingsProvider settingsProvider, IDataCache dataCache = null) 
                : base(settingsProvider, dataCache)
            {
            }

            public TestAppVariables(ISyncStorage syncStorage, IAsyncStorage asyncStorage = null) 
                : base(syncStorage, asyncStorage)
            {
            }

            [Variable(CustomKey = "test.json", ExpirationTime = 1)]
            public Holder<TestData> TestData { get; set; }

            [Variable(CustomKey = "test2.json", MemoryOnly = true)]
            public Holder<TestData> TestData2 { get; set; }
        }

        [Fact]
        public async Task TestAppVariablesSavingAndLoading()
        {
            // Arrange
            var settingsMock = new Mock<ISettingsProvider>();
            var filestorageMock = new Mock<IFileStorageProvider>();

            var dataCache = new DataCache(filestorageMock.Object);
            var appVariables = new TestAppVariables(settingsMock.Object, dataCache);

            // Act
            var emptyData = appVariables.TestData.Value;
            appVariables.TestData.Value = new TestData {Hello = "Test"};
            var readData = appVariables.TestData.Value;

            var emptyAsyncData = await appVariables.TestData2.GetAsync();
            await appVariables.TestData2.SetAsync(new TestData {Hello = "Test"});
            var readAsyncData = await appVariables.TestData2.GetAsync();

            // Assert
            Assert.Null(emptyData);
            Assert.NotNull(readData);
            settingsMock.Verify(
                provider => provider.SetString(It.Is<string>(s => s == "test.json"), It.IsAny<string>()));
            settingsMock.Verify(
                provider => provider.GetString(It.Is<string>(s => s == "test.json")));

            Assert.Null(emptyAsyncData);
            Assert.NotNull(readAsyncData);
            filestorageMock.Verify(
                provider => provider.WriteText(It.Is<string>(s => s == "test.json"), It.IsAny<string>()), Times.Never);
            filestorageMock.Verify(
                provider => provider.ReadTextAsync(It.Is<string>(s => s == "test.json")), Times.Never);
        }

        [Fact]
        public async Task TestAppVariablesWithoutAsyncStorage()
        {
            // Arrange
            var settingsMock = new Mock<ISettingsProvider>();

            var appVariables = new TestAppVariables(settingsMock.Object, null);

            // Act
            var emptyData = appVariables.TestData.Value;
            appVariables.TestData.Value = new TestData {Hello = "Test"};
            var readData = appVariables.TestData.Value;

            await Assert.ThrowsAsync<InvalidOperationException>(async () => 
                await appVariables.TestData2.GetAsync());
            await Assert.ThrowsAsync<InvalidOperationException>(async () =>
                await appVariables.TestData2.SetAsync(new TestData {Hello = "Test"}));

            var emptyAsyncData = appVariables.TestData2.Value;

            // Assert
            Assert.Null(emptyData);
            Assert.NotNull(readData);
            settingsMock.Verify(
                provider => provider.SetString(It.Is<string>(s => s == "test.json"), It.IsAny<string>()));
            settingsMock.Verify(
                provider => provider.GetString(It.Is<string>(s => s == "test.json")));

            Assert.Null(emptyAsyncData);
        }

        [Fact]
        public async Task TestAppVariablesExpiredData()
        {
            // Arrange
            var settingsMock = new Mock<ISettingsProvider>();
            var filestorageMock = new Mock<IFileStorageProvider>();

            var dataCache = new DataCache(filestorageMock.Object);
            var appVariables = new TestAppVariables(settingsMock.Object, dataCache);

            var savedJson = string.Empty;
            filestorageMock.Setup(provider => provider.WriteText(It.IsAny<string>(), It.IsAny<string>()))
                .Callback<string, string>((s, s1) => savedJson = s1);
            filestorageMock.Setup(provider => provider.ReadTextAsync(It.IsAny<string>()))
                .Returns(() => Task.FromResult(savedJson));

            // Act
            await appVariables.TestData.SetAsync(new TestData());
            var readDataBeforeExpiration = await appVariables.TestData.GetAsync();
            await Task.Delay(1100);
            var readDataAfterExpiration = await appVariables.TestData.GetAsync();

            // Assert
            Assert.NotNull(readDataBeforeExpiration);
            Assert.Null(readDataAfterExpiration);
        }
    }
}
