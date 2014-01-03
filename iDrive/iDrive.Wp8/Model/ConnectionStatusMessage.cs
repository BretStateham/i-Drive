using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace iDrive.Model 
{
  public class ConnectionStatusMessage
  {
    public bool IsConnected { get; set; }

    public ConnectionStatusMessage(bool IsConnected)
    {
      this.IsConnected = IsConnected;
    }
  }
}
