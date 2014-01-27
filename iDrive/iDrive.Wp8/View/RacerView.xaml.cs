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
using GalaSoft.MvvmLight.Messaging;
using iDrive.Message;

namespace iDrive.Wp8.View
{

  public partial class RacerView : UserControl
  {
    public RacerView()
    {
      InitializeComponent();

      VisualStateManager.GoToState(this, "Breathing", true);
      VisualStateManager.GoToState(this, "PowerOn", true);
      VisualStateManager.GoToState(this, "NotConnected", true);

    }

    public void RegisterMessages()
    {
      Messenger.Default.Register<ConnectionStatusMessage>(this, HandleConnectionStatusMessage);
    }

    public void UnregisterMessages()
    {
      Messenger.Default.Unregister<ConnectionStatusMessage>(this);
    }

    private void HandleConnectionStatusMessage(ConnectionStatusMessage  Message)
    {
      if (Message.IsConnected)
        VisualStateManager.GoToState(this, "Connected", false);
      else
        VisualStateManager.GoToState(this, "NotConnected", false);
    }

    
  }
}
