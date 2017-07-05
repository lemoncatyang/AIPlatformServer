using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using Accord.Imaging.Converters;
using Accord.Imaging.Filters;
using Accord.MachineLearning.VectorMachines.Learning;
using Accord.Math;
using Accord.Statistics.Analysis;
using Accord.Statistics.Kernels;
using Accord.Statistics.Models.Regression.Linear;

namespace FaceRecognition
{
    public class FaceRecognitionProcessor : IFaceRecognitionProcessor
    {
        public RecognitionResult FaceRecognition(List<Bitmap> bitmaps, Bitmap testBitmap, int[] labels)
        {
            var standardSize = new Size(100, 100);
            var bitmapConvert = new ImageToArray(min: 0, max: 1);
            var dataMatrix = new double[0][];
            var testMatrix = new double[0][];

            bitmaps.ForEach(b =>
            {
                b = new ResizeNearestNeighbor(standardSize.Width, standardSize.Height).Apply(GrayscaleHelper.ToGrayscale(b));
                double[] bitmapMatrix;
                bitmapConvert.Convert(b, out bitmapMatrix);
                dataMatrix = dataMatrix.Concatenate(bitmapMatrix);
            });

            testBitmap = new ResizeNearestNeighbor(standardSize.Width, standardSize.Height).Apply(GrayscaleHelper.ToGrayscale(testBitmap));
            double[] testBitmapMatrix;
            bitmapConvert.Convert(testBitmap, out testBitmapMatrix);
            testMatrix = testMatrix.Concatenate(testBitmapMatrix);

            var pca = new PrincipalComponentAnalysis()
            {
                Method = PrincipalComponentMethod.Center,
                Whiten = true
            };

            pca.Learn(dataMatrix);
            pca.ExplainedVariance = 0.95;


            double[][] preprocessedTraningInputs = pca.Transform(dataMatrix);
            double[][] preprocessedTestInputs = pca.Transform(testMatrix);

            var preprocessedTraningInputsLabels = labels;

            // Create the multi-class learning algorithm for the machine
            var teacher = new MulticlassSupportVectorLearning<Gaussian>
            {
                // Configure the learning algorithm to use SMO to train the
                //  underlying SVMs in each of the binary class subproblems.
                Learner = (param) => new SequentialMinimalOptimization<Gaussian>()
                {
                    // Estimate a suitable guess for the Gaussian kernel's parameters.
                    // This estimate can serve as a starting point for a grid search.
                    UseKernelEstimation = true,
                    UseComplexityHeuristic = true
                },
                ParallelOptions = { MaxDegreeOfParallelism = 1 },
            };

            // Learn a machine
            var machine = teacher.Learn(preprocessedTraningInputs, preprocessedTraningInputsLabels);

            //// 以上过程执行结束后，说明SVM模型已经训练完毕


            // Obtain class predictions for each sample
            int[] predicted = machine.Decide(preprocessedTestInputs);

            // Get class scores for each sample
            double[] scores = machine.Score(preprocessedTestInputs);

            return new RecognitionResult
            {
                Predicted = predicted.ToList().First(),
                Scores = scores.ToList().First()
            };
        }

    }
}
