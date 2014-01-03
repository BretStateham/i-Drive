using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Devices.Sensors;

namespace iDrive.Model
{
  public class AccelerometerCommandProvider : RacerCommandProvider
  {

    Accelerometer accelerometer;

    RacerForwardBackwardDirection forwardBackwardDirection = RacerForwardBackwardDirection.None;
    RacerLeftRightDirection leftRightDirection = RacerLeftRightDirection.None;
    int speed = 0;

    public AccelerometerCommandProvider()
    {
      accelerometer = Accelerometer.GetDefault();
      accelerometer.ReportInterval = 20;
      accelerometer.ReadingChanged += accelerometer_ReadingChanged;
    }

    void accelerometer_ReadingChanged(Accelerometer sender, AccelerometerReadingChangedEventArgs args)
    {

      var lr = (int)Clamp((int)(args.Reading.AccelerationX * 10),-10,10);
      var fb = (int)Clamp((int)(args.Reading.AccelerationY * 10),-10,10);

      var lrval = Math.Round(Map(lr, -3, 3, -1, 1));
      var fbval = Math.Round(Map(fb, -3, 3, -1, 1));

      var lrdir = (RacerLeftRightDirection)lrval;
      var fbdir = (RacerForwardBackwardDirection)fbval;
      var speedval = (int)Math.Round(Map(Math.Abs(fb), 0, 7.5, 0, 15));


      //Debug.WriteLine("(x,y,z)=({0:N4},{1:N4},{2:N4})", args.Reading.AccelerationX, args.Reading.AccelerationY, args.Reading.AccelerationZ);
      //Debug.WriteLine("(lr,fb)=({0},{1})", lr, fb);
      Debug.WriteLine("(lr,fb) = ({0},{1}) - (lrval,fbvbal,speedval)=({2},{3},{4}) - (lrdir,fbdir,speedval)=({5},{6},{4})", lr, fb, lrval, fbval, speedval, lrdir, fbdir);

      if (leftRightDirection != lrdir)
      {
        OnLeftRightDirectionChanged(lrdir);
        leftRightDirection = lrdir;
      }
      if (forwardBackwardDirection != fbdir)
      {
        OnForwardBackwardDirectionChanged(fbdir);
        //forwardBackwardDirection = fbdir;
      }
      if (speed != speedval)
      {
        OnSpeedChanged(speedval);
        //speed = speedval;
      }
    }    

    private double Clamp(double Value, double Minimum, double Maximum)
    {
      return Math.Max(Math.Min(Value, Maximum), Minimum);
    }

    private double Map(double Value, double ValueMin, double ValueMax, double TargetMin, double TargetMax)
    {
      double range = (ValueMax - ValueMin);
      double magnitude = (Value - ValueMin);
      double pct = magnitude / range;

      double targetRange = TargetMax - TargetMin;
      double targetMagnitude = pct * targetRange;
      double targetValue = TargetMin + targetMagnitude;

      return Clamp(targetValue,TargetMin,TargetMax);

      //return (((Value - ValueMin) / (ValueMax - ValueMin)) * (TargetMax - TargetMin)) + TargetMin;
    }

  }
}
