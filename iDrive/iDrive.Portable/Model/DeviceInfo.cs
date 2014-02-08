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

    public override string ToString()
    {
      string deviceInfo = string.Format("DeviceInfo:{0}  Display Name: {1}{0}  Host Name:    {2}{0}  Service Name: {3}", System.Environment.NewLine, DisplayName, HostName, ServiceName);
      return deviceInfo;
    }

  }
}
