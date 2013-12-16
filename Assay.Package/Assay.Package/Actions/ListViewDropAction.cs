using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Interactivity;

namespace Assay.Package
{
    public class ListViewDropAction :TargetedTriggerAction<ListView> 
    {
        protected override void Invoke(object parameter)
        {
            try
            {
                var viewModel = (this.TargetObject as ListView).DataContext as ViewModel;

                if (viewModel != null && viewModel.SelectedItem != null)
                {
                    var data = (string[])(parameter as DragEventArgs).Data.GetData(DataFormats.FileDrop, false);

                    string sourcePath = data[0].ToString();
                    string targetPath = (viewModel.SelectedItem as DirectoryModel).DirectoryPath;

                    string fileName = System.IO.Path.GetFileName(sourcePath);
                    if (fileName.Contains(".txt") || fileName.Contains(".png") || fileName.Contains(".xml"))
                    {
                        System.IO.File.Move(sourcePath, targetPath + "\\" + fileName);
                    }
                    else
                    {
                        System.IO.Directory.Move(sourcePath, targetPath + "\\" + fileName);
                    }                   

                    var items=(viewModel.SelectedItem as DirectoryModel).Items;

                    viewModel.IterateFolders(targetPath, items);

                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }
    }
}
