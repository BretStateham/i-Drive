using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using iDrive.Model;
using iDrive.Service;
using Microsoft.Practices.ServiceLocation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Networking;
using Windows.Networking.Proximity;
using Windows.Networking.Sockets;

namespace iDrive.Wp8.ViewModel
{
  public class MainViewModel : ViewModelBase
  {

    public RelayCommand ConnectCommand { get; set; }

    public RelayCommand DisconnectCommand { get; set; }

    public RelayCommand GoCommand { get; set; }

    private List<DeviceInfo> devices;

    private IDataService dataService;

    public List<DeviceInfo> Devices
    {
      get { return devices; }
      set { Set(ref devices, value); }
    }

    private DeviceInfo racerDeviceInfo;

    public DeviceInfo RacerDeviceInfo
    {
      get { return racerDeviceInfo; }
      set
      {
        Set(ref racerDeviceInfo, value);
        ConnectCommand.RaiseCanExecuteChanged();
      }
    }

    private IRacer racer;

    public IRacer Racer
    {
      get { return racer; }
      set
      {
        Set(ref racer, value);
        DisconnectCommand.RaiseCanExecuteChanged();
        GoCommand.RaiseCanExecuteChanged();
      }
    }

    public MainViewModel()
    {

      dataService = ServiceLocator.Current.GetInstance<IDataService>();

      ConnectCommand = new RelayCommand(ConnectToiRacer, () => racerDeviceInfo != null);

      DisconnectCommand = new RelayCommand(DisconnectFromiRacer, () => racer != null);

      GoCommand = new RelayCommand(Go, () => racer != null && racer.IsConnected);

      PopulateDevices();
    }

    private async void PopulateDevices()
    {
      if (dataService != null)
      {
        var devs = new List<DeviceInfo>();
        devs.AddRange(await dataService.GetDevicesAsync());
        Devices = devs;
      }
    }

    private async void ConnectToiRacer()
    {
      try
      {
        if (racerDeviceInfo != null)
        {
          ////First connect to the bluetooth device...
          //HostName deviceHost = new HostName(racerDeviceInfo.HostName);

          ////If the socket is already in use, dispose of it
          //if (socket != null)
          //  socket.Dispose();

          ////Create a new socket
          //socket = new StreamSocket();
          //await socket.ConnectAsync(deviceHost, "1");

          //this.Racer = new Racer(socket);

          //AccelerometerCommandProvider provider = new AccelerometerCommandProvider();
          //Racer.CommandProvider = provider;

          //Dispose ofthe Racer if it is disposable, and destory it...
          DisposeRacer();

          this.Racer = new Racer();

          await this.Racer.ConnectAsync(racerDeviceInfo);

          this.Racer.CommandProvider = new AccelerometerCommandProvider();

        }
      }
      catch (Exception ex)
      {
        string message = string.Format("An {0} occurred when trying to connect: {1}", ex.GetType().Name, ex.Message);
        ShowMessage(message);
      }
      finally
      {
        GoCommand.RaiseCanExecuteChanged();
      }
    }

    private void DisposeRacer()
    {
      if (this.Racer != null && this.Racer is IDisposable)
      {
        ((IDisposable)Racer).Dispose();
        this.Racer = null;
      }
    }

    private async void DisconnectFromiRacer()
    {
      try
      {
        if (Racer != null && Racer.IsConnected)
        {
          //Stop the car just in case it is still running
          await Racer.StopAsync();

          await Racer.DisconnectAsync();

          //Dispose ofthe Racer if it is disposable, and destory it...
          DisposeRacer();
        }
        ////Dispose of the socket...
        //socket.Dispose();
        //socket = null;

        ////Kill the iRacer
        //Racer = null;
      }
      catch (Exception ex)
      {
        string message = string.Format("An {0} occurred when trying to dis-connect: {1}", ex.GetType().Name, ex.Message);
        ShowMessage(message);
      }
      finally
      {
        GoCommand.RaiseCanExecuteChanged();
      }
    }

    private void Go()
    {
      if (racer != null && racer.IsConnected)
      {
        racer.ControlByte = 0x1F;
        racer.GoAsync();
      }
    }

    private void ShowMessage(string Message)
    {
      IDialogService dialogService = ServiceLocator.Current.GetInstance<IDialogService>();
      if (dialogService != null)
        dialogService.ShowMessage(Message);
    }

  }
}
