﻿namespace Carlton.Core.LayoutServices.Viewport;

/// <summary>
/// Represents the event arguments for viewport changed events.
/// </summary>
/// <param name="Height">The height of the viewport.</param>
/// <param name="Width">The width of the viewport.</param>
public sealed record ViewportModel(double Height, double Width)
{
	/// <summary>
	/// Represents the maximum width for mobile viewports.
	/// </summary>
	public const double MobileMaxWidth = 767.98;

	/// <summary>
	/// Determines whether the viewport is considered to be in a mobile state.
	/// </summary>
	public bool IsMobile { get => Width <= MobileMaxWidth; }
}

