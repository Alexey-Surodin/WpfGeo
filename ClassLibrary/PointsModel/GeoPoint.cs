using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;

namespace ClassLibrary.PointsModel
{
    public class GeoPoint : IGeoTreeItem
    {
        public float X { get; set; }
        public float Y { get; set; }
        public float Z { get; set; }
        public float Rho { get; set; }
        public Color Color { get; set; }
        public bool IsBorder { get; set; }

        private readonly BoolWrapper visibleWrap;
        private readonly BoolWrapper selectedWrap;

        public bool Visible
        {
            get => visibleWrap.Value;
            set
            {
                visibleWrap.Value = value; 
                OnPropertyChanged();
            }
        }

        public bool Selected
        {
            get => selectedWrap.Value;
            set
            {
                selectedWrap.Value = value; 
                OnPropertyChanged();
            }
        }

        public bool Expanded { get; set; }
        
        public GeoPoint(float x, float y, float z, float rho, Color c = default(Color))
        {
            visibleWrap = new BoolWrapper(true);
            visibleWrap.PropertyChanged += (sender, args) => OnPropertyChanged("Visible");
            selectedWrap = new BoolWrapper(false);
           selectedWrap.PropertyChanged += (sender, args) => OnPropertyChanged("Selected");
            X = x;
            Y = y;
            Z = z;
            Rho = rho;
            Color = c;
        }
        public GeoPoint(GeoPoint point)
        {
            selectedWrap = point.selectedWrap;
            visibleWrap = point.visibleWrap;
            X = point.X;
            Y = point.Y;
            Z = point.Z;
            Rho = point.Rho;
            Color = point.Color;
        }

        public event PropertyChangedEventHandler PropertyChanged;
        private void OnPropertyChanged(string prop = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(prop));
        }
    }

    public class GeoPointComparer : IEqualityComparer<IGeoTreeItem>, IComparer<IGeoTreeItem>
    {
        public int Compare(IGeoTreeItem x, IGeoTreeItem y)
        {
            if (x is GeoPoint a && y is GeoPoint b)
            {
                var res = a.X > b.X ? 1 : a.X == b.X ? 0 : -1;
                if (res == 0) res = a.Y > b.Y ? 1 : a.Y == b.Y ? 0 : -1;
                if (res == 0) res = a.Z > b.Z ? 1 : a.Z == b.Z ? 0 : -1;
                if (res == 0) res = a.Rho > b.Rho ? 1 : a.Rho == b.Rho ? 0 : -1;
                return res;
            }
            return 0;
        }

        public bool Equals(IGeoTreeItem x, IGeoTreeItem y)
        {
            if (x is GeoPoint a && y is GeoPoint b)
            {
                return a.X == b.X &&
                       a.Y == b.Y &&
                       a.Z == b.Z &&
                       a.Rho == b.Rho;
            }
            return false;
        }

        public int GetHashCode(IGeoTreeItem obj)
        {
            if (obj is GeoPoint a)
            {
                var hCode = (int)(a.X * a.Y * a.Z * a.Rho);
                return hCode.GetHashCode();
            }
            return obj.GetHashCode();
        }
    }
}

