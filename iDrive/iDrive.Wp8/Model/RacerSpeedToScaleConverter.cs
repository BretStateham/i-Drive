using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace iDrive.Model
{
  public class RacerSpeedToScaleConverter : IValueConverter
  {
    public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
    {
      double scale = 1d;
      double max = 15;

      if (parameter is double)
        max = (double)parameter;

      if (value is int)
      {
        var speed = (int)value;
        scale = speed / max;
      }
      return scale;
    }

    public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
    {
      throw new NotImplementedException();
    }
  }
}
