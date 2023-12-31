﻿namespace Carlton.Core.Components.Layouts.State.Theme;

public class ThemeState(Themes theme) : IThemeState
{
    public event EventHandler<ThemeChangedEventArgs> ThemeChanged;

    public Themes Theme { get; private set; } = theme;

    public ThemeState() : this(Themes.light)
    {
    }

    public void ToggleTheme()
    {
        switch (Theme)
        {
            case Themes.light:
                SetTheme(Themes.dark);
                break;
            case Themes.dark:
                SetTheme(Themes.light);
                break;
            default:
                return;
        }
    }

    public void SetTheme(Themes theme)
    {
        if (Theme == theme)
            return;

        Theme = theme;
        var args = new ThemeChangedEventArgs(theme);
        ThemeChanged?.Invoke(this, args);
    }
}
