using Microsoft.Phone.Controls;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace iDrive.Model
{
  public class OrientationChangedMessage
  {

    public PageOrientation Orientation { get; set; }

    public OrientationChangedMessage(PageOrientation Orientation)
    {
      this.Orientation = Orientation;
    }

  }
}
