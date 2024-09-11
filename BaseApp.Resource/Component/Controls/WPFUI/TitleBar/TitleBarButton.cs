// This Source Code Form is subject to the terms of the MIT License.
// If a copy of the MIT was not distributed with this file, You can obtain one at https://opensource.org/licenses/MIT.
// Copyright (C) Leszek Pomianowski and WPF UI Contributors.
// All Rights Reserved.

using System.Windows;
using System.Windows.Automation.Peers;
using System.Windows.Automation.Provider;
using System.Windows.Media;

// ReSharper disable once CheckNamespace
namespace Wpf.Ui.Controls;

public class TitleBarButton : Wpf.Ui.Controls.Button
{
    /// <summary>Identifies the <see cref="ButtonType"/> dependency property.</summary>
    public static readonly DependencyProperty ButtonTypeProperty = DependencyProperty.Register(
        nameof(ButtonType),
        typeof(TitleBarButtonType),
        typeof(TitleBarButton),
        new PropertyMetadata(TitleBarButtonType.Unknown, OnButtonTypeChanged)
    );

    /// <summary>Identifies the <see cref="ButtonsForeground"/> dependency property.</summary>
    public static readonly DependencyProperty ButtonsForegroundProperty = DependencyProperty.Register(
        nameof(ButtonsForeground),
        typeof(Brush),
        typeof(TitleBarButton),
        new FrameworkPropertyMetadata(
            SystemColors.ControlTextBrush,
            FrameworkPropertyMetadataOptions.Inherits
        )
    );

    /// <summary>Identifies the <see cref="MouseOverButtonsForeground"/> dependency property.</summary>
    public static readonly DependencyProperty MouseOverButtonsForegroundProperty =
        DependencyProperty.Register(
            nameof(MouseOverButtonsForeground),
            typeof(Brush),
            typeof(TitleBarButton),
            new FrameworkPropertyMetadata(null, FrameworkPropertyMetadataOptions.Inherits)
        );

    /// <summary>Identifies the <see cref="RenderButtonsForeground"/> dependency property.</summary>
    public static readonly DependencyProperty RenderButtonsForegroundProperty = DependencyProperty.Register(
        nameof(RenderButtonsForeground),
        typeof(Brush),
        typeof(TitleBarButton),
        new FrameworkPropertyMetadata(
            SystemColors.ControlTextBrush,
            FrameworkPropertyMetadataOptions.Inherits
        )
    );

    /// <summary>
    /// Gets or sets the type of the button.
    /// </summary>
    public TitleBarButtonType ButtonType
    {
        get => (TitleBarButtonType)GetValue(ButtonTypeProperty);
        set => SetValue(ButtonTypeProperty, value);
    }

    /// <summary>
    /// Gets or sets the foreground of the navigation buttons.
    /// </summary>
    public Brush ButtonsForeground
    {
        get => (Brush)GetValue(ButtonsForegroundProperty);
        set => SetValue(ButtonsForegroundProperty, value);
    }

    /// <summary>
    /// Gets or sets the foreground of the navigation buttons when moused over.
    /// </summary>
    public Brush? MouseOverButtonsForeground
    {
        get => (Brush?)GetValue(MouseOverButtonsForegroundProperty);
        set => SetValue(MouseOverButtonsForegroundProperty, value);
    }

    public Brush RenderButtonsForeground
    {
        get => (Brush)GetValue(RenderButtonsForegroundProperty);
        set => SetValue(RenderButtonsForegroundProperty, value);
    }

    public bool IsHovered { get; private set; }

    private readonly Brush _defaultBackgroundBrush = Brushes.Transparent; // REVIEW: Should it be transparent?

    private bool _isClickedDown;

    public TitleBarButton()
    {
        Loaded += TitleBarButton_Loaded;
        Unloaded += TitleBarButton_Unloaded;
    }

    private void TitleBarButton_Unloaded(object sender, RoutedEventArgs e)
    {
    }

    private void TitleBarButton_Loaded(object sender, RoutedEventArgs e)
    {
        SetCurrentValue(RenderButtonsForegroundProperty, ButtonsForeground);
    }

    private void OnButtonsForegroundChanged(object? sender, EventArgs e)
    {
        SetCurrentValue(
            RenderButtonsForegroundProperty,
            IsHovered ? MouseOverButtonsForeground : ButtonsForeground
        );
    }

    /// <summary>
    /// Forces button background to change.
    /// </summary>
    public void Hover()
    {
        if (IsHovered)
        {
            return;
        }

        SetCurrentValue(BackgroundProperty, MouseOverBackground);
        if (MouseOverButtonsForeground != null)
        {
            SetCurrentValue(RenderButtonsForegroundProperty, MouseOverButtonsForeground);
        }

        IsHovered = true;
    }

    /// <summary>
    /// Forces button background to change.
    /// </summary>
    public void RemoveHover()
    {
        if (!IsHovered)
        {
            return;
        }

        SetCurrentValue(BackgroundProperty, _defaultBackgroundBrush);
        SetCurrentValue(RenderButtonsForegroundProperty, ButtonsForeground);

        IsHovered = false;
        _isClickedDown = false;
    }

    /// <summary>
    /// Invokes click on the button.
    /// </summary>
    public void InvokeClick()
    {
        if (
            new ButtonAutomationPeer(this).GetPattern(PatternInterface.Invoke)
            is IInvokeProvider invokeProvider
        )
        {
            invokeProvider.Invoke();
        }

        _isClickedDown = false;
    }

    private static void OnButtonTypeChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
    {
        if (d is not TitleBarButton titleBarButton)
        {
            return;
        }
    }
}
