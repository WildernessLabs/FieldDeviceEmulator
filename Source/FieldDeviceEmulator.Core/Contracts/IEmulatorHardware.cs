using Meadow.Peripherals.Displays;
using Meadow.Peripherals.Sensors.Buttons;

namespace FieldDeviceEmulator.Core;

public interface IEmulatorHardware
{
    RotationType DisplayRotation { get; }
    IPixelDisplay? Display { get; }

    IButton? UpButton { get; }
    IButton? RightButton { get; }
    IButton? LeftButton { get; }
}
