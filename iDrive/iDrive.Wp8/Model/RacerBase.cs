using GalaSoft.MvvmLight;
using iDrive.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace iDrive.Model
{
  public abstract class RacerBase : ObservableObject, IRacer
  {
    public event EventHandler<RacerConnectionStateChangedEventArgs> RacerConnectionStateChanged;

    //private bool isConnected = false;
    //public virtual bool IsConnected
    //{
    //  get { return isConnected; }
    //}

    public abstract bool IsConnected { get; }

    protected int speed;

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

    protected virtual void SetControlByte()
    {

      int dirvalue = 0;
      string direction = string.Format("{0}{1}", LeftRightDirection.ToString()[0], forwardBackwardDirection.ToString()[0]).ToUpper();
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

    public abstract Task GoAsync();

    public abstract Task StopAsync();

    public abstract Task ConnectAsync(DeviceInfo RacerDevice);

    public abstract Task DisconnectAsync();

    protected virtual void RaiseRacerConnectionStateChanged()
    {
      //Raise the RacerConnectionStateChangedEvent
      if (RacerConnectionStateChanged != null)
        RacerConnectionStateChanged(this, new RacerConnectionStateChangedEventArgs(IsConnected));
    }

    #region IDisposable Implementation

    ~RacerBase()
    {
      Dispose(false);
    }

    public void Dispose()
    {
      Dispose(true);
      GC.SuppressFinalize(this);
    }

    protected virtual void Dispose(bool disposing)
    {
      if (disposing)
      {
        //Clean up managed resources here
      }

      //Clean up unmanaged resources here
    }

    #endregion IDisposable Implementation
  }
}
