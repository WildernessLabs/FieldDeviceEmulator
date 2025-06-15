using Meadow.Units;
using System.Threading.Tasks;

namespace FieldDeviceEmulator.Core.EmulatedDevices;

/// <summary>
/// Implementation of a simulated current loop transmitter
/// </summary>
public class SimulatedCurrentLoopTransmitter : ICurrentLoopTransmitter
{
    private Current _lastCurrent;

    /// <summary>
    /// Initializes a new instance of a SimulatedCurrentLoopTransmitter
    /// </summary>
    public SimulatedCurrentLoopTransmitter()
    {
        _lastCurrent = 0.004.Amps();
    }

    /// <summary>
    /// Initializes a new instance of a SimulatedCurrentLoopTransmitter
    /// </summary>
    /// <param name="startCurrent">An intial value for the output current</param>
    public SimulatedCurrentLoopTransmitter(Current startCurrent)
    {
        _lastCurrent = startCurrent;
    }

    /// <inheritdoc/>
    public Current GetOutputCurrent()
    {
        return _lastCurrent;
    }

    /// <inheritdoc/>
    public Task SetOutputCurrent(Current current)
    {
        _lastCurrent = current;
        return Task.CompletedTask;
    }
}
