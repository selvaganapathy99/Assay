using Syncfusion.Windows.Shared;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Interactivity;

namespace Assay.Package
{
    public class ListViewRenameAction : TargetedTriggerAction<ListView>
    {
        protected override void Invoke(object parameter)
        {
            try
            {
                var listView = this.TargetObject as ListView;
                var viewModel = listView.DataContext as ViewModel;
                var textBox = VisualUtils.FindDescendant(listView.ItemContainerGenerator.ContainerFromItem(listView.SelectedItem) as ListViewItem, typeof(TextBox)) as TextBox;
                if (Keyboard.IsKeyDown(Key.F2))
                {
                    if (viewModel != null)
                    {
                        viewModel.ContentSelectedItem.IsEditing = true;
                        textBox.Focus();
                        if(viewModel.ContentSelectedItem is DirectoryModel)
                            textBox.SelectionLength = textBox.Text.Count();
                        else if(viewModel.ContentSelectedItem is FileModel)
                        {
                            var vm = viewModel.ContentSelectedItem as FileModel;
                            if(vm.Name.Split('.').Count()>0)
                            {
                                textBox.SelectionLength= vm.Name.Split('.')[0].Count();
                            }
                        }
                    }
                }
                else if (Keyboard.IsKeyDown(Key.Enter) || Keyboard.IsKeyDown(Key.Escape))
                {
                    if (viewModel != null)
                    {
                        if (Keyboard.IsKeyDown(Key.Enter))
                        {
                            if (viewModel.ContentSelectedItem is DirectoryModel)
                            {
                                var dirmodel = viewModel.ContentSelectedItem as DirectoryModel;
                                var dirPath = dirmodel.DirectoryPath;
                                var location = dirPath.Substring(0, dirPath.Count() - dirmodel.Name.Count());
                                var targetName = textBox.Text;
                                var targetPath = location + targetName;
                                if (System.IO.Directory.Exists(targetPath))
                                {
                                    targetName = viewModel.GetNewItemName(location, targetName, 1);
                                    targetPath = location + targetName;
                                }
                                System.IO.Directory.Move(dirPath, targetPath);
                                dirmodel.Name = targetName;
                                dirmodel.DirectoryPath = targetPath;
                                dirmodel.LastModifiedDate = DateTime.Now;
                            }
                            if (viewModel.ContentSelectedItem is FileModel)
                            {
                                var dirmodel = viewModel.ContentSelectedItem as FileModel;
                                var dirPath = dirmodel.FilePath;
                                var location = dirPath.Substring(0, dirPath.Count() - dirmodel.Name.Count());
                                var targetName = textBox.Text;
                                var targetPath = location + targetName;
                                var ext = "";
                                var name = "";
                                if (targetName.Split('.').Count() > 0)
                                {
                                    var res = targetName.Split('.');
                                    name = res[0];
                                    ext = "." + res[1];
                                }
                                if (System.IO.File.Exists(targetPath))
                                {
                                    targetName = viewModel.GetNewItemName(location, name, ext, 1);
                                    targetPath = location + targetName;
                                }
                                System.IO.File.Move(dirPath, targetPath);
                                dirmodel.Name = targetName;
                                dirmodel.FilePath = targetPath;
                                dirmodel.LastModifiedDate = DateTime.Now;
                            }
                            ViewModel.isNeedToSave = true;
                        }
                        viewModel.ContentSelectedItem.IsEditing = false;
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
