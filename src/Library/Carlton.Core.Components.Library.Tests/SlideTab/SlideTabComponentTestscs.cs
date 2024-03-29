﻿namespace Carlton.Core.Components.Library.Tests;

[Trait("Component", nameof(SlideTab))]
public class SlideTabComponentTests : TestContext
{
    private static readonly string SlideTabMarkup = @"
<div class=""slide-tab"" style=""--slide-tab-bottom:50px;"" b-wxqrrervve>
    <button class=""slide-button"" blazor:onclick=""1"" b-wxqrrervve>Test Title</button>
    <div class=""slide-container "" b-wxqrrervve>
        <span class=""test-content"">Here is some test content</span>
    </div>
</div>";


    [Fact(DisplayName = "Markup Test")]
    public void SlideTab_Markup_RendersCorrectly()
    {
        //Act
        var cut = RenderComponent<SlideTab>(parameters => parameters
            .Add(p => p.Title, "Test Title")
            .Add(p => p.IsExpanded, false)
            .Add(p => p.PositionBottom, 50)
            .Add(p => p.Content, "<span class=\"test-content\">Here is some test content</span>")
            );

        //Assert
        cut.MarkupMatches(SlideTabMarkup);
    }

    [Theory(DisplayName = "Title Parameter Test")]
    [InlineData("Test Title")]
    [InlineData("Another Test Title")]
    [InlineData("One Final Test Title")]
    public void SlideTab_TitleParam_RendersCorrectly(string expectedTitle)
    {
        //Act
        var cut = RenderComponent<SlideTab>(parameters => parameters
            .Add(p => p.Title, expectedTitle)
            .Add(p => p.IsExpanded, false)
            .Add(p => p.PositionBottom, 50)
            .Add(p => p.Content, "<span class=\"test-content\">Here is some test content</span>")
            );

        var btn = cut.Find("button");
        var actualTitle = btn.TextContent;

        //Assert
        Assert.Equal(expectedTitle, actualTitle);
    }

    [Theory(DisplayName = "IsExpanded Parameter Test")]
    [InlineData(true)]
    [InlineData(false)]
    public void SlideTab_IsExpandedParam_RendersCorrectly(bool expectedIsExpanded)
    {
        //Act
        var cut = RenderComponent<SlideTab>(parameters => parameters
            .Add(p => p.Title, "test title")
            .Add(p => p.IsExpanded, expectedIsExpanded)
            .Add(p => p.PositionBottom, 50)
            .Add(p => p.Content, "<span class=\"test-content\">Here is some test content</span>")
            );

        var slideContainer = cut.Find(".slide-container");
        var actualIsExpanded = slideContainer.ClassList.Contains("expanded");

        //Assert
        Assert.Equal(expectedIsExpanded, actualIsExpanded);
    }

    [Theory(DisplayName = "PositionBottom Parameter Test")]
    [InlineData(5)]
    [InlineData(50)]
    [InlineData(500)]
    public void SlideTab_PositionBottomParam_RendersCorrectly(int expectedPositionBottom)
    {
        //Arrange
        var expectedStyleValue = $"--slide-tab-bottom:{expectedPositionBottom}px;";

        //Act
        var cut = RenderComponent<SlideTab>(parameters => parameters
            .Add(p => p.Title, "test title")
            .Add(p => p.IsExpanded, false)
            .Add(p => p.PositionBottom, expectedPositionBottom)
            .Add(p => p.Content, "<span class=\"test-content\">Here is some test content</span>")
            );

        var slideTab = cut.Find(".slide-tab");
        var actualStyleValue = slideTab.Attributes.First(_ => _.Name == "style").Value;

        //Assert
        Assert.Equal(expectedStyleValue, actualStyleValue);
    }

    [Theory(DisplayName = "Content Parameter Test")]
    [InlineData("<span class=\"test-content\">Here is some test content</span>")]
    [InlineData("<button class=\"test-content\">Here is some test content</button>")]
    [InlineData("<div class=\"test-content\">Here is some test content</div>")]
    public void SlideTab_ContentParam_RendersCorrectly(string expectedContent)
    {
        //Act
        var cut = RenderComponent<SlideTab>(parameters => parameters
            .Add(p => p.Title, "test title")
            .Add(p => p.IsExpanded, false)
            .Add(p => p.PositionBottom, 50)
            .Add(p => p.Content, expectedContent)
            );

        var slideContainer = cut.Find(".slide-container");
        var actualContent= slideContainer.InnerHtml;

        //Assert
        Assert.Equal(expectedContent, actualContent);
    }
}
