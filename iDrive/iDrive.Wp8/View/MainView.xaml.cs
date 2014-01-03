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

namespace iDrive.Wp8.View
{
  public partial class MainView : PhoneApplicationPage, IDialogService
  {
    public MainView()
    {
      InitializeComponent();
    }

    protected override void OnNavigatedTo(NavigationEventArgs e)
    {
      SimpleIoc.Default.Register<IDialogService>(() => this);

      base.OnNavigatedTo(e);
    }

    protected override void OnNavigatedFrom(NavigationEventArgs e)
    {
      SimpleIoc.Default.Unregister<IDialogService>();

      base.OnNavigatedFrom(e);
    }

    public void ShowMessage(string Message)
    {
      MessageBox.Show(Message);
    }
  }
}