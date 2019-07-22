using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
namespace ClassLibrary.PointsModel
{
    public class GeoTreeNode : IGeoTreeNode
    {
        public List<IGeoTreeItem> Items { get; } = new List<IGeoTreeItem>();
        public void Add(IGeoTreeItem item)
        {
            Items.Add(item);
            item.PropertyChanged += (sender, args) => OnPropertyChanged(args.PropertyName);
        }
        public string Name { get; set; }
        public bool? Visible
        {
            get
            {
                if (Items.Exists(x => x is IGeoTreeNode igx && igx.Visible == null)) return null;
                bool? res = Items.Exists(x => x.Visible);
                if (res == true && Items.Exists(x => x.Visible == false)) res = null;
                return res;
            }
            set
            {
                npcEnable = false;
                if (value == null) value = false;
                foreach (var node in Items)
                    node.Visible = (bool)value;
                npcEnable = true;
                OnPropertyChanged();
            }
        }
        private bool selected;
        public bool Selected
        {
            get => selected;
            set
            {
                npcEnable = false;
                selected = value;
                foreach (var item in Items)
                    item.Selected = value;
                npcEnable = true;
                OnPropertyChanged();
            }
        }
        public bool Expanded { get; set; }
        bool IGeoTreeItem.Visible { get => Visible ?? false; set => Visible = value; }
        private bool npcEnable = true;
        public event PropertyChangedEventHandler PropertyChanged;
        private void OnPropertyChanged([CallerMemberName]string prop = "")
        {
            if (npcEnable) PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(prop));
        }
        public List<IGeoTreeItem> FindItems(Predicate<IGeoTreeItem> func)
        {
            var values = new List<IGeoTreeItem>();
            foreach (var node in Items)
                if (node is IGeoTreeNode tn) values.AddRange(tn.FindItems(func));
            values.AddRange(Items.FindAll(func));
            return values;
        }
        
    }
}
