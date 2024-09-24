
using Microsoft.ML.OnnxRuntime;
using Microsoft.ML.OnnxRuntime.Tensors;
using OpenCvSharp.Dnn;
using OpenCvSharp;

using System.Diagnostics;


namespace BaseApp.App.Utils
{
    public class FaceAntiSpoofing
    {
        private readonly InferenceSession _session;
        private float _detectorConfidence;

        public FaceAntiSpoofing(string modelPath) {
            _session = new InferenceSession(modelPath);
            _detectorConfidence = 0.6f;
        }

        public float DetectorConfidence(OpenCvSharp.Mat frame, Rect personRect) {
            // pre
            Rect rect = CalculateBox(personRect, frame.Cols, frame.Rows, 2.7f);
            Mat roi = new Mat();
            Cv2.Resize(new Mat(frame, rect), roi, new OpenCvSharp.Size(80, 80));

            Mat f1 = Cv2.ImRead("C:\\Users\\zzz\\Downloads\\Silent-Face-Anti-Spoofing-master\\images\\sample\\image_F1.jpg", ImreadModes.Color);
            var blob = CvDnn.BlobFromImage(f1, 1, new OpenCvSharp.Size(0, 0), new Scalar(0, 0, 0), true, false);
            float[] blobData = new float[blob.Total()];
            blob.GetArray(out blobData);

            // 创建 Tensor
            var inputData = new DenseTensor<float>(blobData, new[] { 1, 3, roi.Height, roi.Width });

            // 准备ONNX模型的输入
            var inputMeta = _session.InputMetadata;
            var inputName = inputMeta.Keys.First();

            var inputs = new List<NamedOnnxValue> { NamedOnnxValue.CreateFromTensor(inputName, inputData) };

            using var outputs = _session.Run(inputs);

            var firstOut = outputs.First();
            var tens = firstOut.Value as DenseTensor<float> ?? firstOut.AsTensor<float>().ToDenseTensor();
            Debug.Assert(tens.Length % 2 == 0, "Output tensor length is invalid.");

            var span = tens.Buffer.Span;
            return 1f;

        }


        private Rect CalculateBox(Rect faceBbox, int width, int height, float _scale)
        {
            int x = faceBbox.X;
            int y = faceBbox.Y;
            int bboxWidth = faceBbox.Width + 1;
            int bboxHeight = faceBbox.Height + 1;

            int shift_x = (int)(bboxWidth * 0.0f);
            int shift_y = (int)(bboxHeight * 0.0f);

            float scale = Math.Min(
                _scale,
                Math.Min((width - 1) / (float)bboxWidth, (height - 1) / (float)bboxHeight)
            );

            int bboxCenterX = bboxWidth / 2 + x;
            int bboxCenterY = bboxHeight / 2 + y;

            int newWidth = (int)(bboxWidth * scale);
            int newHeight = (int)(bboxHeight * scale);

            int leftTopX = bboxCenterX - newWidth / 2 + shift_x;
            int leftTopY = bboxCenterY - newHeight / 2 + shift_y;
            int rightBottomX = bboxCenterX + newWidth / 2 + shift_x;
            int rightBottomY = bboxCenterY + newHeight / 2 + shift_y;

            if (leftTopX < 0)
            {
                rightBottomX -= leftTopX;
                leftTopX = 0;
            }

            if (leftTopY < 0)
            {
                rightBottomY -= leftTopY;
                leftTopY = 0;
            }

            if (rightBottomX >= width)
            {
                int s = rightBottomX - width + 1;
                leftTopX -= s;
                rightBottomX -= s;
            }

            if (rightBottomY >= height)
            {
                int s = rightBottomY - height + 1;
                leftTopY -= s;
                rightBottomY -= s;
            }

            return new Rect(leftTopX, leftTopY, newWidth, newHeight);
        }
    }
}
