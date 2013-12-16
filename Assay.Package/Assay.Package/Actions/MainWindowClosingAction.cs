using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Interactivity;

namespace Assay.Package
{
    public class MainWindowClosingAction : TargetedTriggerAction<MainWindow>
    {
        protected override void Invoke(object parameter)
        {
            try
            {
                if (ViewModel.isNeedToSave && ((parameter is CancelEventArgs && !((this.TargetObject as MainWindow).DataContext as ViewModel).m_lastArgsTypeIsRouted) || (parameter is RoutedEventArgs)))
                {
                    MessageBoxResult windowClosingResult = MessageBox.Show(this.TargetObject as MainWindow, "Want to save your changes?" + Environment.NewLine + "If you click 'No', a recent copy of this explorer will be temporarily available.", "Assay.Package", MessageBoxButton.YesNoCancel);
                    if (windowClosingResult == MessageBoxResult.Yes)
                    {
                        ((this.TargetObject as MainWindow).DataContext as ViewModel).Save(App.ZipPath);
                    }
                    else if (windowClosingResult == MessageBoxResult.No)
                    {
                        if (parameter is RoutedEventArgs)
                        {
                            ((this.TargetObject as MainWindow).DataContext as ViewModel).m_lastArgsTypeIsRouted = true;
                            (this.TargetObject as MainWindow).Close();
                        }
                    }
                    else
                    {
                        if (parameter is CancelEventArgs)
                        {
                            (parameter as CancelEventArgs).Cancel = true;
                        }
                        else if (parameter is RoutedEventArgs)
                        {
                            (parameter as RoutedEventArgs).Handled = true;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }
    }
}
