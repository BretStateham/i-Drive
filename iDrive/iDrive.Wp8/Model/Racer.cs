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

  public class Racer : RacerBase
  {

    override public async Task GoAsync()
    {
      if (IsConnected)
      {
        //Send all control commands on the UI Thread.  
        //This helps prevent Invalid Cross-Thread Access exceptions
        //When binding the racer to the UI.
        Deployment.Current.Dispatcher.BeginInvoke(async () =>
          {
            if (dataWriter != null)
            {
              dataWriter.WriteByte(ControlByte);
              await dataWriter.StoreAsync();
            }
          });
      }
    }

    override public async Task StopAsync()
    {
      if (IsConnected)
      {
        LeftRightDirection = RacerLeftRightDirection.None;
        ForwardBackwardDirection = RacerForwardBackwardDirection.None;
        Speed = 0;
        await GoAsync();
      }
    }


    #region Connection Management

    StreamSocket streamSocket = null;
    DataWriter dataWriter = null;

    override public bool IsConnected
    {
      get { return streamSocket != null && dataWriter != null; }
    }

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

    override public async Task ConnectAsync(DeviceInfo RacerDevice)
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

    override public async Task DisconnectAsync()
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

    override protected void RaiseRacerConnectionStateChanged()
    {
      base.RaiseRacerConnectionStateChanged();

      //Also send a INotifyPropertyChanged event notification for the IsConnected property....
      RaisePropertyChanged("IsConnected");
    }


    #endregion Connection Management

    #region IDisposable Implementation

    override protected void Dispose(bool disposing)
    {
      if (disposing)
      {
        CloseConnection();
      }

      base.Dispose(disposing);

    }

    #endregion IDisposable Implementation
  }
}
