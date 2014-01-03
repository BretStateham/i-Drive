using GalaSoft.MvvmLight;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace iDrive.Model
{
  class RacerBase : ObservableObject, IRacer
  {
    public bool IsConnected
    {
      get { throw new NotImplementedException(); }
    }

    public int Speed
    {
      get
      {
        throw new NotImplementedException();
      }
      set
      {
        throw new NotImplementedException();
      }
    }

    public RacerForwardBackwardDirection ForwardBackwardDirection
    {
      get
      {
        throw new NotImplementedException();
      }
      set
      {
        throw new NotImplementedException();
      }
    }

    public RacerLeftRightDirection LeftRightDirection
    {
      get
      {
        throw new NotImplementedException();
      }
      set
      {
        throw new NotImplementedException();
      }
    }

    public byte ControlByte
    {
      get
      {
        throw new NotImplementedException();
      }
      set
      {
        throw new NotImplementedException();
      }
    }

    public Task GoAsync()
    {
      throw new NotImplementedException();
    }

    public IRacerCommandProvider CommandProvider
    {
      get
      {
        throw new NotImplementedException();
      }
      set
      {
        throw new NotImplementedException();
      }
    }
  }
}
