using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Interactivity;

namespace Assay.Package
{
    public class ListViewClickAction : TargetedTriggerAction<ListView>
    {
        protected override void Invoke(object parameter)
        {
            try
            {
                var selectedItem = (this.TargetObject as ListView).SelectedItem;

                if (selectedItem is FileModel)
                {
                    Process.Start((selectedItem as FileModel).FilePath);
                }
                else if (selectedItem is DirectoryModel)
                {
                    var viewModel = (this.TargetObject as ListView).DataContext as ViewModel;
                    if (viewModel != null)
                    {
                        viewModel.SelectedItem = selectedItem as IFile;
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
