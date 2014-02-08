using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace iDrive.Model
{
  public class RacerConnectionStateChangedEventArgs : EventArgs
  {

    public bool IsConnected { get; set; }

    public RacerConnectionStateChangedEventArgs(bool IsConnected)
    {
      this.IsConnected = IsConnected;
    }

  }
}
