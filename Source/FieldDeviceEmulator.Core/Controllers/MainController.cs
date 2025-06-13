using Meadow;
using System;
using System.Threading.Tasks;

namespace FieldDeviceEmulator.Core;

public class MainController
{
    private IEmulatorHardware _hardware;
    private DisplayController _displayController;

    public async Task Initialize(IEmulatorHardware hardware)
    {
        _hardware = hardware;

        _displayController = new DisplayController(_hardware);
    }

    public async Task Run()
    {
        while (true)
        {
            // add any app logic here

            try
            {
                await Task.Delay(1000);
            }
            catch (AggregateException e)
            {
                Resolver.Log.Error(e.InnerException.ToString());
                throw e;
            }
        }
    }
}
