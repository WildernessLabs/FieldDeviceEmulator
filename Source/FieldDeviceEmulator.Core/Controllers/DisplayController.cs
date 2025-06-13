using Meadow;
using Meadow.Foundation.Graphics;
using Meadow.Foundation.Graphics.MicroLayout;
using Meadow.Units;
using System.Collections.Generic;

namespace FieldDeviceEmulator.Core;

public class DisplayController
{
    private readonly DisplayScreen? screen;
    private readonly List<MicroLayout> navigationStack = new();
    private int currentPage = 0;
    private DisplayTheme? theme;
    private AbsoluteLayout mainLayout;

    public DisplayController(IEmulatorHardware hardware)
    {
        if (hardware.Display != null)
        {
            var theme = new DisplayTheme
            {
                Font = new Font12x20(),
                BackgroundColor = Color.Black,
                TextColor = Color.White
            };

            screen = new DisplayScreen(
                hardware.Display,
                rotation: hardware.DisplayRotation,
                theme: theme);

            GenerateLayout(screen);

            UpdateDisplay();
        }
        else
        {
            Resolver.Log.Warn("Display is null");
        }

        hardware.LeftButton.Clicked += OnPreviousRequested;
        hardware.RightButton.Clicked += OnNextRequested;
    }

    private void GenerateLayout(DisplayScreen screen)
    {
        theme = new DisplayTheme
        {
            BackgroundColor = Color.FromRgb(50, 50, 50)
        };

        mainLayout = new AbsoluteLayout(0, 0, screen.Width, screen.Height);

        navigationStack.Add(new VfdLayout(0, 0, screen.Width, screen.Height));
        navigationStack.Add(new TempSenderLayout(0, 0.Fahrenheit(), 100.Fahrenheit(),
            0, 0, screen.Width, screen.Height));

        for (var i = 0; i < navigationStack.Count; i++)
        {
            mainLayout.Controls.Add(navigationStack[i]);
            navigationStack[i].IsVisible = (i == 0);
        }

        screen.Controls.Add(mainLayout);

        for (var i = 0; i < navigationStack.Count; i++)
        {
            navigationStack[i].ApplyTheme(theme);
        }
    }

    private void OnNextRequested(object sender, System.EventArgs e)
    {
        if (screen == null) return;

        if (currentPage >= navigationStack.Count - 1) return;

        screen.BeginUpdate();

        navigationStack[currentPage].IsVisible = false;
        currentPage++;
        navigationStack[currentPage].IsVisible = true;
        screen.EndUpdate();
    }

    private void OnPreviousRequested(object sender, System.EventArgs e)
    {
        if (screen == null) return;

        if (currentPage <= 0) return;

        screen.BeginUpdate();

        navigationStack[currentPage].IsVisible = false;
        currentPage--;
        navigationStack[currentPage].IsVisible = true;
        screen.EndUpdate();
    }

    private void OnHomeRequested(object sender, System.EventArgs e)
    {
        if (screen == null) return;

        screen.BeginUpdate();

        if (currentPage > 0)
        {
            navigationStack[currentPage].IsVisible = false;
        }
        currentPage = 0;
        navigationStack[currentPage].IsVisible = true;
        screen.EndUpdate();
    }

    private void UpdateDisplay()
    {
        if (screen == null) { return; }

        // TODO: do things
    }
}