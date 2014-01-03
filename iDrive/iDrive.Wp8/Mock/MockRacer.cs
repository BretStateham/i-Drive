using GalaSoft.MvvmLight;
using iDrive.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace iDrive.Mock
{
  public class MockRacer : ObservableObject, IRacer
  {
    private bool isConnected;

    public bool IsConnected
    {
      get { return isConnected; }
      set { Set(ref isConnected, value); }
    }

    private int speed;

    public int Speed
    {
      get { return speed; }
      set { Set(ref speed, value); }
    }

    private RacerForwardBackwardDirection forwardBackwardDirection;

    public RacerForwardBackwardDirection ForwardBackwardDirection
    {
      get { return forwardBackwardDirection; }
      set { Set(ref forwardBackwardDirection, value); }
    }


    private RacerLeftRightDirection leftRightDirection;

    public RacerLeftRightDirection LeftRightDirection
    {
      get { return leftRightDirection; }
      set { Set(ref leftRightDirection, value); }
    }


    private byte controlByte;

    public byte ControlByte
    {
      get { return controlByte; }
      set { Set(ref controlByte, value); }
    }
    

    public Task GoAsync()
    {
      throw new NotImplementedException();
    }

    private IRacerCommandProvider commandProvider;

    public IRacerCommandProvider CommandProvider
    {
      get { return commandProvider; }
      set { Set(ref commandProvider, value); }
    }

    
  }
}
