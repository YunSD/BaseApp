using Microsoft.Win32;
using OpenCvSharp;

namespace BaseApp.App.Services
{
    public class CameraService
    {

        private static volatile OpenCvSharp.VideoCapture videoCapture;

        private static volatile bool isProcess = true;

        static CameraService()
        {
            videoCapture = new OpenCvSharp.VideoCapture(0);
            videoCapture.Set(VideoCaptureProperties.FrameWidth, 1080);
            videoCapture.Set(VideoCaptureProperties.FrameHeight, 720);
            isProcess = false;
        }

        public static bool IsOpened()
        {
            return videoCapture.IsOpened();
        }

        public static OpenCvSharp.Mat Read()
        {
            OpenCvSharp.Mat frame = new OpenCvSharp.Mat();
            CameraService.Read(frame);
            return frame;
        }

        public static void Read(OpenCvSharp.Mat frame)
        {
            if (!videoCapture.IsOpened()) return;
            bool flag = videoCapture.Read(frame);
            if ((!flag || videoCapture.Brightness < 0) && !isProcess)
            {
                ResetCamera();
            }
        }


        public static void Dispose()
        {
            if (videoCapture.IsEnabledDispose) {
                videoCapture.Dispose();
            }
        }

        
        public static void ResetCamera() {
            lock (typeof(CameraService)){
                if (isProcess) return;
                CameraService.isProcess = true;

                if (videoCapture != null)
                {
                    videoCapture.Dispose();
                }
                videoCapture = new OpenCvSharp.VideoCapture(0, OpenCvSharp.VideoCaptureAPIs.DSHOW);
                videoCapture.Set(VideoCaptureProperties.FrameWidth, 1080);
                videoCapture.Set(VideoCaptureProperties.FrameHeight, 720);
                
                CameraService.isProcess = false;
            }
        }

        public static void Empty() { }
    }
}
