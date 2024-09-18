namespace BaseApp.App.Services
{
    public class CameraService
    {

        private static readonly OpenCvSharp.VideoCapture videoCapture = new OpenCvSharp.VideoCapture(0, OpenCvSharp.VideoCaptureAPIs.DSHOW);

        public static bool IsOpened() {
            return videoCapture.IsOpened();
        }


        private static OpenCvSharp.Mat frame = new OpenCvSharp.Mat();

        public static OpenCvSharp.Mat Read() {
            videoCapture.Read(frame);
            return frame;
        }






        public static void Dispose() {
            frame.Dispose();
            videoCapture.Dispose();
        }


    }
}
