using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;
using iDrive.Service;
using Microsoft.Practices.ServiceLocation;
using GalaSoft.MvvmLight.Ioc;
using iDrive.Model;
using GalaSoft.MvvmLight.Messaging;
using iDrive.Message;

namespace iDrive.Wp8.View
{
  public partial class MainView : PhoneApplicationPage, IDialogService
  {
    public MainView()
    {
      InitializeComponent();

      this.OrientationChanged += MainView_OrientationChanged;
    }

    void MainView_OrientationChanged(object sender, OrientationChangedEventArgs e)
    {
      Messenger.Default.Send<OrientationChangedMessage>(new OrientationChangedMessage(e.Orientation));

      PageTitle.Text = e.Orientation.ToString();
    }

    protected override void OnNavigatedTo(NavigationEventArgs e)
    {
      SimpleIoc.Default.Register<IDialogService>(() => this);

      Messenger.Default.Register<ConnectionStatusMessage>(this, HandleConnectionStatusMessage);

      racerView.RegisterMessages();

      base.OnNavigatedTo(e);
    }

    protected override void OnNavigatedFrom(NavigationEventArgs e)
    {
      SimpleIoc.Default.Unregister<IDialogService>();

      Messenger.Default.Unregister<ConnectionStatusMessage>(this);

      racerView.UnregisterMessages();

      base.OnNavigatedFrom(e);
    }

    private void HandleConnectionStatusMessage(ConnectionStatusMessage Message)
    {
      if (Message.IsConnected)
        VisualStateManager.GoToState(this, "Connected", true);
      else
        VisualStateManager.GoToState(this, "NotConnected", true);
    }

    public void ShowMessage(string Message)
    {
      MessageBox.Show(Message);
    }

    protected override void OnOrientationChanged(OrientationChangedEventArgs e)
    {
      if(e.Orientation== PageOrientation.PortraitUp || e.Orientation == PageOrientation.LandscapeLeft)
        base.OnOrientationChanged(e);
    }
  }

}