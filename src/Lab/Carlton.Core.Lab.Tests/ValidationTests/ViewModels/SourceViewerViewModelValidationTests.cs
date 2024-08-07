﻿namespace Carlton.Core.Lab.Test.ValidationTests.ViewModels;

public class SourceViewerViewModelValidationTests
{
	[Theory, AutoData]
	public void SourceViewerViewModel_ShouldPassValidation(string componentSource)
	{
		// Arrange
		var sut = new SourceViewerViewModel { ComponentSource = componentSource };

		// Act
		var result = sut.TryValidate(out var validationErrors);

		// Assert
		result.ShouldBeTrue();
	}

	[Fact]
	public void SourceViewerViewModel_NullSource_ShouldFailValidation()
	{
		// Arrange
		var sut = new SourceViewerViewModel { ComponentSource = null };

		// Act
		var result = sut.TryValidate(out var validationErrors);

		// Assert
		result.ShouldBeFalse();
	}

	[Fact]
	public void SourceViewerViewModel_EmptySource_ShouldFailValidation()
	{
		// Arrange
		var sut = new SourceViewerViewModel { ComponentSource = string.Empty };

		// Act
		var result = sut.TryValidate(out var validationErrors);

		// Assert
		result.ShouldBeFalse();
	}
}
