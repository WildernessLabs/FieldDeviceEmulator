using FieldDeviceEmulator.Core;
using Meadow;
using Meadow.Foundation.Displays;
using System.Threading.Tasks;

namespace FieldDeviceEmulator;

public class MeadowApp : App<Desktop>
{
    private MainController _mainController;

    public override Task Initialize()
    {
        Device.Display?.Resize(320, 240, 2);

        var hardware = new DesktopEmulatorHardware(Device);
        _mainController = new MainController();
        return _mainController.Initialize(hardware);

    }

    public override Task Run()
    {
        // this must be spawned in a worker because the UI needs the main thread
        _ = _mainController.Run();

        // NOTE: this will not return until the display is closed
        ExecutePlatformDisplayRunner();

        return Task.CompletedTask;
    }

    private void ExecutePlatformDisplayRunner()
    {
        if (Device.Display is SilkDisplay sd)
        {
            sd.Run();
        }
        MeadowOS.TerminateRun();
        System.Environment.Exit(0);
    }
}