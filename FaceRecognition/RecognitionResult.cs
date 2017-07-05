using System.Collections.Generic;

namespace FaceRecognition
{
    public class RecognitionResult
    {
        public int Predicted { get; set; }

        public double Scores { get; set; }

        public string PredictedUserName { get; set; }
    }
}
