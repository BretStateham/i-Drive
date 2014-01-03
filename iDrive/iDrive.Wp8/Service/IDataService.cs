using iDrive.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace iDrive.Service
{
  public interface IDataService
  {
    Task<IEnumerable<DeviceInfo>> GetDevicesAsync();
  }
}
