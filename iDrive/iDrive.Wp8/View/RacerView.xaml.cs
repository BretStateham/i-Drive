using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;
using iDrive.Model;
using System.Windows.Data;
using System.Diagnostics;
using iDrive.Service;
using Microsoft.Practices.ServiceLocation;

namespace iDrive.Wp8.View
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
      int rotation = 0;
      try
      {
        if (value is int)
        {
          RacerLeftRightDirection direction = (RacerLeftRightDirection)value;
          rotation = (int)direction * 20;
        }
      }
      catch(Exception ex)
      {
        string message = string.Format("An {0} occurred when converting the {1} value {2} to an int: {3}", ex.GetType().Name, value.GetType().Name, value, ex.Message);
        ShowMessage(message);   
      }
      return rotation;
    }

    public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
    {
      RacerLeftRightDirection direction = RacerLeftRightDirection.None;
      try
      {
        int rotation = (int)value;
        direction = (RacerLeftRightDirection)(rotation / 20);
      }
      catch(Exception ex)
      {
        string message = string.Format("An {0} occurred when converting the {1} value {2} to a RacerLeftRightDirection: {3}", ex.GetType().Name, value.GetType().Name, value, ex.Message);
        ShowMessage(message);  
      }
      return direction;
    }
  }

  public partial class RacerView : UserControl
  {
    public RacerView()
    {
      InitializeComponent();
    }
  }
}
