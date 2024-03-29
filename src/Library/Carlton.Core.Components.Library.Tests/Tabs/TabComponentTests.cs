﻿namespace Carlton.Core.Components.Library.Tests;

[Trait("Component", nameof(TabComponentTests))]
public class TabComponentTests : TestContext
{
    private static readonly string TabMarkup =
        @"<div class=""tab"">
            <div>
                <span class=""message"">This is some test content under this tab</span>
                <div class=""main"">
                    <button>Click Me!</button>
                </div>
            </div>
        </div>";
    private readonly IRenderedComponent<TabBarBase> parent;

    public TabComponentTests()
    {
        parent = RenderComponent<TabBarBase>();
    }

    [Fact(DisplayName = "Markup Test")]
    public void Tab_WithOutParent_ThrowsArgumentNullException()
    {
        //Act
        var act = () => RenderComponent<Tab>(parameters => parameters
            .Add(p => p.DisplayText, "Test Tab")
            );

        //Assert
        Assert.Throws<ArgumentNullException>(act);
    }

    [Fact(DisplayName = "Tab Markup Test")]
    public void Tab_Markup_RendersCorrectly()
    {
        //Act
        var cut = RenderComponent<Tab>(parameters => parameters
            .AddCascadingValue(parent.Instance)
            .Add(p => p.DisplayText, "Test Tab")
            .AddChildContent("<div><span class=\"message\">This is some test content under this tab</span><div class=\"main\"><button>Click Me!</button></div></div>")
            );

        //Assert
        cut.MarkupMatches(TabMarkup);
    }

    [Fact(DisplayName = "Parent Active False, Render Test")]
    public void Tab_Parent_ActiveParam_False_RendersCorrectly()
    {

        //Arrange
        var cut = RenderComponent<Tab>(parameters => parameters
            .AddCascadingValue(parent.Instance)
            .Add(p => p.DisplayText, "Test Tab")
            .AddChildContent("<div><span class=\"message\">This is some test content under this tab</span><div class=\"main\"><button>Click Me!</button></div></div>")
            );

        var tabElement = cut.Find(".tab");

        //Act
        parent.Instance.ActiveTab = null;
        cut.Render();

        //Assert
        Assert.Empty(tabElement.InnerHtml);
    }

    [Fact(DisplayName = "Parent Active True, Render Test")]
    public void Tab_Parent_ActiveParam_True_RendersCorrectly()
    {

        //Arrange
        var childContent = "<div><span class=\"message\">This is some test content under this tab</span><div class=\"main\"><button>Click Me!</button></div></div>";
        var cut = RenderComponent<Tab>(parameters => parameters
            .AddCascadingValue(parent.Instance)
            .Add(p => p.DisplayText, "Test Tab")
            .AddChildContent(childContent)
            );

        var tabElement = cut.Find(".tab");

        //Act
        parent.Instance.ActiveTab = cut.Instance;
        cut.Render();

        //Assert
        Assert.NotEmpty(tabElement.InnerHtml);
        Assert.Equal(childContent, tabElement.InnerHtml);
    }
}
