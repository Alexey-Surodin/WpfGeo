using ClassLibrary;
using ClassLibrary.PointsModel;
using System.Collections.Generic;
namespace GeoModel.Utils
{
    internal class Preprocessor
    {
       private ModelBoundary mbv;
        public MinMaxVal mmv;
        private readonly DrawerBoundary dbw;

        public Preprocessor(ModelBoundary mbv = null)
        {
            dbw = new DrawerBoundary(11, 10, 11, 10, 11, 10);
            this.mbv = mbv;
        }

        private delegate void PointAction(ref GeoPoint point);
        private void ProcessSlices(List<List<List<GeoPoint>>> points, PointAction pointAction)
        {
            if (points == null || points.Count == 0) return;
            foreach (var slice in points)
                foreach (var layer in slice)
                    foreach (var point in layer)
                    {
                        var geoPoint = point;
                        pointAction(ref geoPoint);
                    }
        }
        private void CalcMbv(List<List<List<GeoPoint>>> points)
        {
            mbv = new ModelBoundary(float.NegativeInfinity,
                float.PositiveInfinity,
                float.NegativeInfinity,
                float.PositiveInfinity);

            foreach (var slice in points)
                foreach (var layer in slice)
                    foreach (var point in layer)
                    {
                        mbv.MaxX = point.X > mbv.MaxX ? point.X : mbv.MaxX;
                        mbv.MinX = point.X < mbv.MinX ? point.X : mbv.MinX;
                        mbv.MaxY = point.Y > mbv.MaxX ? point.Y : mbv.MaxX;
                        mbv.MinY = point.Y < mbv.MinY ? point.Y : mbv.MinY;
                    }
            mbv.MaxX *= 0.5f;
            mbv.MinX *= 0.5f;
            mbv.MaxY *= 0.5f;
            mbv.MinY *= 0.5f;
        }
        private void CalcMmv(List<List<List<GeoPoint>>> points)
        {
            mmv = new MinMaxVal();
            foreach (var slice in points)
                foreach (var layer in slice)
                    foreach (var point in layer)
                    {

                        if (mmv.maxX < point.X && point.X < mbv.MaxX) mmv.maxX = point.X;
                        if (mmv.minX > point.X && point.X > mbv.MinX) mmv.minX = point.X;

                        if (mmv.maxY < point.Y && point.Y < mbv.MaxY) mmv.maxY = point.Y;
                        if (mmv.minY > point.Y && point.Y > mbv.MinY) mmv.minY = point.Y;

                        if (mmv.maxZ < point.Z && !float.IsInfinity(point.Z)) mmv.maxZ = point.Z;
                        if (mmv.minZ > point.Z) mmv.minZ = point.Z;

                        if (mmv.maxRho < point.Rho) mmv.maxRho = point.Rho;
                        if (mmv.minRho > point.Rho) mmv.minRho = point.Rho;
                    }
        }
        private void PointNormalizeX(ref GeoPoint point)
        {
            if (point.X >= mbv.MaxX)
            {
                point.X = dbw.HardBorderX;
                point.IsBorder = true;
            }
            else if (point.X <= mbv.MinX)
            {
                point.X = -dbw.HardBorderX;
                point.IsBorder = true;
            }
            else
            {
                point.X = (2 * point.X - (mmv.maxX + mmv.minX)) / mmv.XyNorm * dbw.SoftBorderX;
            }
        }
        private void PointNormalizeY(ref GeoPoint point)
        {
            if (point.Y >= mbv.MaxY)
            {
                point.Y = dbw.HardBorderY;
                point.IsBorder = true;
            }
            else if (point.Y <= mbv.MinY)
            {
                point.Y = -dbw.HardBorderY;
                point.IsBorder = true;
            }
            else
            {
                point.Y = (2 * point.Y - (mmv.maxY + mmv.minY)) / mmv.XyNorm * dbw.SoftBorderY;
            }
        }
        private void PointNormalizeZ(ref GeoPoint point)
        {
            if (float.IsInfinity(point.Z)) point.Z = dbw.HardBorderZ;
            else
            {
                point.Z = (2 * point.Z - (mmv.maxZ + mmv.minZ)) / (mmv.maxZ - mmv.minZ) * dbw.SoftBorderZ;
            }
        }
        private void PointUpdateColor(ref GeoPoint point, ColorPalette colorPalette)
        {
            point.Color = colorPalette.GetColorByRho(point.Rho);
        }
        private void PointHideBorder(ref GeoPoint point)
        {
            if (point.IsBorder) point.Visible = false;
        }

        public void Process(List<List<List<GeoPoint>>> points, ScaleFactor scaleFactor)
        {
            if (mbv == null) CalcMbv(points);
            if (mmv == null) CalcMmv(points);
            PointAction act = PointNormalizeX;
            act += PointNormalizeY;
            act += PointNormalizeZ;
            act += PointHideBorder;
            ProcessSlices(points, act);
            UpdateScale(points, scaleFactor);
        }
        public void UpdateColor(List<List<List<GeoPoint>>> points, ColorPalette colorPalette)
        {
            void Act(ref GeoPoint point) => PointUpdateColor(ref point, colorPalette);
            ProcessSlices(points, Act);
        }
        public void UpdateScale(List<List<List<GeoPoint>>> points, ScaleFactor scaleFactor)
        {
            if (points == null || points.Count == 0) return;
            foreach (var slice in points)
                for (var j = 0; j < slice.Count; j++)
                    foreach (var point in slice[j])
                    {
                        point.X *= scaleFactor.kx;
                        point.Y *= scaleFactor.ky;
                        point.Z += scaleFactor.kz * j;
                    }
        }
        public void HideBorders(List<List<List<GeoPoint>>> points)
        {
            ProcessSlices(points, PointHideBorder);
        }
    }
}
