using Accord.Imaging.Filters;
using System.Drawing;

namespace FaceRecognition
{
    public static class GrayscaleHelper
    {
        public static Bitmap ToGrayscale(Bitmap image)
        {
            return new Grayscale(0.2125, 0.7154, 0.0721).Apply(image);
        }
    }
}
