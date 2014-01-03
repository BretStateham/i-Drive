using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace iDrive.Model
{
  public class RacerSpeedChangedEventArgs : EventArgs
  {
    public int Speed { get; set; }

    public RacerSpeedChangedEventArgs(int Speed)
    {
      this.Speed = Speed;
    }
  }
}
