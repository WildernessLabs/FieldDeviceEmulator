using Meadow.Units;
using System;
using System.Threading.Tasks;

namespace FieldDeviceEmulator.Core.EmulatedDevices;

/// <summary>
/// Emulates a temperature sensor that outputs a 4-20mA or 0-20mA current loop signal
/// proportional to the measured temperature within a specified range
/// </summary>
public class TemperatureTransmitter : CurrentLoopSensor
{
    /// <summary>
    /// Gets the minimum temperature that this sensor can measure
    /// </summary>
    public Temperature MinimumSenseTemp { get; }

    /// <summary>
    /// Gets the maximum temperature that this sensor can measure
    /// </summary>
    public Temperature MaximumSenseTemp { get; }

    /// <summary>
    /// Gets the current loop range (0-20mA or 4-20mA) for this sensor
    /// </summary>
    public CurrentLoopRange Range { get; }

    /// <summary>
    /// Initializes a new instance of the TemperatureCurrentLoopSensor class
    /// </summary>
    /// <param name="transmitter">The current loop transmitter to use for output</param>
    /// <param name="range">The current loop range (0-20mA or 4-20mA)</param>
    /// <param name="minimumSenseTemp">The minimum temperature the sensor can measure</param>
    /// <param name="maximumSenseTemp">The maximum temperature the sensor can measure</param>
    /// <exception cref="ArgumentException">Thrown when minimum temperature is greater than or equal to maximum temperature</exception>
    public TemperatureTransmitter(
        ICurrentLoopTransmitter transmitter,
        CurrentLoopRange range,
        Temperature minimumSenseTemp,
        Temperature maximumSenseTemp)
        : base(transmitter)
    {
        if (minimumSenseTemp >= maximumSenseTemp)
        {
            throw new ArgumentException("Minimum temperature must be less than maximum temperature",
                nameof(minimumSenseTemp));
        }

        MinimumSenseTemp = minimumSenseTemp;
        MaximumSenseTemp = maximumSenseTemp;
        Range = range;
    }

    /// <summary>
    /// Sets the sensor temperature and outputs the corresponding current loop signal.
    /// The output current is linearly interpolated between the sensor's current range
    /// based on where the temperature falls within the sensor's measurement range.
    /// </summary>
    /// <param name="temperature">The temperature to simulate</param>
    /// <returns>A task representing the asynchronous operation</returns>
    /// <exception cref="ArgumentOutOfRangeException">
    /// Thrown when temperature is outside the sensor's minimum and maximum range
    /// </exception>
    /// <example>
    /// For a 4-20mA sensor with range 0°C to 100°C:
    /// - 0°C outputs 4mA
    /// - 50°C outputs 12mA
    /// - 100°C outputs 20mA
    /// </example>
    public async Task SetTemperature(Temperature temperature)
    {
        // Validate temperature is within sensor range
        if (temperature < MinimumSenseTemp || temperature > MaximumSenseTemp)
        {
            throw new ArgumentOutOfRangeException(nameof(temperature),
                $"Temperature {temperature} is outside sensor range {MinimumSenseTemp} to {MaximumSenseTemp}");
        }

        // Determine current range based on loop type
        double minCurrent = Range == CurrentLoopRange.Current_4_20 ? 4.0 : 0.0;
        double maxCurrent = 20.0;

        // Calculate temperature span and position within range
        double tempSpan = MaximumSenseTemp.Celsius - MinimumSenseTemp.Celsius;
        double tempPosition = temperature.Celsius - MinimumSenseTemp.Celsius;
        double tempRatio = tempPosition / tempSpan;

        // Linear interpolation to map temperature to current
        double currentSpan = maxCurrent - minCurrent;
        double outputMilliamps = minCurrent + (tempRatio * currentSpan);

        // Set the output current
        await SetOutputCurrent(new Current(outputMilliamps, Current.UnitType.Milliamps));
    }
}