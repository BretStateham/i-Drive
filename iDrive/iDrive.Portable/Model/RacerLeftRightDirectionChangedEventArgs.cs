using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace iDrive.Model
{
  public class RacerLeftRightDirectionChangedEventArgs : EventArgs
  {
    public RacerLeftRightDirection Direction { get; set; }

    public RacerLeftRightDirectionChangedEventArgs(RacerLeftRightDirection Direction)
    {
      this.Direction = Direction;
    }
  }
}
