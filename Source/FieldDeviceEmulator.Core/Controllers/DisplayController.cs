using Meadow;
using Meadow.Foundation.Graphics;
using Meadow.Foundation.Graphics.MicroLayout;
using System.Collections.Generic;

namespace FieldDeviceEmulator.Core;

public class DisplayController
{
    private readonly DisplayScreen? _screen;
    private readonly List<MicroLayout> _navigationStack = new();
    private int _currentPage = 0;
    private DisplayTheme? _theme;
    private AbsoluteLayout _mainLayout;
    private readonly IEmulatorHardware _hardware;

    public DisplayController(IEmulatorHardware hardware)
    {
        _hardware = hardware;

        if (_hardware.Display != null)
        {
            var theme = new DisplayTheme
            {
                Font = new Font12x20(),
                BackgroundColor = Color.Black,
                TextColor = Color.White
            };

            _screen = new DisplayScreen(
                _hardware.Display,
                rotation: _hardware.DisplayRotation,
                theme: theme);

            GenerateLayout(_screen);

            UpdateDisplay();
        }
        else
        {
            Resolver.Log.Warn("Display is null");
        }

        _hardware.LeftButton.Clicked += OnPreviousRequested;
        _hardware.RightButton.Clicked += OnNextRequested;
        _hardware.UpButton.Clicked += OnUpClicked;
        _hardware.DownButton.Clicked += OnDownClicked;
    }

    private async void OnDownClicked(object sender, System.EventArgs e)
    {
        if (_navigationStack[_currentPage] is IButtonSink buttonSink)
        {
            await buttonSink.OnDownClicked();
        }
    }

    private async void OnUpClicked(object sender, System.EventArgs e)
    {
        if (_navigationStack[_currentPage] is IButtonSink buttonSink)
        {
            await buttonSink.OnUpClicked();
        }
    }

    private void GenerateLayout(DisplayScreen screen)
    {
        _theme = new DisplayTheme
        {
            BackgroundColor = Color.FromRgb(50, 50, 50)
        };

        _mainLayout = new AbsoluteLayout(0, 0, screen.Width, screen.Height);

        _navigationStack.Add(new VfdLayout(_hardware, 0, 0, screen.Width, screen.Height));
        _navigationStack.Add(new TempSenderLayout(
            0,
            _hardware.TemperatureTransmitter,
            0, 0, screen.Width, screen.Height));

        for (var i = 0; i < _navigationStack.Count; i++)
        {
            _mainLayout.Controls.Add(_navigationStack[i]);
            _navigationStack[i].IsVisible = (i == 0);
        }

        screen.Controls.Add(_mainLayout);

        for (var i = 0; i < _navigationStack.Count; i++)
        {
            _navigationStack[i].ApplyTheme(_theme);
        }
    }

    private void OnNextRequested(object sender, System.EventArgs e)
    {
        if (_screen == null) return;

        if (_currentPage >= _navigationStack.Count - 1) return;

        _screen.BeginUpdate();

        _navigationStack[_currentPage].IsVisible = false;
        _currentPage++;
        _navigationStack[_currentPage].IsVisible = true;
        _screen.EndUpdate();
    }

    private void OnPreviousRequested(object sender, System.EventArgs e)
    {
        if (_screen == null) return;

        if (_currentPage <= 0) return;

        _screen.BeginUpdate();

        _navigationStack[_currentPage].IsVisible = false;
        _currentPage--;
        _navigationStack[_currentPage].IsVisible = true;
        _screen.EndUpdate();
    }

    private void OnHomeRequested(object sender, System.EventArgs e)
    {
        if (_screen == null) return;

        _screen.BeginUpdate();

        if (_currentPage > 0)
        {
            _navigationStack[_currentPage].IsVisible = false;
        }
        _currentPage = 0;
        _navigationStack[_currentPage].IsVisible = true;
        _screen.EndUpdate();
    }

    private void UpdateDisplay()
    {
        if (_screen == null) { return; }

        // TODO: do things
    }
}