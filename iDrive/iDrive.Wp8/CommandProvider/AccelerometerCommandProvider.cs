using iDrive.Model;
using Microsoft.Phone.Controls;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Devices.Sensors;

namespace iDrive.CommandProvider
{
  public class AccelerometerCommandProvider : RacerCommandProvider
  {

    Accelerometer accelerometer;

    RacerForwardBackwardDirection forwardBackwardDirection = RacerForwardBackwardDirection.None;
    RacerLeftRightDirection leftRightDirection = RacerLeftRightDirection.None;
    int speed = 0;

    public AccelerometerCommandProvider()
    {
      InitAccelerometer();
    }

    private void InitAccelerometer()
    {
      accelerometer = Accelerometer.GetDefault();
      accelerometer.ReportInterval = 50; //milliseconds: 50 = 20 times per second, 100 = 10 times per second.
      accelerometer.ReadingChanged += accelerometer_ReadingChanged;
    }

    protected override void Dispose(bool disposing)
    {
      if(disposing)
      {
        StopListening();
      }
      base.Dispose(disposing);
    }

    public void StopListening()
    {
      if(accelerometer != null)
        accelerometer.ReadingChanged -= accelerometer_ReadingChanged;
    }

    public PageOrientation Orientation { get; set; }

    private bool IsPortrait()
    {
      return (Orientation == PageOrientation.None || Orientation == PageOrientation.Portrait || Orientation == PageOrientation.PortraitDown || Orientation == PageOrientation.PortraitUp);
    }
    
    void accelerometer_ReadingChanged(Accelerometer sender, AccelerometerReadingChangedEventArgs args)
    {

      double accelLR;
      double accelFB;
      
      if(IsPortrait())
      {
        accelLR = args.Reading.AccelerationX;
        accelFB = args.Reading.AccelerationY;
      }
      else
      {
        accelLR = args.Reading.AccelerationY;
        accelFB = args.Reading.AccelerationX;
      }

      double polarityLR = (Orientation == PageOrientation.None || Orientation == PageOrientation.PortraitUp || Orientation == PageOrientation.LandscapeRight) ? +1 : -1;

      double polarityFB = (Orientation == PageOrientation.None || Orientation == PageOrientation.PortraitUp || Orientation == PageOrientation.LandscapeLeft) ? +1 : -1;

      //Need to figure this out better. 
      var lr = (int)Clamp((int)(accelLR * polarityLR * 10),-10,10);
      var fb = (int)Clamp((int)(accelFB * polarityFB * 10),-10,10);

      var lrval = Math.Round(Map(lr, -3, 3, -1, 1));
      var fbval = Math.Round(Map(fb, -3, 3, -1, 1));

      var lrdir = (RacerLeftRightDirection)lrval;
      var fbdir = (RacerForwardBackwardDirection)fbval;
      
      var speedval = (int)Math.Round(Map(Math.Abs(fb), 0, 6, 0, 15));

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
        forwardBackwardDirection = fbdir;
      }
      if (speed != speedval)
      {
        OnSpeedChanged(speedval);
        speed = speedval;
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

    protected override void OnLeftRightDirectionChanged(RacerLeftRightDirection Direction)
    {
      if (Racer != null && Racer.IsConnected)
      {
        Racer.LeftRightDirection = Direction;
        Racer.GoAsync();
      }

      base.OnLeftRightDirectionChanged(Direction);
    }

    protected override void OnForwardBackwardDirectionChanged(RacerForwardBackwardDirection Direction)
    {
      if (Racer != null && Racer.IsConnected)
      {
        Racer.ForwardBackwardDirection = Direction;
        Racer.GoAsync();
      }

      base.OnForwardBackwardDirectionChanged(Direction);
    }

    protected override void OnSpeedChanged(int Speed)
    {
      if (Racer != null && Racer.IsConnected)
      {
        Racer.Speed = Speed;
        Racer.GoAsync();
      }

      base.OnSpeedChanged(Speed);
    }

  }
}
