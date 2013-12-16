using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;
using Syncfusion.Windows.Shared;

namespace Assay.Package
{
    public class FileModel : NotificationObject, IFile
    {
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

        internal string FilePath  
        {
            get;
            set;
        }

        public bool IsFile
        {
            get;
            set;
        }

        public bool IsImageVisible
        {
            get;
            set;
        }
        public bool IsTextVisible
        {
            get;
            set;
        }


        public string TextContent
        {
            get;
            set;
        }
        #endregion


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
    }
}
