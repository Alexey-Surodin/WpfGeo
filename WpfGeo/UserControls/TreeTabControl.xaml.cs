using ClassLibrary.PointsModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
namespace WpfGeo.UserControls
{
    public partial class TreeTabControl
    {
        public TreeTabControl()
        {
            InitializeComponent();
        }
        private void PointTreeView_SelectedItemChanged(object sender, RoutedPropertyChangedEventArgs<object> e)
        {
            if (e.OldValue is IGeoTreeItem oldValue) oldValue.Selected = false;
            if (e.NewValue is IGeoTreeItem newValue) newValue.Selected = true;
        }
        private void PointTreeView_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (sender is TreeView treeView)
            {
                var item = (IGeoTreeItem)treeView.SelectedItem;
                if (item != null) item.Selected = false;
                treeView.Focus();
            }
        }
    }
}

//using System.Collections.ObjectModel;
//using System.Collections.Specialized;
//public static readonly DependencyProperty SourceModelProperty;
//public ObservableCollection<GeoTreeNode> SourceModel
//{
//    private get => (ObservableCollection<GeoTreeNode>)GetValue(SourceModelProperty);
//    set => SetValue(SourceModelProperty, value);
//}
//public ObservableCollection<GeoTreeNode> ZLayerModel { get; } = new ObservableCollection<GeoTreeNode>();
//static TreeTabControl()
//{
//    SourceModelProperty = DependencyProperty.Register("SourceModel",
//        typeof(ObservableCollection<GeoTreeNode>),
//        typeof(TreeTabControl),
//        new PropertyMetadata(SourceModelUpdated)
//        );
//}
//private static void SourceModelUpdated(DependencyObject d, DependencyPropertyChangedEventArgs e)
//{
//    if (d is TreeTabControl tabControl)
//    {
//        tabControl.UpdateZLayerModel();
//        if (e.NewValue is ObservableCollection<GeoTreeNode> nv) nv.CollectionChanged += tabControl.Handler;
//        if (e.OldValue is ObservableCollection<GeoTreeNode> ov) ov.CollectionChanged -= tabControl.Handler;
//    }
//}
//private NotifyCollectionChangedEventHandler Handler => (sender, args) => UpdateZLayerModel();
//private void UpdateZLayerModel()
//{
//    ZLayerModel.Clear();
//    foreach (var mSlice in SourceModel)
//    {
//        for (var i = 0; i < mSlice.Items.Count; i++)
//        {
//            while (ZLayerModel.Count < mSlice.Items.Count) ZLayerModel.Add(new GeoTreeNode());
//            ZLayerModel[i].Name = $"Z[{i}]";
//            var slice = new GeoTreeNode
//            {
//                Name = mSlice.Name
//            };
//            if (mSlice.Items[i] is GeoTreeNode points)
//            {
//                foreach (var point in points.Items)
//                    slice.Add(point);
//            }
//            ZLayerModel[i].Add(slice);
//        }
//    }
//    ZLayerTree.Items.Refresh();
//}
