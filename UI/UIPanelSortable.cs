using System;
using Terraria.GameContent.UI.Elements;
using Terraria.UI;

namespace FKTModSettings.UI
{
    public abstract class UIPanelSortable : UIPanel
    {
        public int index;

        // Exists for the UIList SortMethod
        public override int CompareTo(object obj)
        {
            if(obj.GetType()== typeof(UIPanelSortable))
            {
                UIPanelSortable ups = (UIPanelSortable)obj;
                return index - ups.index;
            }
            return base.CompareTo(obj);
        }
    }
}
