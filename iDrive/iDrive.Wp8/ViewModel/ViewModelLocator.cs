using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Ioc;
using iDrive.Service;
using iDrive.Wp8.Service;
using Microsoft.Practices.ServiceLocation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace iDrive.Wp8.ViewModel
{
  public class ViewModelLocator
  {
    static ViewModelLocator()
    {
      //Using Laurent's SimpleIoc as the IoC container
      //Leveraging the 

      //ServiceLocator is from the Patterns and Practices team
      //It provides a way to set the IoC container (SetLocatorProvider)
      //And to return an IServiceLocator (ServiceLocator.Current)
      ServiceLocator.SetLocatorProvider(() => SimpleIoc.Default);

      //Register the Data Services
      if (ViewModelBase.IsInDesignModeStatic)
      {
        SimpleIoc.Default.Register<IDataService, Design.DesignDataService>();
      }
      else
      {
        SimpleIoc.Default.Register<IDataService, DataService>();
      }

      //Register the Racer



      SimpleIoc.Default.Register<MainViewModel>();
    }

    public MainViewModel MainViewModel
    {
      get
      {
        return ServiceLocator.Current.GetInstance<MainViewModel>();
      }
    }
  }
}
