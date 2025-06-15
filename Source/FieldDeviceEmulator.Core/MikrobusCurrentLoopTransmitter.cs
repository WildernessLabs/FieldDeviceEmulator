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
    private Current _lastCurrent;

    /// <summary>
    /// Initializes a new instance of the MikrobusCurrentLoopTransmitter class
    /// </summary>
    /// <param name="spiBus">The SPI bus for communication</param>
    /// <param name="chipSelect">The chip select pin</param>
    public MikrobusCurrentLoopTransmitter(ISpiBus spiBus, IPin chipSelect)
    {
        _transmitter = new C420T(spiBus, chipSelect);
        SetOutputCurrent(0.004.Amps()).GetAwaiter().GetResult();
    }

    /// <summary>
    /// Gets the last output current that was set
    /// </summary>
    /// <returns>The current that was last output by this sensor</returns>
    public Current GetOutputCurrent()
    {
        return _lastCurrent;
    }

    /// <summary>
    /// Sets the output current for the transmitter
    /// </summary>
    /// <param name="current">The current to output</param>
    /// <returns>A completed task</returns>
    public Task SetOutputCurrent(Current current)
    {
        _transmitter.GenerateOutput(current);
        _lastCurrent = current;
        return Task.CompletedTask;
    }

}
