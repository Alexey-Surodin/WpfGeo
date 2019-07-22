using System;
using System.Collections.Generic;
namespace ClassLibrary.PointsModel
{
    public interface IGeoTreeNode : IGeoTreeItem
    {
        List<IGeoTreeItem> Items { get; }
        void Add(IGeoTreeItem item);
        
        List<IGeoTreeItem> FindItems(Predicate<IGeoTreeItem> func);
        new bool? Visible { get; }
    }
}
