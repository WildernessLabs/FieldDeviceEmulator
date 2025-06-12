using Meadow.Units;
using System.Collections.Generic;

namespace FieldDeviceEmulator.Core.EmulatedDevices;

public class CerusXDrive : IModbusRtuDevice
{
    public byte BusAddress { get; }

    public CerusXDrive(byte busAddress)
    {
        BusAddress = busAddress;

        OutputCurrent = 42.2.Amps();
        OutputFrequency = 120.1.Hertz();
        AmbientTemperature = 74.Fahrenheit();
        IGBTTemperature = 92.6.Fahrenheit();
        DCBusVoltage = 413.Volts();
        OutputVoltage = 120.1.Volts();
    }

    public Frequency OutputFrequency { get; set; }
    public Current OutputCurrent { get; set; }
    public Temperature AmbientTemperature { get; set; }
    public ushort DriveStatus { get; set; }
    public ushort ErrorCodes { get; set; }
    public ushort OperationalStatus { get; set; }
    public Voltage DCBusVoltage { get; set; }
    public Voltage OutputVoltage { get; set; }
    public Temperature IGBTTemperature { get; set; }
    public ushort ControlMode { get; set; }
    public ushort DigitalInputStatus { get; set; }
    public ushort DigitalOutputStatus { get; set; }

    public IEnumerable<ushort>? ReadHoldingRegisters(ushort startRegister, short count)
    {
        if (count != 1)
        {
            return null;
        }

        switch ((Registers)startRegister)
        {
            case Registers.OutputFrequency:
                // register is frequency * 100
                return [(ushort)(OutputFrequency.Hertz * 100)];
            case Registers.OutputCurrent:
                // register is current * 10
                return [(ushort)(OutputCurrent.Amps * 10)];
            case Registers.AmbientTemperature:
                return [(ushort)(AmbientTemperature.Celsius * 10)];
            case Registers.ErrorCode:
                return [ErrorCodes];
            case Registers.OperationStatus:
                return [OperationalStatus];
            case Registers.DCBusVoltage:
                return [(ushort)(DCBusVoltage.Volts * 10)];
            case Registers.OutputVoltage:
                return [(ushort)(OutputVoltage.Volts * 10)];
            case Registers.DriveStatus:
                return [DriveStatus];
            case Registers.IGBTTemperature:
                return [(ushort)(IGBTTemperature.Celsius * 10)];
            case Registers.ControlMode:
                return [ControlMode];
            case Registers.DigitalInputStatus:
                return [DigitalInputStatus];
            case Registers.DigitalOutputStatus:
                return [DigitalOutputStatus];
            default:
                return null;
        }
    }

    internal enum Registers
    {
        RunCommand = 8192,
        FrequencyCommand = 8193,
        FaultControlCommand = 8194,

        ErrorCode = 8448,
        OperationStatus = 8449,
        FrequencyCommandValue = 8450,
        OutputFrequency = 8451,
        OutputCurrent = 8452,
        DCBusVoltage = 8453,
        OutputVoltage = 8454,
        MultiStepSpeed = 8455,
        // Reserved = 8456,
        CounterValue = 8457,
        PowerFactorAngle = 8458,
        Torque = 8459,
        MotorSpeed = 8460,
        // Reserved = 8461,
        // Reserved = 8462,
        OutputPower = 8463,
        MultiFunctionDisplay = 8470,
        MaximumOperatingFrequency = 8475,
        DecimalPortionOfOutputCurrent = 8479,

        // Additional status registers
        OutputCurrent2 = 8704,
        CounterValue2 = 8705,
        OutputFrequency2 = 8706,
        DCBusVoltage2 = 8707,
        OutputVoltage2 = 8708,
        PowerAngle = 8709,
        MotorPower = 8710,
        MotorSpeed2 = 8711,
        Torque2 = 8712,
        // Reserved = 8713,
        PIDFeedbackValue = 8714,
        AVI1InputValuePercentage = 8715,
        ACIInputValuePercentage = 8716,
        AVI2InputValuePercentage = 8717,
        IGBTTemperature = 8718,
        AmbientTemperature = 8719,
        DigitalInputStatus = 8720,
        DigitalOutputStatus = 8721,
        MultiStepSpeedBeingExecuted = 8722,
        CPUPinStatusForDigitalInputs = 8723,
        CPUPinStatusForDigitalOutputs = 8724,
        // Reserved = 8725,
        // Reserved = 8726,
        // Reserved = 8727,
        // Reserved = 8728,
        CounterOverloadTimePercentage = 8729,
        GFFPercentage = 8730,
        DCBusRipple = 8731,
        PLCRegisterD1043Data = 8732,
        // Reserved = 8733,
        UserPageDisplay = 8734,
        OutputValueOfOutputFrequencyCoefficient = 8735,
        NumberOfMotorRevolutionsWhileRunning = 8736,
        OperatingPositionOfTheMotor = 8737,
        VFDCoolingFanSpeed = 8738,
        ControlMode = 8739,
        CarrierFrequencyStatus = 8740,
        // Reserved = 8741,
        DriveStatus = 8742,
        // Reserved = 8743,
        // Reserved = 8744,
        Power = 8745,
        AVI1PT100 = 8746,
        ACIPT100 = 8747,
        // Reserved = 8748,
        // Reserved = 8749,
        PIDReferenceValue = 8750,
        PIDOffsetValue = 8751,
        PIDOutputFrequency = 8752,
        HardwareID = 8753,
        UPhaseCurrent = 8754,
        VPhaseCurrent = 8755,
        WPhaseCurrent = 8756,
        AuxAnalogInput = 8759,
        TorquePercentage = 8762,

        DigitalInputStatus1 = 9729,
        DigitalInputStatus2 = 9730,
        DigitalOutputStatus1 = 9733,

        AVI1ProportionalValue = 9825,
        ACIProportionalValue = 9826,
        AVI2ProportionalValue = 9827,
        ExpansionCardAI10Percentage = 9835,
        ExpansionCardAI11Percentage = 9836,
        AO1Percentage = 9856,
        AO2Percentage = 9857,
        AFM1OutputProportionalValue = 9889,
        AFM2OutputProportionalValue = 9890,
        ExpansionCardAO10Percentage = 9899,
        ExpansionCardAO11Percentage = 9900
    }
}
