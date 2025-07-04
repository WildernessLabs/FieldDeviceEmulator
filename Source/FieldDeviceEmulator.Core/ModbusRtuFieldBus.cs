using Meadow;
using Meadow.Hardware;
using Meadow.Modbus;
using System;
using System.Collections.Concurrent;
using System.Linq;

namespace FieldDeviceEmulator.Core.EmulatedDevices;

public class ModbusRtuFieldBus : IDisposable
{
    private readonly ModbusRtuServer _server;
    private readonly ConcurrentDictionary<byte, IModbusRtuDevice> _devices = new();

    public bool IsDisposed { get; private set; } = false;

    public ModbusRtuFieldBus(ISerialPort hostPort)
    {
        _server = new ModbusRtuServer(hostPort);
        _server.ReadHoldingRegisterRequest += OnReadHoldingRegisterRequest;
    }

    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }

    protected virtual void Dispose(bool disposing)
    {
        if (!IsDisposed && disposing)
        {
            _server?.Stop();
            _server?.Dispose();
            IsDisposed = true;
        }
    }

    public void Connect()
    {
        _server.Start();
    }

    private IModbusResult? OnReadHoldingRegisterRequest(byte modbusAddress, ushort startRegister, short length)
    {
        try
        {
            Resolver.Log.Info($"Modbus Read Holding Registers Request: Address={modbusAddress}, Start={startRegister}, Length={length}");

            // Validate parameters
            if (length <= 0 || length > 125)
                return new ModbusErrorResult(ModbusErrorCode.IllegalDataValue);

            if (_devices.TryGetValue(modbusAddress, out IModbusRtuDevice device))
            {
                var registers = device.ReadHoldingRegisters(startRegister, length);

                if (registers == null)
                    return new ModbusErrorResult(ModbusErrorCode.IllegalDataAddress);

                var registerArray = registers.ToArray();
                if (registerArray.Length != length)
                    return new ModbusErrorResult(ModbusErrorCode.IllegalDataAddress);

                return new ModbusReadResult(registerArray);
            }
            else
            {
                return new ModbusErrorResult(ModbusErrorCode.IllegalDataAddress);
            }
        }
        catch (Exception ex)
        {
            // Log the exception if logging is available
            System.Diagnostics.Debug.WriteLine($"Modbus server error: {ex.Message}");
            return new ModbusErrorResult(ModbusErrorCode.DeviceFailure);
        }
    }

    public void Add(IModbusRtuDevice device)
    {
        if (!_devices.TryAdd(device.BusAddress, device))
        {
            throw new Exception("Device already exists at that address");
        }
    }
}
