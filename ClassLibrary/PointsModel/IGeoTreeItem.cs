using System.ComponentModel;
namespace ClassLibrary.PointsModel
{
    public interface IGeoTreeItem: INotifyPropertyChanged
    {
        bool Visible { get; set; }
        bool Selected { get; set; }
        bool Expanded { get; set; }        
    }
}
