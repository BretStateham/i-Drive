using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace iDrive.Model
{
  public class RacerCommandProvider : IRacerCommandProvider
  {


    protected IRacer racer;

    public virtual IRacer Racer
    {
      get { return racer; }
      set { racer = value; }
    }
    

    public event EventHandler<RacerLeftRightDirectionChangedEventArgs> RacerLeftRightDirectionChanged;

    public event EventHandler<RacerForwardBackwardDirectionChangedEventArgs> RacerForwardBackwardDirectionChanged;

    public event EventHandler<RacerSpeedChangedEventArgs> RacerSpeedChanged;

    protected virtual void OnLeftRightDirectionChanged(RacerLeftRightDirection Direction)
    {
      if (RacerLeftRightDirectionChanged != null)
        RacerLeftRightDirectionChanged(this, new RacerLeftRightDirectionChangedEventArgs(Direction));
    }

    protected virtual void OnForwardBackwardDirectionChanged(RacerForwardBackwardDirection Direction)
    {
      if (RacerForwardBackwardDirectionChanged != null)
        RacerForwardBackwardDirectionChanged(this, new RacerForwardBackwardDirectionChangedEventArgs(Direction));
    }

    protected virtual void OnSpeedChanged(int Speed)
    {
      if (RacerSpeedChanged != null)
        RacerSpeedChanged(this, new RacerSpeedChangedEventArgs(Speed));
    }

    #region IDisposable Implementation

    public void Dispose()
    {
      Dispose(true);
      GC.SuppressFinalize(this);
    }

    ~RacerCommandProvider()
    {
      Dispose(false);
    }

    protected virtual void Dispose(bool disposing)
    {
      if(disposing)
      {

      }
    }

    #endregion IDisposable Implementation

  }
}
