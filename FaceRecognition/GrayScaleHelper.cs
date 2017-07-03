using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Accord.Imaging.Filters;
using System.Drawing;

namespace FaceRecognition
{
    public static class GrayScaleHelper
    {
        public static Bitmap ToGrayImage(Bitmap image)
        {
            // create grayscale filter (BT709)
            var filter = new Grayscale(0.2125, 0.7154, 0.0721);
            // apply the filter
            var grayImage = filter.Apply(image);
            return grayImage;
        }
    }
}
