using System;
using System.Threading.Tasks;
namespace iDrive.Model
{
  public interface IRacer : IDisposable
  {

    event EventHandler<RacerConnectionStateChangedEventArgs> RacerConnectionStateChanged;

    bool IsConnected { get; }

    int Speed { get; set; }

    RacerForwardBackwardDirection ForwardBackwardDirection { get; set; }

    RacerLeftRightDirection LeftRightDirection { get; set; }

    byte ControlByte { get; set; }

    Task GoAsync();

    Task StopAsync();

    Task ConnectAsync(DeviceInfo RacerDevice);

    Task DisconnectAsync();
    
  }
}
