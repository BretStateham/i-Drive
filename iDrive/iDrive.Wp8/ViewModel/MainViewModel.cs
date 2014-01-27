using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Messaging;
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
using Microsoft.Devices;
using iDrive.CommandProvider;
using iDrive.Message;

namespace iDrive.Wp8.ViewModel
{
  public class MainViewModel : ViewModelBase
  {

    private AccelerometerCommandProvider accelerometerCommandProvider;

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
        if (racer != value)
        {
          //Delete the existing racer if it exists....
          if (racer != null)
            DestroyRacer();

          //Assign the field to the new value (if there is one)...
          //And subscribe to it's OnRacerConnectionStateChanged event....
          if (value != null)
          {
            racer = value;
            racer.RacerConnectionStateChanged += OnRacerConnectionStateChanged;
          }

          //Manually raise the INotifyPropertyChanged PropertyChanged event...
          RaisePropertyChanged("Racer");

          //Rase CanExecuteChangedEvents...
          DisconnectCommand.RaiseCanExecuteChanged();
          GoCommand.RaiseCanExecuteChanged();
        }
      }
    }

    void OnRacerConnectionStateChanged(object sender, RacerConnectionStateChangedEventArgs e)
    {
      ConnectionStatusMessage connectionMsg = new ConnectionStatusMessage(e.IsConnected);
      Messenger.Default.Send<ConnectionStatusMessage>(connectionMsg);
    }

    public MainViewModel()
    {

      dataService = ServiceLocator.Current.GetInstance<IDataService>();

      ConnectCommand = new RelayCommand(ConnectToiRacer, () => racerDeviceInfo != null);

      DisconnectCommand = new RelayCommand(DisconnectFromiRacer, () => racer != null);

      GoCommand = new RelayCommand(Go, () => racer != null && racer.IsConnected);

      PopulateDevices();

      RegisterMessages();
    }

    private void RegisterMessages()
    {
      Messenger.Default.Register<OrientationChangedMessage>(this, HandleOrientationChanged);
    }

    private void UnregisterMessages()
    {
      Messenger.Default.Unregister<OrientationChangedMessage>(this);
    }

    private void HandleOrientationChanged(OrientationChangedMessage Message)
    {
      if (accelerometerCommandProvider != null)
        accelerometerCommandProvider.Orientation = Message.Orientation;
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

    private void DestroyRacer()
    {
      //To prevent an infinite loop, 
      //make sure to work with the racer field, not the Racer property
      
      if (this.racer != null)
      {
        //Unsubscribe from the RacerConnectionStateChanged event...
        racer.RacerConnectionStateChanged -= OnRacerConnectionStateChanged;

        Racer.Dispose();
        this.racer = null;

        //Get rid of the accelerometerCommandProvider;
        accelerometerCommandProvider.Dispose();
        accelerometerCommandProvider = null;

        GoCommand.RaiseCanExecuteChanged();
        DisconnectCommand.RaiseCanExecuteChanged();
      }
    }


    private async void ConnectToiRacer()
    {
      try
      {
        if (racerDeviceInfo != null)
        {
          //Get rid of the command provider
          if (accelerometerCommandProvider != null)
            accelerometerCommandProvider = null;

          //Dispose of the Racer and destory it if it exists...
          DestroyRacer();

          //Create a new Racer...
          //If running on the device, create real racer
          //Otherwise, (running on the emulator) create a mock racer
          if (Microsoft.Devices.Environment.DeviceType == DeviceType.Device)
            this.Racer = new Racer();
            //this.Racer = new Mock.MockRacer();
          else
            this.Racer = new Mock.MockRacer();


          //Connect it to the selected device
          await this.Racer.ConnectAsync(racerDeviceInfo);

          //Assign the AccelerometerCommandProvider to control it
          //this.Racer.CommandProvider = new AccelerometerCommandProvider();
          accelerometerCommandProvider = new AccelerometerCommandProvider();
          accelerometerCommandProvider.Racer = this.Racer;
        }
      }
      catch (Exception ex)
      {
        string message = string.Format("An {0} occurred when trying to connect: {1}", ex.GetType().Name, ex.Message);
        ShowMessage(message);
      }

    }

    private async void DisconnectFromiRacer()
    {
      try
      {
        if (Racer != null)
        {
          //Stop the car just in case it is still running
          await Racer.StopAsync();

          await Racer.DisconnectAsync();

          //Dispose ofthe Racer and destory it...
          DestroyRacer();
        }
      }
      catch (Exception ex)
      {
        string message = string.Format("An {0} occurred when trying to dis-connect: {1}", ex.GetType().Name, ex.Message);
        ShowMessage(message);
      }
    }

    /// <summary>
    /// This is just a test method to perform a go operation without having to have a command provider.
    /// </summary>
    private void Go()
    {
      if (this.Racer != null)
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
