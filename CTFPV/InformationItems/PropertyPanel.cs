using System.Collections.Generic;
using System.Windows.Controls;

namespace Encryption_Key_Finder.InformationItems
{
    public abstract class PropertyPanel
    {
        public string latestParentPointer { get; set; }
        public abstract void InitData(string parentPointer);
        public abstract void RefreshData(string parentPointer);
        public abstract List<TreeViewItem> GetPanel();
        public abstract void RefreshPanel(ref TreeView panel);
    }
}
