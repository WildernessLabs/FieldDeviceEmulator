using FieldDeviceEmulator.Core.EmulatedDevices;
using Meadow.Modbus;
using System.Diagnostics;
using EmulatedDrive = FieldDeviceEmulator.Core.EmulatedDevices.CerusXDrive;
using VFD = Meadow.Foundation.VFDs.CerusXDrive;

namespace FieldDeviceEmulator.Tests;

public class CerusXDriveTests
{
    [Fact]
    public async Task DesktopLoopbackTest()
    {
        using var serverPort = new SerialPortShim("COM12", 9600, Meadow.Hardware.Parity.None, 8, Meadow.Hardware.StopBits.One);
        using var bus = new ModbusRtuFieldBus(serverPort);
        using var clientPort = new SerialPortShim("COM8", 9600, Meadow.Hardware.Parity.None, 8, Meadow.Hardware.StopBits.One);

        bus.Connect();
        var simulatedDrive = new EmulatedDrive(10);
        bus.Add(simulatedDrive);

        var client = new ModbusRtuClient(clientPort);
        var vfd = new VFD(client, 10);
        await vfd.Connect();

        Debug.WriteLine($"reading...");
        for (var i = 0; i < 5; i++)
        {
            try
            {
                var temp = await vfd.ReadAmbientTemperature();
                Debug.WriteLine($"{temp.Fahrenheit:N1}F");
            }
            catch (Exception ex)
            {
            }
            await Task.Delay(250);
        }

    }
}