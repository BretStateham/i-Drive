using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace iDrive.Model
{
  public class RacerForwardBackwardDirectionChangedEventArgs : EventArgs
  {
    public RacerForwardBackwardDirection Direction { get; set; }

    public RacerForwardBackwardDirectionChangedEventArgs(RacerForwardBackwardDirection Direction)
    {
      this.Direction = Direction;
    }
  }
}
