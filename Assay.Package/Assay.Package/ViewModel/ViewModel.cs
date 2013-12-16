using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Drawing;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Interop;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using Microsoft.Win32;
using Syncfusion.Windows.Shared;

namespace Assay.Package
{
    public class ViewModel : NotificationObject
    {
        #region Resources

        BitmapImage image = new BitmapImage(new Uri("/Assay.Package;component/Assets/folder.png", UriKind.RelativeOrAbsolute));

        #endregion

        #region Ctor

        public ViewModel()
        {
            ExtractZipFiles(true);
            CreateCommands();

            CreatePane();
            CreateLayout();
            DirectoryList.CollectionChanged -= DirectoryList_CollectionChanged;
            DirectoryList.CollectionChanged += DirectoryList_CollectionChanged;
            this.SelectedItem = DirectoryList[0];
        }

        void DirectoryList_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            isNeedToSave = true;
        }

        #endregion       

        #region
        internal bool m_lastArgsTypeIsRouted;
        #endregion

        #region Properties

        private IFile _selectedItem;

        public IFile SelectedItem
        {
            get
            {
                return _selectedItem;
            }
            set
            {
                _selectedItem = value;
                RightSideContent = value;
                this.RaisePropertyChanged(() => this.SelectedItem);
            }
        }

        private ObservableCollection<IFile> _directoryList;

        public ObservableCollection<IFile> DirectoryList
        {
            get
            {
                return _directoryList;
            }
            set
            {
                _directoryList = value;
                this.RaisePropertyChanged(() => this.DirectoryList);
            }
        }

        private IFile rightSideContent;

        public IFile RightSideContent
        {
            get
            {
                return rightSideContent;
            }
            set
            {
                rightSideContent = value;
                this.RaisePropertyChanged(() => this.RightSideContent);
            }
        }
        

        private bool isEditing; 

        public bool IsEditing  
        {
            get
            {
                return isEditing;
            }
            set
            {
                isEditing = value;
                this.RaisePropertyChanged(() => this.IsEditing);
            }
        }

        private IFile contentSelectedItem;

        public IFile ContentSelectedItem  
        {
            get
            {
                return contentSelectedItem;
            }
            set
            {
                if(contentSelectedItem !=null)
                {
                    contentSelectedItem.IsEditing = false;
                }
                contentSelectedItem = value;
                if (value != null)
                    RightSideContent = value;
                this.RaisePropertyChanged(() => this.ContentSelectedItem);
            }
        }
        
        

        private ICommand selectedItemChangedCommand;

        public ICommand SelectedItemChangedCommand
        {
            get
            {
                return selectedItemChangedCommand;
            }
            set
            {
                selectedItemChangedCommand = value;
            }
        }

        private ICommand newItemCommand;

        public ICommand NewItemCommand
        {
            get
            {
                return newItemCommand;
            }
            set
            {
                newItemCommand = value;
            }
        }

        private ICommand saveCommand;

        public ICommand SaveCommand  
        {
            get
            {
                return saveCommand;
            }
            set
            {
                saveCommand = value;
            }
        }

        private ICommand saveAsCommand;

        public ICommand SaveAsCommand
        {
            get
            {
                return saveAsCommand;
            }
            set
            {
                saveAsCommand = value;
            }
        }

        private ICommand deleteCommand;

        public ICommand DeleteCommand
        {
            get
            {
                return deleteCommand;
            }
            set
            {
                deleteCommand = value;
            }
        }

        private ICommand copyPathCommand;

        public ICommand CopyPathCommand
        {
            get
            {
                return copyPathCommand;
            }
            set
            {
                copyPathCommand = value;
            }
        }

        private ICommand openCommand;

        public ICommand OpenCommand
        {
            get
            {
                return openCommand;
            }
            set
            {
                openCommand = value;
            }
        }

        private ICommand cutCommand;

        public ICommand CutCommand
        {
            get
            {
                return cutCommand;
            }
            set
            {
                cutCommand = value;
            }
        }

        private ICommand copyCommand;

        public ICommand CopyCommand
        {
            get
            {
                return copyCommand;
            }
            set
            {
                copyCommand = value;
            }
        }

        private ICommand pasteCommand;

        public ICommand PasteCommand
        {
            get
            {
                return pasteCommand;
            }
            set
            {
                pasteCommand = value;
            }
        }       
        

        public string FileName
        {
            get
            {
                return System.IO.Path.GetFileName(App.ZipPath);
            }
        }

        public string ToolTipName
        {
            get
            {
                return App.ZipPath;
            }
        }

        internal static bool isNeedToSave = false;

        #endregion            

        #region Methods
        internal string extractedFolder = string.Empty;
        internal void ExtractZipFiles(bool needRefresh)
        {
            try
            {
                string sourcePath = App.ZipPath;
                string extractPath = System.IO.Path.GetTempPath();
                  string folderName = System.IO.Path.GetFileName(sourcePath).Replace(Path.GetExtension (sourcePath ), "");

                if (needRefresh)
                {
                     extractedFolder = extractPath  + folderName ;
                     if (Directory.Exists(extractedFolder))
                    {
                        Directory.Delete(extractedFolder, true);
                    }

                     Directory.CreateDirectory(extractedFolder);
                     ZipFile.ExtractToDirectory(sourcePath, extractedFolder);
                }

              
                CreateDirectoryList(extractPath + folderName);
            }
            catch (Exception e)
            {
                
                isNeedToSave = false;
                if (e.Message == "Object reference not set to an instance of an object.")
                {
                    MessageBox.Show("Unable to open the Assay package");
                }
                Console.WriteLine(e.Message);
            }
        }

        private void CreateDirectoryList(string path)
        {
            DirectoryList = new ObservableCollection<IFile>();

            var dirList = new DirectoryModel
            {
                Name = this.FileName,
                Icon = image,
                IsSelected = true,
                DirectoryPath=path
            };

            DirectoryList.Add(dirList);           
            IterateFolders(path, dirList.Items);
        }

        internal  ObservableCollection<IFile> IterateFolders(string sDir, ObservableCollection<IFile> list)
        {
            try
            {
                DirectoryInfo dir = new DirectoryInfo(sDir);                
                foreach (var internalDir in dir.GetDirectories())
                {
                    var dirList = new DirectoryModel
                    {
                        Name = internalDir.Name,
                        CreatedDate = internalDir.CreationTime,
                        LastModifiedDate = internalDir.LastWriteTime,
                        Type = "File Folder",
                        Icon = image,
                        DirectoryPath = internalDir.FullName,                        
                        IsFile = false,
                    };

                    list.Add(dirList);
                    IterateFolders(internalDir.FullName, dirList.Items);
                }

                foreach (var file in dir.GetFiles())
                {
                    var fileList = new FileModel
                    {
                        Name = file.Name,
                        Type = file.Extension,
                        CreatedDate = file.CreationTime,
                        LastModifiedDate = file.LastWriteTime,
                        Size = file.Length / 1024,
                        Icon = ExtractIcon(file),
                        FilePath = file.FullName,
                        IsFile = true
                    };
                    var isImage = isImageFile(file.FullName);
                    fileList.IsImageVisible = (isImage == "img") ? true : false;
                    fileList.IsTextVisible = !fileList.IsImageVisible;
                    fileList.TextContent = fileList.IsImageVisible ? file.FullName : (isImage == "txt") ?getTextContent(file.FullName) : "Select a file to preview";
                    list.Add(fileList);
                }
            }
            catch (System.Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return list;
        }

        private string isImageFile(string filePath)
        {
            var ext = Path.GetExtension(filePath);
            if (ext == ".png" || ext == ".jpg" || ext == ".jpeg" || ext == ".bmp" || ext == ".ico")
                return "img";
            if (ext == ".txt" || ext == ".xml" || ext == ".htm" || ext == ".html" || ext == ".xaml" || ext == ".cs")
                return "txt";
            return "other";

        }

        private string getTextContent(string filePath)
        {
    

            string contentText = string.Empty;
            using (StreamReader sr = File.OpenText(filePath))
            {
                contentText = sr.ReadToEnd();
            }
            return contentText;

        }

        private ImageSource ExtractIcon(FileInfo directory)
        {
            System.Drawing.Icon ico = System.Drawing.Icon.ExtractAssociatedIcon(directory.FullName);
            Bitmap bmp = ico.ToBitmap();
            MemoryStream strm = new MemoryStream();
            bmp.Save(strm, System.Drawing.Imaging.ImageFormat.Png);
            BitmapImage bmpImage = new BitmapImage();

            bmpImage.BeginInit();
            strm.Seek(0, SeekOrigin.Begin);
            bmpImage.StreamSource = strm;
            bmpImage.EndInit();
            ImageSource source = bmpImage;
            return source;
        }

        #endregion

        #region Commands

        private void CreateCommands()
        {
            selectedItemChangedCommand = new DelegateCommand<object>(SelectedItemChanged);
            newItemCommand = new DelegateCommand<object>(CreateNewItem);
            saveCommand = new DelegateCommand<object>(Save);
            saveAsCommand = new DelegateCommand<object>(SaveAs);
            deleteCommand = new DelegateCommand<object>(Delete, CanCommandExecute);
            copyPathCommand = new DelegateCommand<object>(CopyPath, CanCommandExecute);
            cutCommand = new DelegateCommand<object>(Cut, CanCommandExecute);
            copyCommand = new DelegateCommand<object>(Copy, CanCommandExecute);
            pasteCommand = new DelegateCommand<object>(Paste, CanCommandExecute);
            openCommand =   new DelegateCommand<object>(Open, CanCommandExecute);
        }

        private void SelectedItemChanged(object param)
        {
            try
            {
                if (param != null)
                {
                    IFile file = param as IFile;
                    if (file != null)
                    {
                        this.SelectedItem = file;
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        private void CreateNewItem(object param)
        {
            try
            {
                if (param != null)
                {
                    string location = string.Empty;

                    if (this.SelectedItem is DirectoryModel)
                        location = (this.SelectedItem as DirectoryModel).DirectoryPath ?? extractedFolder;

                    if (this.SelectedItem is FileModel)
                        location = (this.SelectedItem as FileModel).FilePath ?? extractedFolder;

                    if (param.ToString() == "txt")
                    {
                        var txtName = "\\New Text Document";
                        var ext = ".txt";
                        if (File.Exists(location + txtName + ext))
                        {
                            txtName = GetNewItemName(location, txtName, ext, 1);
                        }
                        else
                        {
                            txtName = txtName + ext;
                        }
                        File.Create(location + txtName);

                        FileInfo file = new FileInfo(location + txtName);

                        var fileList = new FileModel
                        {
                            Name = file.Name,
                            Type = file.Extension,
                            CreatedDate = file.CreationTime,
                            LastModifiedDate = file.LastWriteTime,
                            Size = file.Length / 1024,
                            FilePath = file.FullName,
                            Icon = ExtractIcon(file),
                            IsFile = true
                        };

                        (this.SelectedItem as DirectoryModel).Items.Add(fileList);
                    }
                    else if (param.ToString() == "xml")
                    {
                        var XmlName = "\\New Text Document";
                        var ext = ".xml";
                        if (File.Exists(location + XmlName + ext))
                        {
                            XmlName = GetNewItemName(location, XmlName, ext, 1);
                        }
                        else
                        {
                            XmlName = XmlName + ext;
                        }
                        File.Create(location + XmlName);

                        FileInfo file = new FileInfo(location + XmlName);

                        var fileList = new FileModel
                        {
                            Name = file.Name,
                            Type = file.Extension,
                            CreatedDate = file.CreationTime,
                            LastModifiedDate = file.LastWriteTime,
                            Size = file.Length / 1024,
                            FilePath = file.FullName,
                            Icon = ExtractIcon(file),
                            IsFile = true
                        };

                        (this.SelectedItem as DirectoryModel).Items.Add(fileList);
                    }
                    else
                    {
                        var DirName = "\\New Folder";
                        if (Directory.Exists(location + DirName))
                        {
                            DirName = GetNewItemName(location, DirName, 1);
                        }
                        Directory.CreateDirectory(location + DirName);

                        DirectoryInfo internalDir = new DirectoryInfo(location + DirName);

                        var dirList = new DirectoryModel
                        {
                            Name = internalDir.Name,
                            CreatedDate = internalDir.CreationTime,
                            LastModifiedDate = internalDir.LastWriteTime,
                            Type = "File Folder",
                            DirectoryPath = internalDir.FullName,
                            Icon = image
                        };
                        isNeedToSave = true;
                        (this.SelectedItem as DirectoryModel).Items.Add(dirList);

                        //WORKAROUND :TreeViewAdv Expand Button is Visible problem 
                        var selectedTreeViewItem = (App.Current.MainWindow as MainWindow).leftSidePane.zipTreeView.SelectedContainer;
                        if(selectedTreeViewItem!=null)
                        {
                        if((selectedTreeViewItem.Template.FindName("PART_Expander",selectedTreeViewItem)as Expander).Visibility == Visibility.Hidden)
                            (selectedTreeViewItem.Template.FindName("PART_Expander", selectedTreeViewItem) as Expander).Visibility = Visibility.Visible;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        internal string GetNewItemName(string location ,string name,int count)
        {
            string return_name = "";
           if(Directory.Exists(location+name+"(" + count +")"))
           {
               return_name = GetNewItemName(location, name, ++count);
           }
           else
           {
               return_name = name+"(" + count + ")";
           }
           return return_name;
        }

        internal string GetNewItemName(string location, string name, string ext, int count)
        {
            string return_name = "";
            if (File.Exists(location + name + "(" + count + ")"+ext))
            {
                return_name = GetNewItemName(location, name,ext, ++count);
            }
            else
            {
                return_name = name + "(" + count + ")"+ext;
            }
            return return_name;
        }

        internal void Save(object param)  
        {
            try
            {
                string folderPath = System.IO.Path.GetFileName(param.ToString()).Replace(".assay-protocol", "");

                string sourcePath = System.IO.Path.GetTempPath() + folderPath;
                string targetPath = param.ToString();

                if (File.Exists(targetPath))
                {
                    File.Delete(targetPath);
                }
               
                ZipFile.CreateFromDirectory(sourcePath, targetPath);
                isNeedToSave = false;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }            
        }

        private void SaveAs(object param)
        {
            try
            {
                SaveFileDialog saveDlg = new SaveFileDialog();
                if ((bool)saveDlg.ShowDialog())
                {
                    string targetPath = saveDlg.FileName;
                    string folderPath = System.IO.Path.GetFileName(param.ToString()).Replace(".assay-protocol", "");
                    string sourcePath = System.IO.Path.GetTempPath() + folderPath;                   

                    if (File.Exists(targetPath))
                    {                        
                        File.Delete(targetPath);
                    }

                    ZipFile.CreateFromDirectory(sourcePath, targetPath);
                    isNeedToSave = false;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);                
            }
        }

        private void Delete(object param)
        {
            try
            {
                if (this.contentSelectedItem != null)
                {
                    MessageBoxResult result = MessageBox.Show("Are you sure want to Delete", "Delete", MessageBoxButton.YesNo, MessageBoxImage.Warning);
                    if (result == MessageBoxResult.Yes)
                    {
                        if (this.ContentSelectedItem is FileModel)
                        {
                            string fileName = (this.ContentSelectedItem as FileModel).FilePath;
                            if (File.Exists(fileName))
                                File.Delete(fileName);

                            var localItem = (this.SelectedItem as DirectoryModel);

                        }

                        if (this.ContentSelectedItem is DirectoryModel)
                        {
                            string dirName = (this.ContentSelectedItem as DirectoryModel).DirectoryPath;
                            if (Directory.Exists(dirName))
                                Directory.Delete(dirName, true);
                        }

                        var path = SelectedItem as DirectoryModel;
                        path.Items.Clear();

                        IterateFolders(path.DirectoryPath, path.Items);
                        isNeedToSave = true;
                        if(Directory.EnumerateDirectories(path.DirectoryPath).Count()<1)
                        {
                            var selectedTreeViewItem = (App.Current.MainWindow as MainWindow).leftSidePane.zipTreeView.SelectedContainer;
                            (selectedTreeViewItem.Template.FindName("PART_Expander", selectedTreeViewItem) as Expander).Visibility = Visibility.Hidden;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        private void CopyPath(object param)
        {
            try
            {
                if (this.ContentSelectedItem != null)
                {
                    if (this.ContentSelectedItem is FileModel)
                    {
                        Clipboard.SetText((this.ContentSelectedItem as FileModel).FilePath);
                    }
                    if (this.ContentSelectedItem is DirectoryModel)
                    {
                        Clipboard.SetText((this.ContentSelectedItem as DirectoryModel).DirectoryPath);
                    }
                }
            }
            catch (Exception ex)
            {                
               Console.WriteLine(ex.Message);
            }
        }

        private bool CanCommandExecute(object param)
        {
            //return this.ContentSelectedItem != null; 

            return true;
        }

        string sourcePath = string.Empty;
        string targetPath = string.Empty;
        bool isfileCopied = false;
        object oldSelectedItem;

        private void Cut(object param)
        {
            try
            {
                if (this.ContentSelectedItem == null)
                {
                    if (this.SelectedItem is FileModel)
                    {
                        sourcePath = (this.SelectedItem as FileModel).FilePath;
                    }
                    else if (this.SelectedItem is DirectoryModel)
                    {
                        sourcePath = (this.SelectedItem as DirectoryModel).DirectoryPath;
                    }

                    oldSelectedItem = this.SelectedItem;
                }
                else
                {
                    if (this.ContentSelectedItem is FileModel)
                    {
                        sourcePath = (this.ContentSelectedItem as FileModel).FilePath;
                    }
                    else if (this.ContentSelectedItem is DirectoryModel)
                    {
                        sourcePath = (this.ContentSelectedItem as DirectoryModel).DirectoryPath;
                    }

                    oldSelectedItem = this.ContentSelectedItem;
                }
                
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        private void Copy(object param)
        {
            try
            {
                if (this.ContentSelectedItem == null)
                {
                    if (this.SelectedItem is FileModel)
                    {
                        sourcePath = (this.SelectedItem as FileModel).FilePath;
                        isfileCopied = true;
                    }
                    else if (this.SelectedItem is DirectoryModel)
                    {
                        sourcePath = (this.SelectedItem as DirectoryModel).DirectoryPath;
                        isfileCopied = true;
                    }
                    oldSelectedItem = this.SelectedItem;
                }
                else
                {
                    if (this.ContentSelectedItem is FileModel)
                    {
                        sourcePath = (this.ContentSelectedItem as FileModel).FilePath;
                        isfileCopied = true;
                    }
                    else if (this.ContentSelectedItem is DirectoryModel)
                    {
                        sourcePath = (this.ContentSelectedItem as DirectoryModel).DirectoryPath;
                        isfileCopied = true;
                    }
                    oldSelectedItem = this.ContentSelectedItem;
                }
               
            }
            catch (Exception ex)
            {                
              Console.WriteLine(ex.Message);
            }           
        }

        private void Paste(object param)
        {
            try
            {
                if (this.ContentSelectedItem == null)
                {
                    if (this.SelectedItem is FileModel)
                        targetPath = (this.SelectedItem as FileModel).FilePath;

                    else if (this.SelectedItem is DirectoryModel)
                        targetPath = (this.SelectedItem as DirectoryModel).DirectoryPath;
                }
                else
                {
                    if (this.ContentSelectedItem is FileModel)
                        targetPath = (this.ContentSelectedItem as FileModel).FilePath;

                    else if (this.ContentSelectedItem is DirectoryModel)
                        targetPath = (this.ContentSelectedItem as DirectoryModel).DirectoryPath;
                }

                if (isfileCopied)
                {
                    if (this.oldSelectedItem is DirectoryModel)
                    {
                        string fileName = System.IO.Path.GetFileName(sourcePath);
                        System.IO.Directory.Move(sourcePath, targetPath + "//" + fileName);
                    }
                    else
                    {
                        string fileName = System.IO.Path.GetFileName(sourcePath);
                        System.IO.File.Copy(sourcePath, targetPath + "//" + fileName);
                    }
                    isfileCopied = false;
                }
                else
                {
                    if (this.oldSelectedItem is DirectoryModel)
                    {
                        string fileName = System.IO.Path.GetFileName(sourcePath);
                        System.IO.Directory.Move(sourcePath, targetPath + "//" + fileName);
                    }
                    else
                    {
                        string fileName = System.IO.Path.GetFileName(sourcePath);
                        System.IO.File.Move(sourcePath, targetPath + "//" + fileName);
                    }
                    isfileCopied = false;
                }
            }
            catch (Exception ex)
            {                
               Console.WriteLine(ex.Message);
            }
            ExtractZipFiles(false);
        }

        private void Open(object param)
        {
            if (isNeedToSave)
            {
                MessageBoxResult windowClosingResult = MessageBox.Show("Want to save your changes?" + Environment.NewLine + "If you click 'No', a recent copy of this explorer will be temporarily available.", "Assay.Package", MessageBoxButton.YesNoCancel);
                if (windowClosingResult == MessageBoxResult.Yes)
                {
                    Save(App.ZipPath);
                }
                else if (windowClosingResult == MessageBoxResult.Cancel)
                {
                    return;
                }
            }

            OpenFileDialog openDialog = new OpenFileDialog();
            if (openDialog.ShowDialog().Value)
            {
                App.ZipPath = openDialog.FileName;
                this.SelectedItem = null;
                ExtractZipFiles(true);
            }
        }

        #endregion             

        #region Pane/Layout

        private ObservableCollection<Common> paneList;

        public ObservableCollection<Common> PaneList
        {
            get
            {
                return paneList;
            }
            set
            {
                paneList = value;
                this.RaisePropertyChanged(() => this.PaneList);
            }
        }

        void CreatePane()
        {
            paneList = new ObservableCollection<Common>();

            paneList.Add(new Common() { Name = "Details Pane", IsChecked = true });
            paneList.Add(new Common() { Name = "Preview Pane", IsChecked = false });           
        }

        private ObservableCollection<Common> layout;

        public ObservableCollection<Common> LayoutList
        {
            get
            {
                return layout;
            }
            set
            {
                layout = value;
                this.RaisePropertyChanged(() => this.LayoutList);
            }
        }


        void CreateLayout()
        {
            layout = new ObservableCollection<Common>();

            layout.Add(new Common() { Name = "Details", IsChecked = true });
            layout.Add(new Common() { Name = "Content", IsChecked = false });
            layout.Add(new Common() { Name = "List", IsChecked = false });
            layout.Add(new Common() { Name = "Medium icons", IsChecked = false });
            layout.Add(new Common() { Name = "Small icons", IsChecked = false });
        }       
        

        #endregion

    }    

}
