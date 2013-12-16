using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Interactivity;

namespace Assay.Package
{
    public class ListViewItemDragAndDropAction : TargetedTriggerAction<ListView>
    {
        DirectoryModel index =new DirectoryModel();

        int listItemCount = 0;
        protected override void Invoke(object parameter)
        {
            try
            {
                ListView localListView = (this.TargetObject as ListView);

                var treeView = ((App.Current.MainWindow as MainWindow).leftSidePane as LeftSidePane).zipTreeView;
                var curindex = treeView.SelectedItem as DirectoryModel;


                //if (index != curindex || listItemCount != localListView.Items.Count)
                {
                    index = curindex;
                    listItemCount = localListView.Items.Count;
                    foreach (var item in localListView.Items)
                    {
                        ListViewItem lvitem = localListView.ItemContainerGenerator.ContainerFromItem(item) as ListViewItem;

                        if (lvitem != null)
                        {
                            lvitem.PreviewMouseLeftButtonDown -= lvitem_MouseLeftButtonDown;
                            lvitem.PreviewMouseLeftButtonUp -= lvitem_MouseLeftButtonUp;

                            lvitem.PreviewMouseLeftButtonDown += lvitem_MouseLeftButtonDown;
                            lvitem.PreviewMouseLeftButtonUp += lvitem_MouseLeftButtonUp;
                        }
                    }

                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        string targetPath = "";
        string sourcePath = "";   

        void lvitem_MouseLeftButtonUp(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            try
            {
                ListViewItem droppedItem = sender as ListViewItem;
                var viewModel = (TargetObject as ListView).DataContext as ViewModel;

                if (droppedItem.DataContext is DirectoryModel)
                {
                    targetPath = (droppedItem.DataContext as DirectoryModel).DirectoryPath;

                    var getSourceItem = System.IO.Path.GetExtension(sourcePath);
                    var getTargetItem = System.IO.Path.GetExtension(targetPath);
                    var getSourceName = System.IO.Path.GetFileName(sourcePath);
                    if (sourcePath != targetPath)
                    {
                        if (getSourceItem == string.Empty && getTargetItem == string.Empty)
                        {
                            if (!System.IO.Directory.Exists(targetPath + "\\" + getSourceName))
                            {
                                System.IO.Directory.Move(sourcePath, targetPath + "\\" + getSourceName);
                            }

                        }
                        else
                        {

                            string fileName = System.IO.Path.GetFileName(sourcePath);
                            if (!System.IO.File.Exists(targetPath + "\\" + fileName))
                            {
                                System.IO.File.Move(sourcePath, targetPath + "\\" + fileName);
                            }

                        }

                        var path = viewModel.SelectedItem as DirectoryModel;
                        path.Items.Clear();                       

                        viewModel.IterateFolders(path.DirectoryPath ?? viewModel.extractedFolder, path.Items);
                        ViewModel.isNeedToSave = true;
                    }
                }
                (this.TargetObject as ListView).Cursor = Cursors.Arrow;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            finally
            {
                (this.TargetObject as ListView).Cursor = Cursors.Arrow;
            }
        }

        void lvitem_MouseLeftButtonDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            try
            {
                ListViewItem localItem = sender as ListViewItem;

                if (localItem.DataContext is FileModel)
                {
                    sourcePath = (localItem.DataContext as FileModel).FilePath;
                }
                else if (localItem.DataContext is DirectoryModel)
                {
                    sourcePath = (localItem.DataContext as DirectoryModel).DirectoryPath;
                }

                (this.TargetObject as ListView).Cursor = Cursors.SizeAll;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);  
            }
        }
    }
}
