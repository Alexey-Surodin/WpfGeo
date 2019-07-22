using ClassLibrary;
using Microsoft.Win32;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Controls;
using ClassLibrary.PointsModel;
using GeoModel;
namespace WpfGeo
{
    public class GeoViewModel : INotifyPropertyChanged
    {
        public GeoViewModel()
        {
            Tabs = new ObservableCollection<TabItem>();
            NewTabCommand.Execute(null);
        }

        private TabItem selectedTab;
        public TabItem SelectedTab
        {
            get => selectedTab;
            set
            {
                selectedTab = value;
                OnPropertyChanged(null);
            }
        }

        public ObservableCollection<TabItem> Tabs { get; }

        public event PropertyChangedEventHandler PropertyChanged;
        private void OnPropertyChanged([CallerMemberName]string prop = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(prop));
        }

        #region GeoModel Properties
        public ObservableCollection<GeoTreeNode> SourceModel => ((GpModel)SelectedTab).SourceModel;

        public ObservableCollection<GeoTreeNode> VzPoints => ((GpModel)SelectedTab).VzPointsCollection;
        
        public ObservableCollection<GeoTreeNode> ZLayerModel => ((GpModel)SelectedTab).ZLayerModel;

        public bool ShowAxis
        {
            get => ((GpModel)SelectedTab).ShowAxis;
            set => ((GpModel)SelectedTab).ShowAxis = value;
        }
        public bool ShowColorScale
        {
            get => ((GpModel)SelectedTab).ShowColorScale;
            set => ((GpModel)SelectedTab).ShowColorScale = value;
        }
        public bool FillTriangles
        {
            get => ((GpModel)SelectedTab).FillTriangles;
            set => ((GpModel)SelectedTab).FillTriangles = value;
        }
        public bool RightHandCoordinateSystem
        {
            get => ((GpModel)SelectedTab).RightHandCoordinateSystem;
            set => ((GpModel)SelectedTab).RightHandCoordinateSystem = value;
        }
        public int PaletteAlpha
        {
            get => (byte)(((GpModel)SelectedTab).ColorPalette.Alpha / 2.55);
            set
            {
                ((GpModel)SelectedTab).ColorPalette.Alpha = (byte)(value * 2.55);
                ((GpModel)SelectedTab).UpdatePalette();
            }
        }
        public int PaletteNumOfColors
        {
            get => ((GpModel)SelectedTab).ColorPalette.NumOfColors;
            set
            {
                ((GpModel)SelectedTab).ColorPalette.NumOfColors = value;
                ((GpModel)SelectedTab).UpdatePalette();
            }
        }
        public float PaletteMinRho
        {
            get => ((GpModel)SelectedTab).ColorPalette.MinRho;
            set
            {
                if (!PaletteAutoScale) ((GpModel)SelectedTab).ColorPalette.MinRho = value;
                ((GpModel)SelectedTab).UpdatePalette();
            }
        }
        public float PaletteMaxRho
        {
            get => ((GpModel)SelectedTab).ColorPalette.MaxRho;
            set
            {
                if (!PaletteAutoScale) ((GpModel)SelectedTab).ColorPalette.MaxRho = value;
                ((GpModel)SelectedTab).UpdatePalette();
            }
        }
        public bool PaletteAutoScale
        {
            get => ((GpModel)SelectedTab).PaletteAutoScale;
            set
            {
                ((GpModel)SelectedTab).PaletteAutoScale = value;
                OnPropertyChanged(null);
            }
        }
        public float ScaleFactorX
        {
            get => ((GpModel)SelectedTab).ScaleFactor.kx;
            set => ((GpModel)SelectedTab).ScaleFactor = new ScaleFactor(value, ScaleFactorY, ScaleFactorZ);
        }
        public float ScaleFactorY
        {
            get => ((GpModel)SelectedTab).ScaleFactor.ky;
            set => ((GpModel)SelectedTab).ScaleFactor = new ScaleFactor(ScaleFactorX, value, ScaleFactorZ);
        }
        public float ScaleFactorZ
        {
            get => ((GpModel)SelectedTab).ScaleFactor.kz;
            set => ((GpModel)SelectedTab).ScaleFactor = new ScaleFactor(ScaleFactorX, ScaleFactorY, value);
        }
        public bool IsLoaded => ((GpModel)SelectedTab).IsLoaded;
        #endregion

        #region Commands
        // CloseTab
        private void CloseTab()
        {
            var index = Tabs.IndexOf(SelectedTab);
            if (Tabs.Count == 1)
            {
                NewTab();
                Tabs.RemoveAt(index);
            }
            else
            {
                SelectedTab = Tabs[index == 0 ? 1 : index - 1];
                Tabs.RemoveAt(index);
            }
        }
        private RelayCommand closeTab;
        public RelayCommand CloseTabCommand => closeTab ??
            (closeTab = new RelayCommand(obj => CloseTab()));

        // NewTab
        private void NewTab()
        {
            var geoTab = new GpModel();
            var ofc = new UserControls.OpenFileControl();
            geoTab.Content = ofc;
            geoTab.PropertyChanged += (sender, args) => OnPropertyChanged(null);
            Tabs.Add(geoTab);
            SelectedTab = geoTab;
        }
        private RelayCommand newTab;
        public RelayCommand NewTabCommand => newTab ??
            (newTab = new RelayCommand(obj => NewTab()));

        //LoadFile
        private void LoadFile(object obj)
        {
            if (obj is OpenGeoFileArgs args)
                ((GpModel)SelectedTab).LoadModel(args.FilePath, args.ModelBoundary);
        }
        private RelayCommand loadFileCommand;
        public RelayCommand LoadFileCommand => loadFileCommand ??
            (loadFileCommand = new RelayCommand(LoadFile));

        //LoadVZPoints
        private void LoadVzPoints()
        {
            var openFileDialog = new OpenFileDialog();
            if (openFileDialog.ShowDialog() == true)
            {
                var filepath = openFileDialog.FileName;
                ((GpModel)SelectedTab).LoadVzPoints(filepath);
            }
        }
        private RelayCommand loadVzPoints;
        public RelayCommand LoadVzPointsCommand => loadVzPoints ??
            (loadVzPoints = new RelayCommand(obj => LoadVzPoints()));

        //RemoveVZP
        private void RemoveVzp(object obj)
        {
            if (obj is GeoTreeNode gtn)
                ((GpModel)SelectedTab).RemoveVzPoints(gtn);
        }
        private RelayCommand removeVzpCommand;
        public RelayCommand RemoveVzpCommand => removeVzpCommand ??
            (removeVzpCommand = new RelayCommand(RemoveVzp));

        //HideBordersCommand
        private void HideBorders()
        {
            ((GpModel)SelectedTab).HideBorders();
        }
        private RelayCommand hideBordersCommand;
        public RelayCommand HideBordersCommand => hideBordersCommand ??
            (hideBordersCommand = new RelayCommand(obj => HideBorders()));
        #endregion
    }
}
