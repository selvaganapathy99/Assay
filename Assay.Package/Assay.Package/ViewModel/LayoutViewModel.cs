using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assay.Package
{
    public class LayoutViewModel
    {
        #region Ctor

        public LayoutViewModel()
        {
            layoutList = new ObservableCollection<LayoutModel>();
            CreateLayoutList();
        }

        #endregion

        #region Properties

        private ObservableCollection<LayoutModel> layoutList;

        public ObservableCollection<LayoutModel> LayoutList
        {
            get
            {
                return layoutList;
            }
            set
            {
                layoutList = value;
            }
        }
        

        #endregion

        #region Methods

        private void CreateLayoutList()
        {
            LayoutModel extraLarge = new LayoutModel() { Name = "Extra large icons", IsChecked = false };
            LayoutModel large = new LayoutModel() { Name = "Large icons", IsChecked = false };
            LayoutModel medium = new LayoutModel() { Name = "Medium icons", IsChecked = false };

            LayoutModel small = new LayoutModel() { Name = "Small icons", IsChecked = false };
            LayoutModel list = new LayoutModel() { Name = "List", IsChecked = false };
            LayoutModel details = new LayoutModel() { Name = "Details", IsChecked = true };

            LayoutModel tiles = new LayoutModel() { Name = "Tiles", IsChecked = false };
            LayoutModel content = new LayoutModel() { Name = "Content", IsChecked = false };

            layoutList.Add(extraLarge);
            layoutList.Add(large);
            layoutList.Add(medium);

            layoutList.Add(small);
            layoutList.Add(list);
            layoutList.Add(details);

            layoutList.Add(tiles);
            layoutList.Add(content);
        }

        #endregion
    }
}
