using GalaSoft.MvvmLight;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace iDrive.Model
{
  public class DeviceInfo : ObservableObject
  {

    private string displayName;

    public string DisplayName
    {
      get { return displayName; }
      set { Set(ref displayName,value); }
    }

    private string hostName;

    public string HostName
    {
      get { return hostName; }
      set { Set(ref hostName,value); }
    }

    private string serviceName;

    public string ServiceName
    {
      get { return serviceName; }
      set { Set(ref serviceName, value); }
    }

  }
}
