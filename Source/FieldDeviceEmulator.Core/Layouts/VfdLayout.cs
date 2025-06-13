using Meadow;
using Meadow.Foundation.Graphics;
using Meadow.Foundation.Graphics.MicroLayout;

namespace FieldDeviceEmulator.Core;

public class VfdLayout : GridLayout
{
    protected IFont LargeFont { get; }

    public VfdLayout(int left, int top, int width, int height)
        : base(left, top, width, height, 6, 4)
    {
        LargeFont = new Font12x20();
        this.BackgroundColor = Color.FromRgb(50, 50, 50);

        this.Add(
            new Label(100, 30, "XDrive VFD")
            {
                TextColor = Color.White,
                Font = LargeFont
            },
            0, 0, colspan: 4
            );

        this.Add(
            new Label(130, 30, "Current:")
            {
                TextColor = Color.WhiteSmoke,
                HorizontalAlignment = HorizontalAlignment.Right
            },
            1, 0);
        this.Add(
            new Button(50, 30, "-")
            {
                TextColor = Color.White,
                Font = LargeFont
            },
            1, 1);
        this.Add(
            new Label(50, 30, "10A")
            {
                TextColor = Color.White,
                HorizontalAlignment = HorizontalAlignment.Center
            },
            1, 2);
        this.Add(
            new Button(50, 30, "+")
            {
                TextColor = Color.White,
                Font = LargeFont
            },
            1, 3);

        this.Add(
            new Label(130, 30, "Voltage:")
            {
                TextColor = Color.WhiteSmoke,
                HorizontalAlignment = HorizontalAlignment.Right
            },
            2, 0);
        this.Add(
            new Button(50, 30, "-")
            {
                TextColor = Color.White,
                Font = LargeFont
            },
            2, 1);
        this.Add(
            new Label(50, 30, "120.1V")
            {
                TextColor = Color.White,
                HorizontalAlignment = HorizontalAlignment.Center
            },
            2, 2);
        this.Add(
            new Button(50, 30, "+")
            {
                TextColor = Color.White,
                Font = LargeFont
            },
            2, 3);
    }
}
