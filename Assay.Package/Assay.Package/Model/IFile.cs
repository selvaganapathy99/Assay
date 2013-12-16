using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;
using Syncfusion.Windows.Shared;

namespace Assay.Package
{
    public interface IFile
    {
        #region Properties
        string Name { get; set; }

        string Type { get; set; }

        ImageSource Icon { get; set; }

        double Size { get; set; }

        DateTime LastModifiedDate { get; set; }

        DateTime CreatedDate { get; set; }

        bool IsFile { get; set; }

        bool IsEditing { get; set; }

        #endregion
    }
}
