using iDrive.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;

namespace iDrive.Model
{
  public class RacerForwardBackwardDirectionToVisibilityConverter : IValueConverter
  {
    public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
    {
      Visibility visibility = Visibility.Visible;
      
      bool invert = false;
      bool.TryParse(parameter.ToString(), out invert);

      if (value is RacerForwardBackwardDirection)
      {
        var direction = (RacerForwardBackwardDirection)value;
        if ((direction == RacerForwardBackwardDirection.Forward && !invert) || direction == RacerForwardBackwardDirection.Backward && invert)
          visibility = Visibility.Visible;
        else
          visibility = Visibility.Collapsed;
      }

      return visibility;
    }

    public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
    {
      throw new NotImplementedException();
    }
  }
}
