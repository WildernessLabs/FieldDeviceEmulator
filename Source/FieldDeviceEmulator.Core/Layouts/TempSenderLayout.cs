using FieldDeviceEmulator.Core.EmulatedDevices;
using Meadow;
using Meadow.Foundation.Graphics;
using Meadow.Foundation.Graphics.MicroLayout;
using Meadow.Units;
using System.Threading.Tasks;

namespace FieldDeviceEmulator.Core;

public interface IButtonSink
{
    Task OnUpClicked();
    Task OnDownClicked();
}

public class TempSenderLayout : GridLayout, IButtonSink
{
    protected IFont LargeFont { get; }
    protected IFont SmallFont { get; }
    public int ChannelNumber { get; }
    public Temperature MinTemp { get; }
    public Temperature MaxTemp { get; }

    private readonly Label _tempLabel;
    private readonly Label _mALabel;
    private readonly TemperatureTransmitter _temperatureTransmitter;

    public TempSenderLayout(
        int channelNumber,
        TemperatureTransmitter transmitter,
        int left, int top, int width, int height)
        : base(left, top, width, height, 8, 5)
    {
        _temperatureTransmitter = transmitter;
        LargeFont = new Font12x20();
        SmallFont = new Font8x12();
        this.BackgroundColor = Color.FromRgb(50, 50, 50);
        ChannelNumber = channelNumber;
        MinTemp = transmitter.MinimumSenseTemp;
        MaxTemp = transmitter.MaximumSenseTemp;

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

        var decrementButton = new Button(50, 30, "-")
        {
            TextColor = Color.White,
            Font = LargeFont
        };
        decrementButton.Clicked += OnDecrementRequested;
        this.Add(decrementButton,
            4, 1);

        _tempLabel = new Label(50, 30, "--F")
        {
            TextColor = Color.White,
            HorizontalAlignment = HorizontalAlignment.Center
        };
        this.Add(
            _tempLabel,
            4, 2);

        var incrementButton = new Button(50, 30, "+")
        {
            TextColor = Color.White,
            Font = LargeFont
        };
        incrementButton.Clicked += OnIncrementRequested;

        this.Add(
            incrementButton,
            4, 3);

        _mALabel = new Label(320, 20, $"--mA")
        {
            TextColor = Color.White,
            Font = SmallFont,
            HorizontalAlignment = HorizontalAlignment.Center,
        };
        this.Add(
            _mALabel,
            7, 0, colspan: 5
            );

        UpdateValues();
    }

    private void UpdateValues()
    {
        _tempLabel.Text = $"{_temperatureTransmitter.GetCurrentTemperature().Fahrenheit:N0}F";
        _mALabel.Text = $"{_temperatureTransmitter.GetOutputCurrent().Milliamps:N1}mA";
    }

    public async Task Increment()
    {
        var temp = _temperatureTransmitter.GetCurrentTemperature().Fahrenheit;
        if (temp <= _temperatureTransmitter.MaximumSenseTemp.Fahrenheit - 1)
        {
            await _temperatureTransmitter.SetTemperature((temp + 1).Fahrenheit());
        }
        else
        { // because of double rounding
            await _temperatureTransmitter.SetTemperature(_temperatureTransmitter.MaximumSenseTemp);
        }
        UpdateValues();
    }

    public async Task Decrement()
    {
        var temp = _temperatureTransmitter.GetCurrentTemperature().Fahrenheit;
        if (temp >= _temperatureTransmitter.MinimumSenseTemp.Fahrenheit + 1)
        {
            await _temperatureTransmitter.SetTemperature((temp - 1).Fahrenheit());
        }
        else
        { // because of double rounding
            await _temperatureTransmitter.SetTemperature(_temperatureTransmitter.MinimumSenseTemp);
        }
        UpdateValues();
    }

    public async Task OnUpClicked()
    {
        await Increment();
    }

    public async Task OnDownClicked()
    {
        await Decrement();
    }

    private async void OnIncrementRequested(object sender, System.EventArgs e)
    {
        await Increment();
    }

    private async void OnDecrementRequested(object sender, System.EventArgs e)
    {
        await Decrement();
    }
}
