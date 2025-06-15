using FieldDeviceEmulator.Core;
using Meadow.Devices;
using System.Threading.Tasks;

namespace FieldDeviceEmulator.ProjLab;

public class MeadowApp : ProjectLabCoreComputeApp
{
    public override async Task Run()
    {
        var emulatorHardware = new ProjectLabEmulatorHardware(Hardware);
        var mainController = new MainController();
        await mainController.Initialize(emulatorHardware);
    }
}