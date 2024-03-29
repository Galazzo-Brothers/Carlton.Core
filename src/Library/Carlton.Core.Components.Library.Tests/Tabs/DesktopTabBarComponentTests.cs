﻿namespace Carlton.Core.Components.Library.Tests;

[Trait("Component", nameof(DesktopTabBar))]
public class DesktopTabBarComponentTests : TestContext
{
    private readonly string DesktopTabBarMarkup = @"
<div class=""tabs"" b-1252a8sdqd>
    <div class=""content"" b-1252a8sdqd>
        <div class=""carlton-tab active"" b-1252a8sdqd>
            <a class="""" blazor:onclick=""1"" href=""#"" b-1252a8sdqd>Test Tab 1</a>
            <span class=""tab-selected-bar"" b-1252a8sdqd></span>
        </div>
        <div class=""carlton-tab"" b-1252a8sdqd>
            <a class="""" blazor:onclick=""2"" href=""#"" b-1252a8sdqd>Test Tab 2</a>
            <span class=""tab-selected-bar"" b-1252a8sdqd></span>
        </div>
        <div class=""carlton-tab"" b-1252a8sdqd>
            <a class="""" blazor:onclick=""3"" href=""#"" b-1252a8sdqd>Test Tab 3</a>
            <span class=""tab-selected-bar"" b-1252a8sdqd></span>
        </div>
    </div>
</div>

<div class=""tab"">
    <span>This is the first tab</span>
</div>
<div class=""tab"">
</div>
<div class=""tab"">
</div>";

    [Fact(DisplayName = "Markup Test")]
    public void DesktopTabBar_Markup_RendersCorrectly()
    {
        //Act
        var cut = RenderComponent<DesktopTabBar>(parameters => parameters
            .AddChildContent<Tab>(parameters => parameters
                .Add(p => p.DisplayText, "Test Tab 1")
                .Add(p => p.Icon, "Icon 1")
                .Add(p => p.ChildContent, "<span>This is the first tab</span>"))
            .AddChildContent<Tab>(parameters => parameters
                .Add(p => p.DisplayText, "Test Tab 2")
                .Add(p => p.Icon, "Icon 2")
                .Add(p => p.ChildContent, "<span>This is the second tab</span>"))
             .AddChildContent<Tab>(parameters => parameters
                .Add(p => p.DisplayText, "Test Tab 3")
                .Add(p => p.Icon, "Icon 3")
                .Add(p => p.ChildContent, "<span>This is the third tab</span>"))
            );

        //Assert
        cut.MarkupMatches(DesktopTabBarMarkup);
    }

    [Fact]
    [DisplayName("One Tab, Render Test")]
    public void DesktopTabBar_WithOneTab_RendersCorrectly()
    {
        //Arrange
        var expectedCount = 1;

        //Act
        var cut = RenderComponent<DesktopTabBar>(parameters => parameters
                    .AddChildContent<Tab>(parameters => parameters
                        .Add(p => p.DisplayText, "Test Tab 1")
                        .Add(p => p.Icon, "Icon 1")
                        .Add(p => p.ChildContent, "<span>This is the first tab</span>")));

        var tabs = cut.FindAll(".carlton-tab");
        var count = tabs.Count;


        //Assert
        Assert.Equal(expectedCount, count);
    }

    [Fact(DisplayName = "Two Tabs, Render Test")]
    public void DesktopTabBar_WithTwoTabs_RendersCorrectly()
    {
        //Arrange
        var expectedCount =2;

        //Act
        var cut = RenderComponent<DesktopTabBar>(parameters => parameters
                    .AddChildContent<Tab>(parameters => parameters
                        .Add(p => p.DisplayText, "Test Tab 1")
                        .Add(p => p.Icon, "Icon 1")
                        .Add(p => p.ChildContent, "<span>This is the first tab</span>"))
                    .AddChildContent<Tab>(parameters => parameters
                        .Add(p => p.DisplayText, "Test Tab 2")
                        .Add(p => p.Icon, "Icon 2")
                        .Add(p => p.ChildContent, "<span>This is the second tab</span>")));

        var tabs = cut.FindAll(".carlton-tab");
        var count = tabs.Count;


        //Assert
        Assert.Equal(expectedCount, count);
    }

    [Fact]
    [DisplayName("Three Tabs, Render Test")]
    public void DesktopTabBar_WithThreeTabs_RendersCorrectly()
    {
        //Arrange
        var expectedCount = 3;

        //Act
        var cut = RenderComponent<DesktopTabBar>(parameters => parameters
                    .AddChildContent<Tab>(parameters => parameters
                        .Add(p => p.DisplayText, "Test Tab 1")
                        .Add(p => p.Icon, "Icon 1")
                        .Add(p => p.ChildContent, "<span>This is the first tab</span>"))
                    .AddChildContent<Tab>(parameters => parameters
                        .Add(p => p.DisplayText, "Test Tab 2")
                        .Add(p => p.Icon, "Icon 2")
                        .Add(p => p.ChildContent, "<span>This is the second tab</span>"))
                    .AddChildContent<Tab>(parameters => parameters
                        .Add(p => p.DisplayText, "Test Tab 3")
                        .Add(p => p.Icon, "Icon 3")
                        .Add(p => p.ChildContent, "<span>This is the third tab</span>")));

        var tabs = cut.FindAll(".carlton-tab");
        var count = tabs.Count;


        //Assert
        Assert.Equal(expectedCount, count);
    }

    [Fact(DisplayName = "Default Active Tab Test")]
    public void DesktopTabBar_DefaultActiveTab_RendersCorrectly()
    {
        //Act
        var cut = RenderComponent<DesktopTabBar>(parameters => parameters
            .AddChildContent<Tab>(parameters => parameters
                .Add(p => p.DisplayText, "Test Tab 1")
                .Add(p => p.Icon, "Icon 1")
                .Add(p => p.ChildContent, "<span>This is the first tab</span>"))
            .AddChildContent<Tab>(parameters => parameters
                .Add(p => p.DisplayText, "Test Tab 2")
                .Add(p => p.Icon, "Icon 2")
                .Add(p => p.ChildContent, "<span>This is the second tab</span>"))
             .AddChildContent<Tab>(parameters => parameters
                .Add(p => p.DisplayText, "Test Tab 3")
                .Add(p => p.Icon, "Icon 2")
                .Add(p => p.ChildContent, "<span>This is the third tab</span>"))
            );

        var defaultTab = cut.FindAll(".carlton-tab")[0];
        var isActive = defaultTab.ClassList.Contains("active");

        //Assert
        Assert.True(isActive);
    }

    [Theory(DisplayName = "Default Active Tab, CSS Active Class Test")]
    [InlineData(0)]
    [InlineData(1)]
    [InlineData(2)]
    public void DesktopTabBar_ActiveTabClass_RendersCorrectly(int activeTabIndex)
    {
        //Act
        var cut = RenderComponent<DesktopTabBar>(parameters => parameters
            .AddChildContent<Tab>(parameters => parameters
                .Add(p => p.DisplayText, "Test Tab 1")
                .Add(p => p.Icon, "Icon 1")
                .Add(p => p.ChildContent, "<span>This is the first tab</span>"))
            .AddChildContent<Tab>(parameters => parameters
                .Add(p => p.DisplayText, "Test Tab 2")
                .Add(p => p.Icon, "Icon 2")
                .Add(p => p.ChildContent, "<span>This is the second tab</span>"))
             .AddChildContent<Tab>(parameters => parameters
                .Add(p => p.DisplayText, "Test Tab 3")
                .Add(p => p.Icon, "Icon 3")
                .Add(p => p.ChildContent, "<span>This is the third tab</span>"))
            );

        var tabs = cut.FindAll(".carlton-tab", true);
        var selectedTab = tabs.ElementAt(activeTabIndex);
        var anchor = selectedTab.Children.First();
        anchor.Click();
        var isActive = tabs.ElementAt(activeTabIndex).ClassList.Contains("active");

        //Assert
        Assert.True(isActive);
    }

    [Theory(DisplayName = "Active Tab, Render Test")]
    [InlineData(0, "<span>This is the first tab</span>")]
    [InlineData(1, "<span>This is the second tab</span>")]
    [InlineData(2, "<span>This is the third tab</span>")]
    public void DesktopTabBar_ActiveTab_RendersCorrectly(int activeTabIndex, string expectedText)
    {
        //Act
        var cut = RenderComponent<DesktopTabBar>(parameters => parameters
            .AddChildContent<Tab>(parameters => parameters
                .Add(p => p.DisplayText, "Test Tab 1")
                .Add(p => p.Icon, "Icon 1")
                .Add(p => p.ChildContent, "<span>This is the first tab</span>"))
            .AddChildContent<Tab>(parameters => parameters
                .Add(p => p.DisplayText, "Test Tab 2")
                .Add(p => p.Icon, "Icon 2")
                .Add(p => p.ChildContent, "<span>This is the second tab</span>"))
             .AddChildContent<Tab>(parameters => parameters
                .Add(p => p.DisplayText, "Test Tab 3")
                .Add(p => p.Icon, "Icon 3")
                .Add(p => p.ChildContent, "<span>This is the third tab</span>"))
            );

        var tabs = cut.FindAll(".carlton-tab");
        var selectedTab = tabs.ElementAt(activeTabIndex);
        var anchor = selectedTab.Children.First();
        anchor.Click();
        var actualText = cut.FindAll(".tab").ElementAt(activeTabIndex).InnerHtml;

        //Assert
        Assert.Equal(expectedText, actualText);
    }

    [Theory(DisplayName = "ChildDisplayText Parameter")]
    [InlineData("Display Test 1")]
    [InlineData("Display Test Again")]
    [InlineData("Display Test One Final Time")]
    public void DesktopTabBar_ChildDisplayTextParam_RendersCorrectly(string displayText)
    {
        //Act
        var cut = RenderComponent<DesktopTabBar>(parameters => parameters
            .AddChildContent<Tab>(parameters => parameters
                .Add(p => p.DisplayText, displayText)
                .Add(p => p.Icon, "Icon 1")
                .Add(p => p.ChildContent, "<span>This is the first tab</span>"))
            );

        var anchor = cut.Find("a");
        var actualText = anchor.TextContent;

        //Assert
        Assert.Equal(displayText, actualText);
    }
}
