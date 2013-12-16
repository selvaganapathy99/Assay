using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;
using Syncfusion.Windows.Shared;

namespace Assay.Package
{
    public class DirectoryModel : NotificationObject, IFile 
    {
        #region Ctor

        public DirectoryModel()
        {
            Items = new ObservableCollection<IFile>();
        } 

        #endregion

        #region Properties

        private string name;

        public string Name
        {
            get
            {
                return name;
            }
            set
            {
                name = value;
                this.RaisePropertyChanged(() => this.Name);
            }
        }

        public string Type
        {
            get;
            set;
        }

        public ImageSource Icon
        {
            get;
            set;
        }

        public double Size
        {
            get;
            set;
        }

        public DateTime LastModifiedDate
        {
            get;
            set;
        }

        public DateTime CreatedDate
        {
            get;
            set;
        }

        public ObservableCollection<IFile> Items { get; set; }

        internal string DirectoryPath { get; set; }

        #endregion


        public bool IsFile
        {
            get;
            set;
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

        public bool IsSelected { get; set; }
    }
}
