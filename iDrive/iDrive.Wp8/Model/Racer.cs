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
using Windows.Networking.Sockets;
using Windows.Storage.Streams;

namespace iDrive.Model
{

  public class Racer : ObservableObject, IRacer
  {
    StreamSocket socket = null;
    DataWriter dataWriter = null;

    public bool IsConnected
    {
      get { return socket != null && dataWriter != null; }
    }

    public Racer(StreamSocket StreamSocket)
    {
      if (StreamSocket != null)
      {
        this.socket = StreamSocket;
        dataWriter = new DataWriter(socket.OutputStream);
        RaisePropertyChanged("IsConnected");
      }
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

    private IRacerCommandProvider commandProvider;
    public IRacerCommandProvider CommandProvider
    {
      get { return commandProvider; }
      set
      {
        //unsubscribe from events
        if (commandProvider != null)
        {
          commandProvider.RacerForwardBackwardDirectionChanged -= OnForwardBackwardDirectionChanged;
          commandProvider.RacerLeftRightDirectionChanged -= OnLeftRightDirectionChanged;
          commandProvider.RacerSpeedChanged -= OnSpeedChanged;
          commandProvider = null;
        }

        if (value != null)
        {
          commandProvider = value;
          commandProvider.RacerForwardBackwardDirectionChanged += OnForwardBackwardDirectionChanged;
          commandProvider.RacerLeftRightDirectionChanged += OnLeftRightDirectionChanged;
          commandProvider.RacerSpeedChanged += OnSpeedChanged;
        }
      }
    }

    async void OnSpeedChanged(object sender, RacerSpeedChangedEventArgs e)
    {
      Speed = e.Speed;
      await GoAsync();
    }

    async void OnLeftRightDirectionChanged(object sender, RacerLeftRightDirectionChangedEventArgs e)
    {
      LeftRightDirection = e.Direction;
      await GoAsync();
    }

    async void OnForwardBackwardDirectionChanged(object sender, RacerForwardBackwardDirectionChangedEventArgs e)
    {
      ForwardBackwardDirection = e.Direction;
      await GoAsync();
    }

    public async Task GoAsync()
    {
      if (dataWriter != null)
      {
        dataWriter.WriteByte(ControlByte);
        await dataWriter.StoreAsync();
      }

    }
    /// <summary>
    /// Sets the iRacer Control byte based on the LeftRightDirection and ForwardBackwardDirection property values.  
    /// </summary>
    private void SetControlByte()
    {

      //Clamp the speed value between 0 (stopped) and 15 (fastest)
      int speedvalue = Math.Max(Math.Min(Speed, 15), 0);

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

      dirvalue <<= 4;

      ControlByte = (byte)(dirvalue | speedvalue);
    }

  }
}
