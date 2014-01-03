using GalaSoft.MvvmLight;
using iDrive.Model;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Threading;
using Windows.Networking;
using Windows.Networking.Sockets;
using Windows.Storage.Streams;

namespace iDrive.Model
{

  public class Racer : ObservableObject, IRacer
  {

    StreamSocket streamSocket = null;
    DataWriter dataWriter = null;

    public bool IsConnected
    {
      get { return streamSocket != null && dataWriter != null; }
    }

    private int speed;

    public int Speed
    {
      get { return speed; }
      set
      {
        Deployment.Current.Dispatcher.BeginInvoke(() =>
        {
          Set(ref speed, value);
          SetControlByte();
        });
      }
    }

    private RacerLeftRightDirection leftRightDirection;

    public RacerLeftRightDirection LeftRightDirection
    {
      get { return leftRightDirection; }
      set
      {
        Deployment.Current.Dispatcher.BeginInvoke(() =>
        {
          Set(ref leftRightDirection, value);
          SetControlByte();
        });
      }
    }

    private RacerForwardBackwardDirection forwardBackwardDirection;

    public RacerForwardBackwardDirection ForwardBackwardDirection
    {
      get { return forwardBackwardDirection; }
      set
      {
        Deployment.Current.Dispatcher.BeginInvoke(() =>
        {
          Set(ref forwardBackwardDirection, value);
          SetControlByte();
        });
      }
    }

    private byte controlByte = 0;

    public byte ControlByte
    {
      get { return controlByte; }
      set
      { Set(ref controlByte, value); }
    }
    public async Task GoAsync()
    {
      if (IsConnected)
      {
        //Send all control commands on the UI Thread.  
        //This helps prevent Invalid Cross-Thread Access exceptions
        //When binding the racer to the UI.
        Deployment.Current.Dispatcher.BeginInvoke(async () =>
          {
            dataWriter.WriteByte(ControlByte);
            await dataWriter.StoreAsync();
          });
      }
    }

    public async Task StopAsync()
    {
      if (IsConnected)
      {
        LeftRightDirection = RacerLeftRightDirection.None;
        ForwardBackwardDirection = RacerForwardBackwardDirection.None;
        Speed = 0;
        await GoAsync();
      }
    }

    /// <summary>
    /// Sets the iRacer Control byte based on the LeftRightDirection and ForwardBackwardDirection property values.  
    /// </summary>
    private void SetControlByte()
    {

      int dirvalue = 0;
      string direction = string.Format("{0}{1}", LeftRightDirection.ToString()[0], forwardBackwardDirection.ToString()[0]).ToUpper();
      Debug.WriteLine("Direction: {0}", direction);
      switch (direction)
      {
        case "NN": //stopped
          dirvalue = 0;
          break;
        case "NF": //straight forward
          dirvalue = 1;
          break;
        case "NB": //straight backward
          dirvalue = 2;
          break;
        case "LN": //left none
          dirvalue = 3;
          break;
        case "RN": //right none
          dirvalue = 4;
          break;
        case "LF": //left forward
          dirvalue = 5;
          break;
        case "RF": //right forward
          dirvalue = 6;
          break;
        case "LB": //left back
          dirvalue = 7;
          break;
        case "RB": //right back
          dirvalue = 8;
          break;
        default:
          dirvalue = 0;
          break;
      }

      //Clamp the speed value between 0 (stopped) and 15 (fastest)
      //If there is no direction, ensure the speed is zero.
      int speedvalue = (dirvalue != 0) ? Math.Max(Math.Min(Speed, 15), 0) : 0;

      dirvalue <<= 4;

      ControlByte = (byte)(dirvalue | speedvalue);
    }

    #region Connection Management

    public event EventHandler<RacerConnectionStateChangedEventArgs> RacerConnectionStateChanged;
		
    private void CloseConnection()
    {
      if (dataWriter != null)
      {
        dataWriter.Dispose();
        dataWriter = null;
      }

      if (streamSocket != null)
      {
        streamSocket.Dispose();
        streamSocket = null;
      }
    }

    public async Task ConnectAsync(DeviceInfo RacerDevice)
    {
      try
      {
        //First connect to the bluetooth device...
        HostName deviceHost = new HostName(RacerDevice.HostName);

        //If the socket is already in use, dispose of it
        if (streamSocket != null || dataWriter != null)
          CloseConnection();

        //Create a new socket
        streamSocket = new StreamSocket();
        await streamSocket.ConnectAsync(deviceHost, "1");

        dataWriter = new DataWriter(streamSocket.OutputStream);
        RaisePropertyChanged("IsConnected");
      }
      catch(Exception ex)
      {
        //Dispose and Destroy the StreamSocket and DataWriter
        CloseConnection();

        //Not sure what to do here yet, just pass it along
        throw;
      }
      finally
      {
        //Regardless of what happened, let the view know that the connection state likely changed. 
        RaiseRacerConnectionStateChanged();
      }
    }

    public async Task DisconnectAsync()
    {
      try
      {
        if (IsConnected)
          CloseConnection();
      }
      catch
      {
        throw;
      }
      finally
      {
        RaiseRacerConnectionStateChanged();
      }
    }

    private void RaiseRacerConnectionStateChanged()
    {
      //Raise the RacerConnectionStateChangedEvent
      if (RacerConnectionStateChanged != null)
        RacerConnectionStateChanged(this, new RacerConnectionStateChangedEventArgs(IsConnected));

      //Also send a INotifyPropertyChanged event notification for the IsConnected property....
      RaisePropertyChanged("IsConnected");
    }


    #endregion Connection Management

    #region IRacerCommandProvider Implementation

    //private IRacerCommandProvider commandProvider;
    //public IRacerCommandProvider CommandProvider
    //{
    //  get { return commandProvider; }
    //  set
    //  {
    //    //unsubscribe from events
    //    if (commandProvider != null)
    //    {
    //      commandProvider.RacerForwardBackwardDirectionChanged -= OnForwardBackwardDirectionChanged;
    //      commandProvider.RacerLeftRightDirectionChanged -= OnLeftRightDirectionChanged;
    //      commandProvider.RacerSpeedChanged -= OnSpeedChanged;
    //      commandProvider = null;
    //    }

    //    if (value != null)
    //    {
    //      commandProvider = value;
    //      commandProvider.RacerForwardBackwardDirectionChanged += OnForwardBackwardDirectionChanged;
    //      commandProvider.RacerLeftRightDirectionChanged += OnLeftRightDirectionChanged;
    //      commandProvider.RacerSpeedChanged += OnSpeedChanged;
    //    }
    //  }
    //}

    //async void OnSpeedChanged(object sender, RacerSpeedChangedEventArgs e)
    //{
    //  Speed = e.Speed;
    //  await GoAsync();
    //}

    //void OnLeftRightDirectionChanged(object sender, RacerLeftRightDirectionChangedEventArgs e)
    //{
    //  LeftRightDirection = e.Direction;
    //  Deployment.Current.Dispatcher.BeginInvoke(async () => await GoAsync());
    //  //await GoAsync();
    //}

    //async void OnForwardBackwardDirectionChanged(object sender, RacerForwardBackwardDirectionChangedEventArgs e)
    //{
    //  ForwardBackwardDirection = e.Direction;
    //  await GoAsync();
    //}

    #endregion IRacerCommandProvider Implementation

    #region IDisposable Implementation
    ~Racer()
    {
      Dispose(false);
    }

    public void Dispose()
    {
      Dispose(true);
      GC.SuppressFinalize(this);
    }

    private void Dispose(bool disposing)
    {
      if (disposing)
      {
        CloseConnection();
      }
    }

    #endregion IDisposable Implementation
  }
}
