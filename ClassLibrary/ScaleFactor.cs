namespace ClassLibrary
{
    public class ScaleFactor
    {
        public float kx, ky, kz;

        public ScaleFactor(float kx=1, float ky=1, float kz=0)
        {
            this.kx = kx;
            this.ky = ky;
            this.kz = kz;
        }
        
    }
}
