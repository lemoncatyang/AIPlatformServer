using System.Collections.Generic;
using System.Drawing;

namespace FaceRecognition
{
    public interface IFaceRecognitionProcessor
    {
        RecognitionResult FaceRecognition(List<Bitmap> bitmaps, Bitmap testBitmap, int[] labels);
    }
}
