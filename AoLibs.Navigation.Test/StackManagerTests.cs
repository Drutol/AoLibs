using System;
using System.Collections.Generic;
using AoLibs.Navigation.Core;
using AoLibs.Navigation.Core.Interfaces;
using AoLibs.Navigation.Core.PageProviders;
using AoLibs.Navigation.Test.Mocks;
using Moq;
using Xunit;

namespace AoLibs.Navigation.Test
{
    public class StackManagerTests
    {
        private Dictionary<PageIndex, IPageProvider<INavigationPage>> _pageDefinitions;

        private TaggedStack<BackstackEntry<INavigationPage>> _stack;
        private Mock<IParentNavigationManager<INavigationPage, PageIndex>> _navigationManager;
        private StackManager<INavigationPage, PageIndex> _stackManager;
        private Mock<INavigationPage> _pageA;
        private Mock<INavigationPage> _pageB;

        public StackManagerTests()
        {
            _stack = new TaggedStack<BackstackEntry<INavigationPage>>();
            _navigationManager = new Mock<IParentNavigationManager<INavigationPage, PageIndex>>();
            _stackManager = new StackManager<INavigationPage, PageIndex>(_stack, _navigationManager.Object);

            _pageA = CreateNavigationPage(PageIndex.PageA);
            _pageB = CreateNavigationPage(PageIndex.PageB);

            _pageDefinitions = new Dictionary<PageIndex, IPageProvider<INavigationPage>>
            {
                {
                    PageIndex.PageA,
                    new CachedPageProvider<INavigationPage>(
                        _pageA.Object,
                        () =>
                        {
                            _pageA = CreateNavigationPage(PageIndex.PageA);
                            return _pageA.Object;
                        })
                },
                {
                    PageIndex.PageB,
                    new CachedPageProvider<INavigationPage>(
                        _pageB.Object,
                        () =>
                        {
                            _pageB = CreateNavigationPage(PageIndex.PageB);
                            return _pageB.Object;
                        })
                },
            };

            _navigationManager.Setup(manager => manager.PageDefinitions).Returns(() => _pageDefinitions);
        }

        [Fact]
        public void TestBasicNavigation()
        {
            //Act
            _stackManager.Navigate(PageIndex.PageA); //initial navigation
            _stackManager.Navigate(PageIndex.PageB);
            //Assert
            Assert.Equal(PageIndex.PageA, _stack.Peek().Page.PageIdentifier);
        }

        [Fact]
        public void TestBasicBackwardsNavigation()
        {
            //Act
            _stackManager.Navigate(PageIndex.PageA); //initial navigation
            _stackManager.Navigate(PageIndex.PageB);
            _stackManager.GoBack();
            //Assert
            Assert.True(_stack.Count == 0);
            _pageA.Verify(page => page.NavigatedTo(), Times.Once);
            _pageA.Verify(page => page.NavigatedFrom(), Times.Once);
            _pageB.Verify(page => page.NavigatedTo(), Times.Once);
            _pageB.Verify(page => page.NavigatedFrom(), Times.Once);
            _pageA.Verify(page => page.NavigatedBack(), Times.Once);
        }

        private Mock<INavigationPage> CreateNavigationPage(PageIndex page)
        {
            var mock = new Mock<INavigationPage>();
            mock.Setup(navigationPage => navigationPage.PageIdentifier).Returns(page);
            return mock;
        }
    }
}
