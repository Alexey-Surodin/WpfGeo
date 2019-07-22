

namespace GeoModel.Utils
{
    internal struct DrawerBoundary
    {
        public readonly float HardBorderX;
        public readonly float SoftBorderX;
        public readonly float HardBorderY;
        public readonly float SoftBorderY;
        public readonly float HardBorderZ;
        public readonly float SoftBorderZ;

        public DrawerBoundary(float hx, float sx, float hy, float sy, float hz, float sz)
        {
            HardBorderX = hx;
            SoftBorderX = sx;
            HardBorderY = hy;
            SoftBorderY = sy;
            HardBorderZ = hz;
            SoftBorderZ = sz;
        }
    }
}
