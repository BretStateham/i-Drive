using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace iDrive.Model
{
  public interface IRacerCommandProvider: IDisposable
  {

    IRacer Racer { get; set; }

    event EventHandler<RacerLeftRightDirectionChangedEventArgs> RacerLeftRightDirectionChanged;
    event EventHandler<RacerForwardBackwardDirectionChangedEventArgs> RacerForwardBackwardDirectionChanged;
    event EventHandler<RacerSpeedChangedEventArgs> RacerSpeedChanged;

  }
}
