using FaceRecognitionDotNet;
using System.IO;

namespace BaseApp.App.Utils
{
    public class FaceUtil
    {

        private static readonly string ModelDirectory = "Models";

        public static ModelParameter FaceModelParameter() {
            return new ModelParameter
            {
                PosePredictor68FaceLandmarksModel = File.ReadAllBytes(Path.Combine(ModelDirectory, "shape_predictor_68_face_landmarks.dat")),
                PosePredictor5FaceLandmarksModel = File.ReadAllBytes(Path.Combine(ModelDirectory, "shape_predictor_5_face_landmarks.dat")),
                FaceRecognitionModel = File.ReadAllBytes(Path.Combine(ModelDirectory, "dlib_face_recognition_resnet_model_v1.dat")),
                CnnFaceDetectorModel = File.ReadAllBytes(Path.Combine(ModelDirectory, "mmod_human_face_detector.dat"))
            };
        }

        public static readonly OpenCvSharp.CascadeClassifier cascadeClassifier = new OpenCvSharp
            .CascadeClassifier(Path.Combine(ModelDirectory, "haarcascade_frontalface_default.xml"));
    }
}
