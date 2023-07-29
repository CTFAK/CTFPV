using System.Windows.Controls;

namespace Encryption_Key_Finder.InformationItems
{
    public abstract class PropertyPanel
    {
        public abstract void InitData();
        public abstract void RefreshData();
        public abstract TreeView GetPanel();
    }
}
