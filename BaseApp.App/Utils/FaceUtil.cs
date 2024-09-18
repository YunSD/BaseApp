using FaceAiSharp;
using OpenCvSharp;
using OpenCvSharp.Face;
using SixLabors.ImageSharp.PixelFormats;
using System.IO;

namespace BaseApp.App.Utils
{
    public class FaceUtil
    {
        private static readonly string ModelDirectory = "Resources\\models";

        private static readonly OpenCvSharp.CascadeClassifier cascadeClassifier = new OpenCvSharp
            .CascadeClassifier(Path.Combine(ModelDirectory, "haarcascade_frontalface_default.xml"));

        private static readonly LBPHFaceRecognizer lBPHFaceRecognizer = LBPHFaceRecognizer.Create();

        private static readonly IFaceDetectorWithLandmarks faceDetector = FaceAiSharpBundleFactory.CreateFaceDetectorWithLandmarks();
        private static readonly IFaceEmbeddingsGenerator faceEmbeddings = FaceAiSharpBundleFactory.CreateFaceEmbeddingsGenerator();


        /// <summary>
        /// 通过 FaceAI 获取人脸的数据矩阵
        /// </summary>
        /// <param name="frame"></param>
        /// <returns></returns>
        /// <exception cref="NullReferenceException"></exception>
        public static float[]? GenerateEmbedding(OpenCvSharp.Mat frame)
        {
            if (frame.Empty()) throw new NullReferenceException();

            SixLabors.ImageSharp.Image<Rgb24> image = SixLabors.ImageSharp.Image.Load<Rgb24>(frame.ToMemoryStream());

            IReadOnlyCollection<FaceDetectorResult> faces = faceDetector.DetectFaces(image);

            float[]? result = null;
            if (faces.Count == 1)
            {
                faceEmbeddings.AlignFaceUsingLandmarks(image, faces.First().Landmarks!);
                result = faceEmbeddings.GenerateEmbedding(image);
            }
            image.Dispose();
            return result;
        }

        public static bool DotEmbedding(float[] first, float[] second)
        {
            float dot = FaceAiSharp.Extensions.GeometryExtensions.Dot(first, second);
            if (dot >= 0.60) return true;
            return false;
        }


        public static void FaceDetect(OpenCvSharp.Mat mat)
        {
            Rect[] rects = cascadeClassifier.DetectMultiScale(mat, 1.05, 15, OpenCvSharp.HaarDetectionTypes.ScaleImage, new OpenCvSharp.Size(100, 100));
            if (rects.Length == 1)
            {
                Mat faceROI = new Mat(mat, rects[0]);
                DrawFocusRectangle(mat, rects[0], 50, OpenCvSharp.Scalar.Green, 8);
            }
        }


        private static Rect ExpandRect(Rect rect, int padding)
        {
            // 计算扩展后的矩形坐标和尺寸
            int x = rect.X - padding;
            int y = rect.Y - padding;
            int width = rect.Width + 2 * padding;
            int height = rect.Height + 2 * padding;

            return new Rect(x, y, width, height);
        }

        private static void DrawFocusRectangle(Mat mat, Rect rect, int cornerLength, Scalar color, int thickness)
        {
            rect = ExpandRect(rect, 30);

            // 左上角
            Cv2.Line(mat, new OpenCvSharp.Point(rect.X, rect.Y), new OpenCvSharp.Point(rect.X + cornerLength, rect.Y), color, thickness); // 水平线
            Cv2.Line(mat, new OpenCvSharp.Point(rect.X, rect.Y), new OpenCvSharp.Point(rect.X, rect.Y + cornerLength), color, thickness); // 垂直线

            // 右上角
            Cv2.Line(mat, new OpenCvSharp.Point(rect.X + rect.Width, rect.Y), new OpenCvSharp.Point(rect.X + rect.Width - cornerLength, rect.Y), color, thickness); // 水平线
            Cv2.Line(mat, new OpenCvSharp.Point(rect.X + rect.Width, rect.Y), new OpenCvSharp.Point(rect.X + rect.Width, rect.Y + cornerLength), color, thickness); // 垂直线

            // 左下角
            Cv2.Line(mat, new OpenCvSharp.Point(rect.X, rect.Y + rect.Height), new OpenCvSharp.Point(rect.X + cornerLength, rect.Y + rect.Height), color, thickness); // 水平线
            Cv2.Line(mat, new OpenCvSharp.Point(rect.X, rect.Y + rect.Height), new OpenCvSharp.Point(rect.X, rect.Y + rect.Height - cornerLength), color, thickness); // 垂直线

            // 右下角
            Cv2.Line(mat, new OpenCvSharp.Point(rect.X + rect.Width, rect.Y + rect.Height), new OpenCvSharp.Point(rect.X + rect.Width - cornerLength, rect.Y + rect.Height), color, thickness); // 水平线
            Cv2.Line(mat, new OpenCvSharp.Point(rect.X + rect.Width, rect.Y + rect.Height), new OpenCvSharp.Point(rect.X + rect.Width, rect.Y + rect.Height - cornerLength), color, thickness); // 垂直线
        }
    }
}
