using OpenCvSharp;
using OpenCvSharp.WpfExtensions;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Threading.Channels;
using System.Windows.Media.Media3D;
using ViewFaceCore.Core;
using ViewFaceCore.Model;

namespace BaseApp.App.Utils
{
    public class CVUtil
    {
        private static readonly string ModelDirectory = "Resources\\models";

        private static readonly OpenCvSharp.CascadeClassifier cascadeClassifier = new OpenCvSharp
            .CascadeClassifier(Path.Combine(ModelDirectory, "haarcascade_frontalface_default.xml"));


        public static bool FaceDetect(OpenCvSharp.Mat mat)
        {
            Rect[] rects = cascadeClassifier.DetectMultiScale(mat, 1.05, 20, OpenCvSharp.HaarDetectionTypes.ScaleImage, new OpenCvSharp.Size(150, 150));
            if (rects.Length > 0)
            {
                DrawFocusRectangle(mat, ExpandRect(rects[0], 30), 50, OpenCvSharp.Scalar.Green, 8);
                return true;
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
