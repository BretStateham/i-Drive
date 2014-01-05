using iDrive.Model;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace iDrive.Mock
{
  public class MockRacer : RacerBase
  {

    private DeviceInfo racerDevice;

    private bool isConnected = false;
    override public bool IsConnected
    {
      get { return isConnected; }
    }

    public override async Task GoAsync()
    {
      string goMessage = string.Format ("Going: {0},{1},{2} ({3})",LeftRightDirection,ForwardBackwardDirection,Speed,ControlByte);
      Debug.WriteLine(goMessage);
    }

    public override async Task StopAsync()
    {
      Debug.WriteLine("Stopped");
    }

    public override async Task ConnectAsync(DeviceInfo RacerDevice)
    {
      racerDevice = RacerDevice;

      string connectMessage = string.Format("Connecting to:\n{0}", racerDevice.ToString());
      Debug.WriteLine(connectMessage);

      isConnected = true;
      RaiseRacerConnectionStateChanged();
    }

    public override async Task DisconnectAsync()
    {
      string connectMessage = string.Format("Disconnecting From:\n{0}", racerDevice.ToString());
      Debug.WriteLine(connectMessage);


      isConnected = false;
      RaiseRacerConnectionStateChanged();
    }
  }
}
