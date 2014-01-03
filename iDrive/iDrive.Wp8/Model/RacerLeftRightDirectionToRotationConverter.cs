using iDrive.Service;
using Microsoft.Practices.ServiceLocation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace iDrive.Model
{
  public class RacerLeftRightDirectionToRotationConverter : IValueConverter
  {
    private void ShowMessage(string Message)
    {
      IDialogService dialogService = ServiceLocator.Current.GetInstance<IDialogService>();
      if (dialogService != null)
        dialogService.ShowMessage(Message);
    }

    public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
    {
      double rotation = 0;
      double angle = 20; //default angle of rotation for the tires is 20 degrees
      try
      {
        if(parameter is double)
        {
          angle = (double)parameter;
        }
        if (value is RacerLeftRightDirection)
        {
          RacerLeftRightDirection direction = (RacerLeftRightDirection)value;
          rotation = (double)direction * angle;
        }
      }
      catch (Exception ex)
      {
        string message = string.Format("An {0} occurred when converting the {1} value {2} to an int: {3}", ex.GetType().Name, value.GetType().Name, value, ex.Message);
        ShowMessage(message);
      }
      return rotation;
    }

    public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
    {
      RacerLeftRightDirection direction = RacerLeftRightDirection.None;
      double angle = 20; //default angle of rotation for the tires is 20 degrees
      try
      {
        if (parameter is double)
        {
          angle = (double)parameter;
        }
        double rotation = (double)value;
        direction = (RacerLeftRightDirection)(rotation / angle);
      }
      catch (Exception ex)
      {
        string message = string.Format("An {0} occurred when converting the {1} value {2} to a RacerLeftRightDirection: {3}", ex.GetType().Name, value.GetType().Name, value, ex.Message);
        ShowMessage(message);
      }
      return direction;
    }
  }
}
