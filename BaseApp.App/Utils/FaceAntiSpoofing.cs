
using Microsoft.ML.OnnxRuntime;
using Microsoft.ML.OnnxRuntime.Tensors;
using OpenCvSharp.Dnn;
using OpenCvSharp;

using System.Diagnostics;
using System.Drawing.Imaging;
using System.Drawing;
using OpenCvSharp.Extensions;


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

            float[] blobData = PreprocessImage(roi, 80, 80);

            // 创建 Tensor
            var inputData = new DenseTensor<float>(blobData, new[] { 1, 3, roi.Height, roi.Width });

            // 准备ONNX模型的输入
            var inputMeta = _session.InputMetadata;
            var inputName = inputMeta.Keys.First();

            var inputs = new List<NamedOnnxValue> { NamedOnnxValue.CreateFromTensor(inputName, inputData) };

            using var outputs = _session.Run(inputs);

            var firstOut = outputs.First();
            var tens = firstOut.Value as DenseTensor<float> ?? firstOut.AsTensor<float>().ToDenseTensor();
            var data = tens.ToArray();
            Softmax(data);
            var span = tens.Buffer.Span;
            return 1f;

        }


        public static int Softmax(float[] prob)
        {
            float total = 0;
            float max = prob[0];
            int OUTPUT_SIZE = prob.Length;

            // 找到最大值
            for (int i = 1; i < OUTPUT_SIZE; i++)
            {
                max = Math.Max(prob[i], max);
            }

            // 计算 total（Softmax 分母）
            for (int i = 0; i < OUTPUT_SIZE; i++)
            {
                total += (float)Math.Exp(prob[i] - max);
            }

            // 计算 Softmax 输出
            List<float> result = new List<float>();
            for (int i = 0; i < OUTPUT_SIZE; i++)
            {
                result.Add((float)Math.Exp(prob[i] - max) / total);
            }

            // 找到最大概率的类别
            int maxPos = Array.IndexOf(prob, prob.Max());
            int _class = maxPos;
            float _prob  = result[maxPos];

            return 0;
        }

        private static float[] PreprocessImage(OpenCvSharp.Mat frame, int width, int height)
        {
            // Convert Mat to Bitmap
            using (var resizedImage = OpenCvSharp.Extensions.BitmapConverter.ToBitmap(frame))
            {
                // Use 24bppRgb since we only care about RGB data
                var bitmapData = resizedImage.LockBits(
                    new Rectangle(0, 0, resizedImage.Width, resizedImage.Height),
                    ImageLockMode.ReadOnly,
                    PixelFormat.Format24bppRgb);

                float[] data = new float[width * height * 3];
                int bytesPerPixel = 3; // RGB format (3 bytes per pixel)

                unsafe
                {
                    byte* ptr = (byte*)bitmapData.Scan0;
                    for (int y = 0; y < height; y++)
                    {
                        for (int x = 0; x < width; x++)
                        {
                            int pixelIndex = y * bitmapData.Stride + x * bytesPerPixel;
                            byte b = ptr[pixelIndex];       // Blue
                            byte g = ptr[pixelIndex + 1];   // Green
                            byte r = ptr[pixelIndex + 2];   // Red

                            // Normalize and assign to float array
                            int index = (y * width + x) * 3;
                            data[index] = r / 255.0f;       // Red
                            data[index + 1] = g / 255.0f;   // Green
                            data[index + 2] = b / 255.0f;   // Blue
                        }
                    }
                }

                resizedImage.UnlockBits(bitmapData);
                return data;
            }
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
