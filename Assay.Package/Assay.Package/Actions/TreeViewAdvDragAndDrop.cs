using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Interactivity;
using Syncfusion.Windows.Tools.Controls;

namespace Assay.Package
{
    public class TreeViewAdvDragAndDrop : TargetedTriggerAction<Syncfusion.Windows.Tools.Controls.TreeViewAdv>
    {
        protected override void Invoke(object parameter)
        {
            try
            {
                var viewModel = (this.TargetObject as TreeViewAdv).DataContext as ViewModel;
                var dataContext = ((parameter as System.Windows.DragEventArgs).OriginalSource as System.Windows.FrameworkElement).DataContext;

                string sourcePath = string.Empty;
                string targetPath = string.Empty;

                if (viewModel != null)
                {
                    targetPath = (dataContext as DirectoryModel).DirectoryPath;

                    if (viewModel.SelectedItem is FileModel)
                    {
                        sourcePath = (viewModel.SelectedItem as FileModel).FilePath;
                        string fileName = System.IO.Path.GetFileName(sourcePath);

                        System.IO.File.Move(sourcePath, targetPath + "\\" + fileName);

                    }
                    else if (viewModel.SelectedItem is DirectoryModel)
                    {
                        sourcePath = (viewModel.SelectedItem as DirectoryModel).DirectoryPath;
                        string fileName = System.IO.Path.GetFileName(sourcePath);

                        System.IO.Directory.Move(sourcePath, targetPath + "\\" + fileName);

                    }

                    var path = viewModel.SelectedItem as DirectoryModel;
                    path.Items.Clear();

                    viewModel.IterateFolders(path.DirectoryPath, path.Items);
                    ViewModel.isNeedToSave = true;
                    //viewModel.ExtractZipFiles(false);
                }

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);   
            }
        }
    }
}
