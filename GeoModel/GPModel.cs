using ClassLibrary;
using ClassLibrary.PointsModel;
using Drawer;
using GeoModel.Utils;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Windows.Controls;
namespace GeoModel
{
    public class GpModel : TabItem, INotifyPropertyChanged
    {
        private readonly FileReader fileReader;
        private Preprocessor preprocessor;
        private readonly DrawerControl drawer;
        private readonly List<List<List<GeoPoint>>> model;
        private readonly List<List<List<GeoPoint>>> vzModel;
        private ScaleFactor scaleFactor;
        private bool paletteAutoScale;

        public GpModel()
        {
            Header = "Open File";
            drawer = new DrawerControl();
            scaleFactor = new ScaleFactor();
            ColorPalette = new ColorPalette(12);
            fileReader = new FileReader();
            model = new List<List<List<GeoPoint>>>();
            vzModel = new List<List<List<GeoPoint>>>();
            SourceModel = new ObservableCollection<GeoTreeNode>();
            VzPointsCollection = new ObservableCollection<GeoTreeNode>();
            ZLayerModel = new ObservableCollection<GeoTreeNode>();
            paletteAutoScale = true;
        }
        public void LoadModel(string filepath, ModelBoundary mbv)
        {
            preprocessor = new Preprocessor(mbv);

            foreach (var slice in fileReader.Read(filepath)) SourceModel.Add(slice);

            model.Clear();
            foreach (var slice in SourceModel)
            {
                for (var i = 0; i < slice.Items.Count; i++)
                {
                    while (ZLayerModel.Count < slice.Items.Count) ZLayerModel.Add(new GeoTreeNode());

                    ZLayerModel[i].Name = $"Z[{i}]";

                    var mSlice = new GeoTreeNode
                    {
                        Name = slice.Name
                    };

                    if (slice.Items[i] is GeoTreeNode points)
                    {
                        foreach (var point in points.Items)
                            mSlice.Add(point);
                    }

                    ZLayerModel[i].Add(mSlice);
                }

                var newSlice = new List<List<GeoPoint>>();
                foreach (var sliceItem in slice.Items)
                {
                    if (sliceItem is IGeoTreeNode layer)
                    {
                        var newLayer = new List<GeoPoint>();
                        foreach (var layerItem in layer.Items)
                        {
                            if (layerItem is GeoPoint point)
                            {
                                newLayer.Add(new GeoPoint(point));
                            }
                        }
                        newSlice.Add(newLayer);
                    }
                }
                model.Add(newSlice);
            }

            preprocessor.Process(model, ScaleFactor);
            ColorPalette.MaxRho = preprocessor.mmv.maxRho;
            ColorPalette.MinRho = preprocessor.mmv.minRho;
            preprocessor.UpdateColor(model, ColorPalette);

            drawer.Model = model;
            drawer.VzModel = vzModel;
            drawer.rhoColors = ColorPalette.rhoColors;
            Content = drawer;
            Header = filepath;
            IsLoaded = true;
            OnPropertyChanged(null);
        }

        public void LoadVzPoints(string filepath)
        {
            var vz = fileReader.ReadVzPoints(filepath);
            var vzPoints = new GeoTreeNode
            {
                Name = vz.Name
            };

            foreach (var vzItem in vz.Items)
            {
                if (vzItem is GeoPoint point)
                {
                    var vzLayer = new GeoTreeNode
                    {
                        Name = $"X={(int)point.X}; Y={(int)point.Y}"
                    };

                    var vzList = new List<IGeoTreeItem>();

                    foreach (var slice in SourceModel)
                    {
                        vzList.AddRange(slice.FindItems(p =>
                            (p as GeoPoint)?.X == point.X &
                            (p as GeoPoint)?.Y == point.Y));
                    }


                    vzList = vzList.Distinct(new GeoPointComparer()).ToList();
                    vzList.Sort(new GeoPointComparer());


                    foreach (var vzPoint in vzList)
                    {
                        if (vzPoint is GeoPoint vp)
                        {
                            var p = new GeoPoint(vp.X, vp.Y, vp.Z, vp.Rho, vp.Color);
                            vzLayer.Add(p);
                        }
                    }

                    vzPoints.Add(vzLayer);
                }
            }

            VzPointsCollection.Add(vzPoints);

            vzModel.Clear();

            foreach (var vzCollection in VzPointsCollection)
            {
                var vzSet = new List<List<GeoPoint>>();
                foreach (var vzItems in vzCollection.Items)
                {
                    if (vzItems is IGeoTreeNode vzNode)
                    {
                        var points = new List<GeoPoint>();
                        foreach (var vzItem in vzNode.Items)
                        {
                            if (vzItem is GeoPoint vzPoint)
                                points.Add(new GeoPoint(vzPoint));
                        }

                        vzSet.Add(points);
                    }
                }

                vzModel.Add(vzSet);
            }

            preprocessor.Process(vzModel, ScaleFactor);
            preprocessor.UpdateColor(vzModel, ColorPalette);
        }

        public void RemoveVzPoints(GeoTreeNode vzNode)
        {
            var index = VzPointsCollection.IndexOf(vzNode);
            if (index >= 0)
            {
                VzPointsCollection.RemoveAt(index);
                vzModel.RemoveAt(index);
            }
        }

        public void UpdatePalette()
        {
            preprocessor?.UpdateColor(model, ColorPalette);
            preprocessor?.UpdateColor(vzModel, ColorPalette);
            if (drawer != null) drawer.rhoColors = ColorPalette.rhoColors;
        }
        public void HideBorders()
        {
            preprocessor?.HideBorders(model);
            preprocessor?.HideBorders(vzModel);
        }

        public event PropertyChangedEventHandler PropertyChanged;
        private void OnPropertyChanged([CallerMemberName]string prop = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(prop));
        }

        #region Properties
        public ObservableCollection<GeoTreeNode> SourceModel { get; }
        public ObservableCollection<GeoTreeNode> VzPointsCollection { get; }
        public ObservableCollection<GeoTreeNode> ZLayerModel { get; }
        public ScaleFactor ScaleFactor
        {
            get => scaleFactor;
            set
            {
                var temp = new ScaleFactor
                {
                    kx = value.kx / scaleFactor.kx,
                    ky = value.ky / scaleFactor.ky,
                    kz = value.kz - scaleFactor.kz
                };
                preprocessor?.UpdateScale(model, temp);
                preprocessor?.UpdateScale(vzModel, temp);
                scaleFactor = value;
            }
        }
        public ColorPalette ColorPalette { get; }
        public bool PaletteAutoScale
        {
            get => paletteAutoScale;
            set
            {
                paletteAutoScale = value;
                if (value && preprocessor != null)
                {
                    ColorPalette.MaxRho = preprocessor.mmv.maxRho;
                    ColorPalette.MinRho = preprocessor.mmv.minRho;
                }
                UpdatePalette();
            }
        }
        public bool ShowAxis
        {
            get => drawer.ShowAxis;
            set => drawer.ShowAxis = value;
        }
        public bool ShowColorScale
        {
            get => drawer.ShowColorScale;
            set => drawer.ShowColorScale = value;
        }
        public bool RightHandCoordinateSystem
        {
            get => drawer.RightHandCoordinateSystem;
            set => drawer.RightHandCoordinateSystem = value;
        }
        public bool FillTriangles
        {
            get => drawer.FillTriangles;
            set => drawer.FillTriangles = value;
        }
        public new bool IsLoaded { get; private set; }
        #endregion
    }
}
