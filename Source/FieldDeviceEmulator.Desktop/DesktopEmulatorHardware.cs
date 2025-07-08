using FieldDeviceEmulator.Core;
using FieldDeviceEmulator.Core.EmulatedDevices;
using Meadow;
using Meadow.Foundation.Sensors.Buttons;
using Meadow.Foundation.Sensors.Hid;
using Meadow.Peripherals.Displays;
using Meadow.Peripherals.Sensors.Buttons;
using Meadow.Units;

namespace FieldDeviceEmulator;

internal class DesktopEmulatorHardware : IEmulatorHardware
{
    private readonly Desktop device;
    private readonly Keyboard keyboard;

    public RotationType DisplayRotation => RotationType.Default;
    public IPixelDisplay? Display => device.Display;
    public IButton? UpButton { get; }
    public IButton? RightButton { get; }
    public IButton? LeftButton { get; }
    public IButton? DownButton { get; }

    public TemperatureTransmitter TemperatureTransmitter { get; }
    public CerusXDrive VFD { get; }

    public DesktopEmulatorHardware(Desktop device)
    {
        this.device = device;

        keyboard = new Keyboard();

        UpButton = new PushButton(keyboard.Pins.Up);
        DownButton = new PushButton(keyboard.Pins.Down);
        LeftButton = new PushButton(keyboard.Pins.Left);
        RightButton = new PushButton(keyboard.Pins.Right);

        TemperatureTransmitter = new TemperatureTransmitter(
            new SimulatedCurrentLoopTransmitter(0.012.Amps()),
            CurrentLoopRange.Current_4_20,
            0.Fahrenheit(),
            100.Fahrenheit());
    }

    public ModbusRtuFieldBus GetModbusFieldBus()
    {
        throw new System.NotImplementedException();
    }
}
