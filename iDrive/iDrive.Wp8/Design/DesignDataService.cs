using iDrive.Model;
using iDrive.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace iDrive.Wp8.Design
{
  class DesignDataService : IDataService
  {
    public async Task<IEnumerable<Model.DeviceInfo>> GetDevicesAsync()
    {
      List<DeviceInfo> devices = new List<DeviceInfo>
      {
        new DeviceInfo { DisplayName = "Sample Device 01", HostName="Host Name 01", ServiceName="Service Name 01" },
        new DeviceInfo { DisplayName = "Sample Device 02", HostName="Host Name 02", ServiceName="Service Name 02" },
        new DeviceInfo { DisplayName = "Sample Device 03", HostName="Host Name 03", ServiceName="Service Name 03" },
        new DeviceInfo { DisplayName = "Sample Device 04", HostName="Host Name 04", ServiceName="Service Name 04" },
        new DeviceInfo { DisplayName = "Sample Device 05", HostName="Host Name 05", ServiceName="Service Name 05" },
        new DeviceInfo { DisplayName = "Sample Device 06", HostName="Host Name 06", ServiceName="Service Name 06" },
        new DeviceInfo { DisplayName = "Sample Device 07", HostName="Host Name 07", ServiceName="Service Name 07" },
        new DeviceInfo { DisplayName = "Sample Device 08", HostName="Host Name 08", ServiceName="Service Name 08" },
        new DeviceInfo { DisplayName = "Sample Device 09", HostName="Host Name 09", ServiceName="Service Name 09" },
        new DeviceInfo { DisplayName = "Sample Device 10", HostName="Host Name 10", ServiceName="Service Name 10" }
      };

      return devices;
    }
  }
}
