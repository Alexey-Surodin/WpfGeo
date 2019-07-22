namespace ClassLibrary
{
    public class ModelBoundary
    {
        public ModelBoundary(float maxX, float minX, float maxY, float minY)
        {
            MaxX = maxX;
            MinX = minX;
            MaxY = maxY;
            MinY = minY;
        }
        public float MaxX, MinX, MaxY, MinY;
    }
}
