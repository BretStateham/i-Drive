﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace iDrive.Model
{
  public interface IRacerCommandProvider
  {

    event EventHandler<RacerLeftRightDirectionChangedEventArgs> RacerLeftRightDirectionChanged;
    event EventHandler<RacerForwardBackwardDirectionChangedEventArgs> RacerForwardBackwardDirectionChanged;
    event EventHandler<RacerSpeedChangedEventArgs> RacerSpeedChanged;

  }
}
