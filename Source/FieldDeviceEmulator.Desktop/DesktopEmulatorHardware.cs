using FieldDeviceEmulator.Core;
using Meadow;
using Meadow.Foundation.Sensors.Buttons;
using Meadow.Foundation.Sensors.Hid;
using Meadow.Peripherals.Displays;
using Meadow.Peripherals.Sensors.Buttons;

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

    public DesktopEmulatorHardware(Desktop device)
    {
        this.device = device;

        keyboard = new Keyboard();

        UpButton = new PushButton(keyboard.Pins.Up);
        LeftButton = new PushButton(keyboard.Pins.Left);
        RightButton = new PushButton(keyboard.Pins.Right);
    }

}
