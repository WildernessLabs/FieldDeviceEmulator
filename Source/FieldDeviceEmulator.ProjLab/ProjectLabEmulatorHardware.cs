using FieldDeviceEmulator.Core;
using FieldDeviceEmulator.Core.EmulatedDevices;
using Meadow.Devices;
using Meadow.Peripherals.Displays;
using Meadow.Peripherals.Sensors.Buttons;
using Meadow.Units;

namespace FieldDeviceEmulator.ProjLab;

public class ProjectLabEmulatorHardware : IEmulatorHardware
{
    private readonly IProjectLabHardware _hardware;
    private ModbusRtuFieldBus _modbusFieldBus;

    public RotationType DisplayRotation => RotationType._270Degrees;
    public IPixelDisplay? Display => _hardware.Display;
    public IButton? UpButton => _hardware.UpButton;
    public IButton? DownButton => _hardware.DownButton;
    public IButton? RightButton => _hardware.RightButton;
    public IButton? LeftButton => _hardware.LeftButton;

    public TemperatureTransmitter TemperatureTransmitter { get; }
    public CerusXDrive VFD { get; }

    public ProjectLabEmulatorHardware(IProjectLabHardware hardware)
    {
        _hardware = hardware;

        TemperatureTransmitter = new TemperatureTransmitter(
            new MikrobusCurrentLoopTransmitter(_hardware.MikroBus1),
            CurrentLoopRange.Current_4_20,
            0.Fahrenheit(),
            100.Fahrenheit());

        VFD = new CerusXDrive(1);
    }


    public ModbusRtuFieldBus GetModbusFieldBus()
    {
        if (_modbusFieldBus is null)
        {
            _modbusFieldBus = new ModbusRtuFieldBus(_hardware.Rs485Connector.CreateSerialPort(9600));
        }

        return _modbusFieldBus;
    }
}
