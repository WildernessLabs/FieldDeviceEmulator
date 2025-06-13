using Meadow;
using Meadow.Foundation.Graphics;
using Meadow.Foundation.Graphics.MicroLayout;
using Meadow.Units;

namespace FieldDeviceEmulator.Core;

public class TempSenderLayout : GridLayout
{
    protected IFont LargeFont { get; }
    protected IFont SmallFont { get; }
    public int ChannelNumber { get; }
    public Temperature MinTemp { get; }
    public Temperature MaxTemp { get; }

    public TempSenderLayout(int channelNumber, Temperature minTemp, Temperature maxTemp,
        int left, int top, int width, int height)
        : base(left, top, width, height, 8, 5)
    {
        LargeFont = new Font12x20();
        SmallFont = new Font8x12();
        this.BackgroundColor = Color.FromRgb(50, 50, 50);
        ChannelNumber = channelNumber;
        MinTemp = minTemp;
        MaxTemp = maxTemp;

        this.Add(
            new Label(320, 20, $"Temperature")
            {
                TextColor = Color.White,
                Font = LargeFont,
                HorizontalAlignment = HorizontalAlignment.Center,
            },
            0, 0, colspan: 5
            );

        this.Add(
            new Label(320, 20, $"Channel {ChannelNumber}")
            {
                TextColor = Color.White,
                Font = LargeFont,
                HorizontalAlignment = HorizontalAlignment.Center,
            },
            1, 0, colspan: 5
            );

        this.Add(
            new Label(320, 20, $"Range {MinTemp.Fahrenheit:N0}-{MaxTemp.Fahrenheit:N0}F")
            {
                TextColor = Color.White,
                Font = LargeFont,
                HorizontalAlignment = HorizontalAlignment.Center,
            },
            2, 0, colspan: 5
            );

        this.Add(
            new Button(50, 30, "-")
            {
                TextColor = Color.White,
                Font = LargeFont
            },
            4, 1);
        this.Add(
            new Label(50, 30, "78.4F")
            {
                TextColor = Color.White,
                HorizontalAlignment = HorizontalAlignment.Center
            },
            4, 2);
        this.Add(
            new Button(50, 30, "+")
            {
                TextColor = Color.White,
                Font = LargeFont
            },
            4, 3);

        this.Add(
            new Label(320, 20, $"18.5mA")
            {
                TextColor = Color.White,
                Font = SmallFont,
                HorizontalAlignment = HorizontalAlignment.Center,
            },
            7, 0, colspan: 5
            );
    }
}
