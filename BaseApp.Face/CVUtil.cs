using FaceAiSharp;
using FaceONNX;
using OpenCvSharp;
using OpenCvSharp.Extensions;
using System.Drawing;
using System.IO;
using UMapx.Core;

namespace BaseApp.Face.Utils
{
    public class CVUtil
    {

        private static readonly OpenCvSharp.CascadeClassifier cascadeClassifier = new OpenCvSharp
            .CascadeClassifier(Path.Combine("models", "haarcascade_frontalface_default.xml"));

        private static FaceAntiSpoofing FaceAntiSpoofing = FaceAiSharpBundleFactory.CreateFaceAntiSpoofingDetector();
        public static bool FaceDetect(OpenCvSharp.Mat frame)
        {
            Rect[] rects = cascadeClassifier.DetectMultiScale(frame, 1.05, 20, OpenCvSharp.HaarDetectionTypes.ScaleImage, new OpenCvSharp.Size(150, 150));
            if (rects.Length == 1)
            {
                if (FaceAntiSpoofing.DetectorConfidence(frame, rects[0]))
                {
                    DrawFocusRectangle(frame, ExpandRect(rects[0], 30), 50, OpenCvSharp.Scalar.Green, 8);
                    return true;
                }
            }
            return false;
        }


        public static bool FaceDepthDetect(OpenCvSharp.Mat mat)
        {
            try
            {
                var labels = FaceDepthClassifier.Labels;
                FaceDepthClassifier faceDepthClassifier = new FaceDepthClassifier();
                Bitmap bitmap = BitmapConverter.ToBitmap(mat);
                var output = faceDepthClassifier.Forward(bitmap);
                var max = Matrice.Max(output, out int gender);
                string re = labels[gender];
                return gender == 1;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return false;
        }


        public static Rect ExpandRect(Rect rect, int padding)
        {
            // 计算扩展后的矩形坐标和尺寸
            int x = rect.X - padding;
            int y = rect.Y - padding;
            int width = rect.Width + 2 * padding;
            int height = rect.Height + 2 * padding;

            return new Rect(x, y, width, height);
        }

        public static void DrawFocusRectangle(Mat mat, Rect rect, int cornerLength, Scalar color, int thickness)
        {
            // 左上角
            Cv2.Line(mat, new OpenCvSharp.Point(rect.X, rect.Y), new OpenCvSharp.Point(rect.X + cornerLength, rect.Y), color, thickness, lineType: LineTypes.AntiAlias); // 水平线
            Cv2.Line(mat, new OpenCvSharp.Point(rect.X, rect.Y), new OpenCvSharp.Point(rect.X, rect.Y + cornerLength), color, thickness, lineType: LineTypes.AntiAlias); // 垂直线

            // 右上角
            Cv2.Line(mat, new OpenCvSharp.Point(rect.X + rect.Width, rect.Y), new OpenCvSharp.Point(rect.X + rect.Width - cornerLength, rect.Y), color, thickness, lineType: LineTypes.AntiAlias); // 水平线
            Cv2.Line(mat, new OpenCvSharp.Point(rect.X + rect.Width, rect.Y), new OpenCvSharp.Point(rect.X + rect.Width, rect.Y + cornerLength), color, thickness, lineType: LineTypes.AntiAlias); // 垂直线

            // 左下角
            Cv2.Line(mat, new OpenCvSharp.Point(rect.X, rect.Y + rect.Height), new OpenCvSharp.Point(rect.X + cornerLength, rect.Y + rect.Height), color, thickness, lineType: LineTypes.AntiAlias); // 水平线
            Cv2.Line(mat, new OpenCvSharp.Point(rect.X, rect.Y + rect.Height), new OpenCvSharp.Point(rect.X, rect.Y + rect.Height - cornerLength), color, thickness, lineType: LineTypes.AntiAlias); // 垂直线

            // 右下角
            Cv2.Line(mat, new OpenCvSharp.Point(rect.X + rect.Width, rect.Y + rect.Height), new OpenCvSharp.Point(rect.X + rect.Width - cornerLength, rect.Y + rect.Height), color, thickness, lineType: LineTypes.AntiAlias); // 水平线
            Cv2.Line(mat, new OpenCvSharp.Point(rect.X + rect.Width, rect.Y + rect.Height), new OpenCvSharp.Point(rect.X + rect.Width, rect.Y + rect.Height - cornerLength), color, thickness, lineType: LineTypes.AntiAlias); // 垂直线
        }
    }
}
