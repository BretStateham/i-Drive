using System;
using System.Threading.Tasks;
namespace iDrive.Model
{
  public interface IRacer
  {

    bool IsConnected { get; }

    int Speed { get; set; }

    RacerForwardBackwardDirection ForwardBackwardDirection { get; set; }

    RacerLeftRightDirection LeftRightDirection { get; set; }

    byte ControlByte { get; set; }

    Task GoAsync();

    IRacerCommandProvider CommandProvider { get; set; }
    
  }
}
