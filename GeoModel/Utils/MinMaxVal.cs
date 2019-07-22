using System;
namespace GeoModel.Utils
{
    internal class MinMaxVal
    {
        public float minX, minY, minZ, minRho;
        public float maxX, maxY, maxZ, maxRho;
        public MinMaxVal()
        {
            minX = minY = minZ = minRho = float.MaxValue;
            maxX = maxY = maxZ = maxRho = float.MinValue;
        }
        public float XyNorm=> Math.Max(maxX - minX, maxY - minY);
    }
}
