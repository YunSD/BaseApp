namespace BaseApp.App.Services
{
    public class CameraService
    {

        private static readonly OpenCvSharp.VideoCapture videoCapture = new OpenCvSharp.VideoCapture(0, OpenCvSharp.VideoCaptureAPIs.DSHOW);

        public static bool IsOpened()
        {
            return videoCapture.IsOpened();
        }


        public static OpenCvSharp.Mat Read()
        {
            OpenCvSharp.Mat frame = new OpenCvSharp.Mat();
            videoCapture.Read(frame);
            return frame;
        }


        public static void Dispose()
        {
            videoCapture.Dispose();
        }


    }
}
