using Meadow.Foundation.mikroBUS.Sensors;
using Meadow.Hardware;
using Meadow.Units;
using System.Threading.Tasks;

namespace FieldDeviceEmulator.Core.EmulatedDevices;

/// <summary>
/// Implementation of current loop transmitter using Mikrobus C420T module
/// </summary>
public class MikrobusCurrentLoopTransmitter : ICurrentLoopTransmitter
{
    private readonly C420T _transmitter;

    /// <summary>
    /// Initializes a new instance of the MikrobusCurrentLoopTransmitter class
    /// </summary>
    /// <param name="spiBus">The SPI bus for communication</param>
    /// <param name="chipSelect">The chip select pin</param>
    public MikrobusCurrentLoopTransmitter(ISpiBus spiBus, IPin chipSelect)
    {
        _transmitter = new C420T(spiBus, chipSelect);
    }

    /// <summary>
    /// Sets the output current for the transmitter
    /// </summary>
    /// <param name="current">The current to output</param>
    /// <returns>A completed task</returns>
    public Task SetOutputCurrent(Current current)
    {
        _transmitter.GenerateOutput(current);
        return Task.CompletedTask;
    }
}
