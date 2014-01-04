using iDrive.Model;
using iDrive.Service;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Networking.Proximity;

namespace iDrive.Wp8.Service
{
  public class DataService : IDataService
  {
    async public Task<IEnumerable<DeviceInfo>> GetDevicesAsync()
    {
      List<DeviceInfo> devices = new List<DeviceInfo>();

      try
      {
        PeerFinder.AlternateIdentities["Bluetooth:Paired"] = "";
        var pairedDevices = await PeerFinder.FindAllPeersAsync();

        if (pairedDevices.Count == 0)
        {
          //You could launch the Bluetooth settings from here....
          //info: http://msdn.microsoft.com/en-us/library/windowsphone/develop/jj662937(v=vs.105).aspx 
          //exmaple:
          //await Windows.System.Launcher.LaunchUriAsync(new Uri("ms-settings-bluetooth:"));
        
          Debug.WriteLine("No paired devices were found.");
        }
        else
        {
          //The where clause only returns Dagu cars....
          var devs = from p in pairedDevices
                     where p.DisplayName.Contains("Dagu")
                     select new DeviceInfo()
                     {
                       DisplayName = p.DisplayName,
                       HostName = p.HostName.DisplayName,
                       ServiceName = p.ServiceName
                     };

          devices.AddRange(devs);
        }
      }
      catch (Exception ex)
      {
        string message = string.Format("An {0} occurred when trying to find paired devices: {1}", ex.GetType().Name, ex.Message);
        Debug.WriteLine(message);
      }

      return devices;

    }
  }
}
