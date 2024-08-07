﻿using Carlton.Core.Components.Toasts;
namespace Carlton.Core.Components.Tests.Toasts;

[Trait("Component", nameof(Toast))]
public class ToastComponentTests : TestContext
{
    private readonly BunitJSInterop _moduleInterop;

    //Arrange
    public ToastComponentTests()
    {
        _moduleInterop = JSInterop.SetupModule(Toast.ImportPath);
        _moduleInterop.Setup<Task>(Toast.InitNewToast, _ => true);
        _moduleInterop.Setup<Task>(Toast.DisposeToast, _ => true);
    }

    [Theory(DisplayName = "Markup Test")]
    [InlineAutoData(ToastTypes.Success)]
    [InlineAutoData(ToastTypes.Info)]
    [InlineAutoData(ToastTypes.Warning)]
    [InlineAutoData(ToastTypes.Error)]
    public void Toast_Markup_RendersCorrectly(
       ToastTypes expectedToastType,
       int expectedId,
       string expectedTitle,
       string expectedMessage,
       bool expectedIsDismissed)
    {
        //Arrange
        var expectedMarkup =
@$"<div id=""toast-{expectedId}"" class=""toast {expectedToastType.ToString().ToLower()} {(expectedIsDismissed ? "dismissed" : string.Empty)}"">
    <div class=""content"">
        <span class=""icon mdi mdi-24px {ExpectedToastIconClass(expectedToastType)}""></span>
        <div class=""message-container"">
            <span class=""title"">{expectedTitle}</span>
            <span class=""message"">{expectedMessage}</span>
        </div>
        <span class=""dismiss mdi mdi-18px mdi-close""></span>
    </div>
</div>";

        //Act
        var cut = RenderComponent<Toast>(parameters => parameters
            .Add(p => p.Id, expectedId)
            .Add(p => p.Title, expectedTitle)
            .Add(p => p.Message, expectedMessage)
            .Add(p => p.ToastType, expectedToastType)
            .Add(p => p.FadeOutEnabled, false)
            .Add(p => p.IsDismissed, expectedIsDismissed));

        //Assert
        cut.MarkupMatches(expectedMarkup);
    }

    [Theory(DisplayName = "Id Parameter Test"), AutoData]
    public void Toast_IdParameter_RendersCorrectly(
       int expectedId,
       string expectedTitle,
       string expectedMessage,
       ToastTypes expectedToastType,
       bool expectedIsDismissed)
    {
        //Act
        var cut = RenderComponent<Toast>(parameters => parameters
            .Add(p => p.Id, expectedId)
            .Add(p => p.Title, expectedTitle)
            .Add(p => p.Message, expectedMessage)
            .Add(p => p.ToastType, expectedToastType)
            .Add(p => p.FadeOutEnabled, false)
            .Add(p => p.IsDismissed, expectedIsDismissed));

        var expectedToastId = $"toast-{expectedId}"; ;
        var actualId = cut.Find(".toast").Id;

        //Assert
        actualId.ShouldBe(expectedToastId);
    }

    [Theory(DisplayName = "Title Parameter Test"), AutoData]
    public void Toast_TitleParameter_RendersCorrectly(
        int expectedId,
        string expectedTitle,
        string expectedMessage,
        ToastTypes expectedToastType,
        bool expectedIsDismissed)
    {
        //Act
        var cut = RenderComponent<Toast>(parameters => parameters
            .Add(p => p.Id, expectedId)
            .Add(p => p.Title, expectedTitle)
            .Add(p => p.Message, expectedMessage)
            .Add(p => p.ToastType, expectedToastType)
            .Add(p => p.FadeOutEnabled, false)
            .Add(p => p.IsDismissed, expectedIsDismissed));

        var actualTitle = cut.Find(".title").TextContent;

        //Assert
        actualTitle.ShouldBe(expectedTitle);
    }

    [Theory(DisplayName = "Message Parameter Test"), AutoData]

    public void Toast_MessageParameter_RendersCorrectly(
        int expectedId,
        string expectedTitle,
        string expectedMessage,
        ToastTypes expectedToastType,
        bool expectedIsDismissed)
    {
        //Act
        var cut = RenderComponent<Toast>(parameters => parameters
            .Add(p => p.Id, expectedId)
            .Add(p => p.Title, expectedTitle)
            .Add(p => p.Message, expectedMessage)
            .Add(p => p.ToastType, expectedToastType)
            .Add(p => p.FadeOutEnabled, false)
            .Add(p => p.IsDismissed, expectedIsDismissed));

        var actualMessage = cut.Find(".message").TextContent;

        //Assert
        actualMessage.ShouldBe(expectedMessage);
    }

    [Theory(DisplayName = "ToastType Parameter Test")]
    [InlineAutoData(ToastTypes.Success)]
    [InlineAutoData(ToastTypes.Info)]
    [InlineAutoData(ToastTypes.Warning)]
    [InlineAutoData(ToastTypes.Error)]
    public void Toast_ToastTypeParameter_RendersCorrectly(
      ToastTypes expectedToastType,
      int expectedId,
      string expectedTitle,
      string expectedMessage,
      bool expectedIsDismissed)
    {
        //Act
        var cut = RenderComponent<Toast>(parameters => parameters
            .Add(p => p.Id, expectedId)
            .Add(p => p.Title, expectedTitle)
            .Add(p => p.Message, expectedMessage)
            .Add(p => p.ToastType, expectedToastType)
            .Add(p => p.FadeOutEnabled, false)
            .Add(p => p.IsDismissed, expectedIsDismissed));

        var actualToastClassList = cut.Find(".toast").ClassList;
        var actualIconClassList = cut.Find(".icon").ClassList;

        //Assert
        actualToastClassList.ShouldContain(expectedToastType.ToString().ToLower());
        actualIconClassList.ShouldContain(ExpectedToastIconClass(expectedToastType));
    }

    [Theory(DisplayName = "FadeOutEnabled Parameter Test")]
    [InlineAutoData(true)]
    [InlineAutoData(false)]
    public async Task Toast_FadeOutEnabledParameter_RendersCorrectly(
     bool expectedFadeOutEnabled,
     int expectedId,
     string expectedTitle,
     string expectedMessage,
     ToastTypes expectedToastType)
    {
        //Act
        var cut = RenderComponent<Toast>(parameters => parameters
            .Add(p => p.Id, expectedId)
            .Add(p => p.Title, expectedTitle)
            .Add(p => p.Message, expectedMessage)
            .Add(p => p.ToastType, expectedToastType)
            .Add(p => p.FadeOutEnabled, expectedFadeOutEnabled)
            .Add(p => p.IsDismissed, false));

        await Task.Delay(3000);

        //Assert
        cut.Instance.IsDismissed.ShouldBe(expectedFadeOutEnabled);
    }

    [Theory(DisplayName = "IsDismissed Parameter Test")]
    [InlineAutoData(true)]
    [InlineAutoData(false)]
    public void Toast_IsDismissedParameter_RendersCorrectly(
      bool expectedIsDismissed,
      int expectedId,
      string expectedTitle,
      string expectedMessage,
      ToastTypes expectedToastType)
    {
        //Act
        var cut = RenderComponent<Toast>(parameters => parameters
            .Add(p => p.Id, expectedId)
            .Add(p => p.Title, expectedTitle)
            .Add(p => p.Message, expectedMessage)
            .Add(p => p.ToastType, expectedToastType)
            .Add(p => p.FadeOutEnabled, false)
            .Add(p => p.IsDismissed, expectedIsDismissed));

        var toastClassList = cut.Find(".toast").ClassList;
        var actualDismissedClassExists = toastClassList.Contains("dismissed");

        //Assert
        actualDismissedClassExists.ShouldBe(expectedIsDismissed);
    }

    [Theory(DisplayName = "Dismiss Click Test"), AutoData]
    public void Toast_DismissClick_ShouldDismiss(
    int expectedId,
    string expectedTitle,
    string expectedMessage,
    ToastTypes expectedToastType)
    {
        var cut = RenderComponent<Toast>(parameters => parameters
            .Add(p => p.Id, expectedId)
            .Add(p => p.Title, expectedTitle)
            .Add(p => p.Message, expectedMessage)
            .Add(p => p.ToastType, expectedToastType)
            .Add(p => p.FadeOutEnabled, false)
            .Add(p => p.IsDismissed, false));

        var dismissBtn = cut.Find(".dismiss");

        //Act
        dismissBtn.Click();

        var actualToastClassList = cut.Find(".toast").ClassList;

        //Assert
        actualToastClassList.ShouldContain("dismissed");
    }

    [Theory(DisplayName = "OnDismissed Parameter Test"), AutoData]
    public async Task Toast_OnDismissedParameter_ShouldFire(
     int expectedId,
     string expectedTitle,
     string expectedMessage,
     ToastTypes expectedToastType)
    {
        //Arrange
        var eventFired = false;

        var cut = RenderComponent<Toast>(parameters => parameters
            .Add(p => p.Id, expectedId)
            .Add(p => p.Title, expectedTitle)
            .Add(p => p.Message, expectedMessage)
            .Add(p => p.ToastType, expectedToastType)
            .Add(p => p.FadeOutEnabled, false)
            .Add(p => p.IsDismissed, false)
            .Add(p => p.OnDismissed, () => eventFired = true));

        var dismissBtn = cut.Find(".dismiss");

        //Act
        await cut.Instance.HandleDismiss();

        //Assert
        eventFired.ShouldBeTrue();
    }

    [Theory(DisplayName = "JavaScript Interop Init Test"), AutoData]
    public void Toast_OnAfterRender_ShouldFireJavaScript(
    int expectedId,
    string expectedTitle,
    string expectedMessage,
    ToastTypes expectedToastType)
    {
        //Act
        var cut = RenderComponent<Toast>(parameters => parameters
            .Add(p => p.Id, expectedId)
            .Add(p => p.Title, expectedTitle)
            .Add(p => p.Message, expectedMessage)
            .Add(p => p.ToastType, expectedToastType)
            .Add(p => p.FadeOutEnabled, false)
            .Add(p => p.IsDismissed, false));

        //Assert
        _moduleInterop.VerifyInvoke(Toast.InitNewToast);
    }

    [Theory(DisplayName = "JavaScript Interop Dispose Test"), AutoData]
    public void Toast_OnDisposed_ShouldFireJavaScript(
       int expectedId,
       string expectedTitle,
       string expectedMessage,
       ToastTypes expectedToastType)
    {
        //Arrange
        var cut = RenderComponent<Toast>(parameters => parameters
            .Add(p => p.Id, expectedId)
            .Add(p => p.Title, expectedTitle)
            .Add(p => p.Message, expectedMessage)
            .Add(p => p.ToastType, expectedToastType)
            .Add(p => p.FadeOutEnabled, false)
            .Add(p => p.IsDismissed, false));

        //Act
        DisposeComponents();

        //Assert
        _moduleInterop.VerifyInvoke(Toast.DisposeToast);
    }

    private static string ExpectedToastIconClass(ToastTypes expectedToastType)
        => expectedToastType switch
        {
            ToastTypes.Success => "mdi-check-circle",
            ToastTypes.Info => "mdi-alert-circle-outline",
            ToastTypes.Warning => "mdi-alert",
            ToastTypes.Error => "mdi-alert-circle-outline",
            _ => string.Empty,
        };
}

