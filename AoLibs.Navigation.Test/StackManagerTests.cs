using System;
using System.Collections.Generic;
using System.Linq;
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
                        }) {PageIdentifier = PageIndex.PageA}
                },
                {
                    PageIndex.PageB,
                    new CachedPageProvider<INavigationPage>(
                        _pageB.Object,
                        () =>
                        {
                            _pageB = CreateNavigationPage(PageIndex.PageB);
                            return _pageB.Object;
                        }) {PageIdentifier = PageIndex.PageB}
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
            _navigationManager.Verify(manager =>
                manager.NotifyPagePopped(It.Is<INavigationPage>(page => page == _pageB.Object)));
        }

        [Fact]
        public void TestNavigationWithArguments()
        {
            //Arrange
            var navArg = new { Hello = "Test" };
            //Act
            _stackManager.Navigate(PageIndex.PageA,navArg);
            _stackManager.Navigate(PageIndex.PageB);
            //Asset
            _pageA.VerifySet(page => page.NavigationArguments = navArg, Times.Once);
            _pageB.VerifySet(page => page.NavigationArguments = null, Times.Never);
        }

        [Fact]
        public void TestNavigationWithArgumentsWhenGoingBack()
        {
            //Arrange
            var navArg = new { Hello = "Test" };
            //Act
            _stackManager.Navigate(PageIndex.PageA);
            _stackManager.Navigate(PageIndex.PageB);
            _stackManager.GoBack(navArg);
            //Asset
            _pageA.VerifySet(page => page.NavigationArguments = navArg, Times.Once);
            _pageB.VerifySet(page => page.NavigationArguments = null, Times.Never);
            _navigationManager.Verify(manager =>
                manager.NotifyPagePopped(It.Is<INavigationPage>(page => page == _pageB.Object)));
        }

        [Theory]
        [InlineData(NavigationBackstackOption.ClearBackstackToFirstOccurence)]
        [InlineData(NavigationBackstackOption.ForceNewPageInstance)]
        [InlineData(NavigationBackstackOption.NoBackstack)]
        [InlineData(NavigationBackstackOption.SetAsRootPage)]
        public void TestNavigationWithBackstackOptions(NavigationBackstackOption backstackOption)
        {
            //Arrange
            var savedPageB = _pageB;
            //Act
            _stackManager.Navigate(PageIndex.PageA);
            _stackManager.Navigate(PageIndex.PageA);
            _stackManager.Navigate(PageIndex.PageB);         
            _stackManager.Navigate(PageIndex.PageA);
            _stackManager.Navigate(PageIndex.PageA);
            _stackManager.Navigate(PageIndex.PageB, backstackOption);

            var pageList = _stack.Select(entry => (PageIndex)entry.Page.PageIdentifier).ToList();
            switch (backstackOption)
            {
                case NavigationBackstackOption.SetAsRootPage:
                    Assert.Equal(new PageIndex[0], pageList);
                    _navigationManager.Verify(manager => manager.NotifyStackCleared(), Times.Once);
                    break;
                case NavigationBackstackOption.ClearBackstackToFirstOccurence:
                    _navigationManager
                        .Verify(manager => manager
                            .NotifyPagesPopped(It.Is<IEnumerable<INavigationPage>>(
                                pages => pages.ToArray()[0] == _pageA.Object && pages.Count() == 1)), Times.Once);
                    Assert.Equal(new[] {PageIndex.PageA, PageIndex.PageA}, pageList);
                    break;
                case NavigationBackstackOption.NoBackstack:
                    Assert.Equal(new[] {PageIndex.PageA, PageIndex.PageB, PageIndex.PageA, PageIndex.PageA}, pageList);                  
                    break;
                case NavigationBackstackOption.ForceNewPageInstance:
                    Assert.NotEqual(_pageDefinitions[PageIndex.PageB].Page, savedPageB.Object);
                    break;
            }
        }

        [Fact]
        public void TestNavigationWithActionOnBack()
        {
            //Arrage
            var called = false;
            var action = new Action(() => called = true);
            //Act
            _stackManager.AddActionToBackstack(action);
            _stackManager.GoBack();
            //Assert
            Assert.True(called);
        }

        [Fact]
        public void TestPoppingFromBackstack()
        {
            //Act
            _stackManager.Navigate(PageIndex.PageA);
            _stackManager.AddActionToBackstack(() => {});
            _stackManager.Navigate(PageIndex.PageB);
            //Assert&Act ^^
            Assert.False(_stackManager.PopActionFromBackstack());
            _stackManager.GoBack();
            Assert.True(_stackManager.PopActionFromBackstack());
        }

        [Fact]
        public void TestClearingBackstack()
        {
            //Act
            _stackManager.Navigate(PageIndex.PageA);
            _stackManager.Navigate(PageIndex.PageA);
            _stackManager.Navigate(PageIndex.PageA);
            _stackManager.ClearBackStack();
            //Assert
            Assert.True(_stack.Count == 0);
            _navigationManager.Verify(manager => manager.NotifyStackCleared(),Times.Once);
        }
        

        private Mock<INavigationPage> CreateNavigationPage(PageIndex page)
        {
            var mock = new Mock<INavigationPage>();
            mock.Setup(navigationPage => navigationPage.PageIdentifier).Returns(page);
            return mock;
        }
    }
}
