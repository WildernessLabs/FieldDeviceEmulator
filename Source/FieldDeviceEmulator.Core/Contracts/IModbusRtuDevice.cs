using System.Collections.Generic;

namespace FieldDeviceEmulator.Core.EmulatedDevices;

public interface IModbusRtuDevice
{
    byte BusAddress { get; }
    IEnumerable<ushort>? ReadHoldingRegisters(ushort startRegister, short count);
}
