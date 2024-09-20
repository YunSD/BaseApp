using OpenCvSharp;

namespace BaseApp.App.Services
{
    public class CameraService
    {

        private static volatile OpenCvSharp.VideoCapture? videoCapture;

        private static SemaphoreSlim processSemaphore = new SemaphoreSlim(1, 1);

        public static void ConnectCamera()
        {
            if (!processSemaphore.Wait(0)) return;
            try
            {
                videoCapture?.Dispose();
                videoCapture = new OpenCvSharp.VideoCapture(0);
                videoCapture.Set(VideoCaptureProperties.FrameWidth, 1080);
                videoCapture.Set(VideoCaptureProperties.FrameHeight, 720);
            }
            finally
            {
                processSemaphore.Release();
            }
        }

        public static bool IsOpened()
        {
            if (!processSemaphore.Wait(0)) return false;
            try
            {
                return videoCapture?.IsOpened() ?? false;
            }
            finally
            {
                processSemaphore.Release();
            }
        }

        public static OpenCvSharp.Mat Read()
        {
            OpenCvSharp.Mat frame = new OpenCvSharp.Mat();
            CameraService.Read(frame);
            return frame;
        }

        public static void Read(OpenCvSharp.Mat frame)
        {
            if (!processSemaphore.Wait(0)) return;
            try
            {
                if (videoCapture == null || !videoCapture.IsOpened() || !videoCapture.Read(frame))
                {
                    ConnectCamera();
                }
            }
            finally
            {
                processSemaphore.Release();
            }
        }


        public static void Dispose()
        {
            processSemaphore.Wait();

            try
            {
                videoCapture?.Dispose();
            }
            finally
            {
                processSemaphore.Release();
            }
        }
    }
}
