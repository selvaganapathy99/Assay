using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;

namespace Assay.Package
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            bool isStartMainWindow = true ;
            if (e.Args.Count() > 0)
            {
                ZipPath = e.Args[0].ToString();
               var extension =  System.IO.Path.GetExtension(ZipPath);
               if (extension != ".assay-protocol" )
               {
                   if (extension != ".zip")
                   {
                       isStartMainWindow = false;
                   }
              
               }           

            }
            if(isStartMainWindow )
              {
                  MainWindow mainWindow = new MainWindow();
                  mainWindow.Show();
           
               }
               else
               {
                   Progress ziping = new Progress();
                   ziping.Show();
               }
          base.OnStartup(e);

          
 
        }


        public static string ZipPath
        {
            get;
            set;
        }
    }
}
