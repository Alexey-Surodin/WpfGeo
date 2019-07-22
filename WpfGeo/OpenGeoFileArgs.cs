using ClassLibrary;
namespace WpfGeo
{
    public class OpenGeoFileArgs
    {
        public string FilePath { get; }
        public ModelBoundary ModelBoundary { get; }
        public OpenGeoFileArgs(string filepath, ModelBoundary mb)
        {
            FilePath = filepath;
            ModelBoundary = mb;
        }
    }
}
