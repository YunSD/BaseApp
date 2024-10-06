namespace BaseApp.App.Utils
{
    using HandyControl.Tools.Extension;
    using Microsoft.ML.OnnxRuntime;
    using Microsoft.ML.OnnxRuntime.Tensors;
    using OpenCvSharp;
    using System;
    using System.Drawing;
    using System.Drawing.Imaging;

    /// <summary>
    /// Defines the <see cref="FaceAntiSpoofing" />
    /// </summary>
    public class FaceAntiSpoofing
    {
        private readonly InferenceSession first_session_27;

        private readonly InferenceSession second_session_400;

        /// <summary>
        /// Initializes a new instance of the <see cref="FaceAntiSpoofing"/> class.
        /// </summary>
        /// <param name="modelPath_27">The modelPath_27<see cref="string"/></param>
        /// <param name="modelPath_400">The modelPath_400<see cref="string"/></param>
        public FaceAntiSpoofing(string modelPath_27, string modelPath_400)
        {
            first_session_27 = new InferenceSession(modelPath_27);
            second_session_400 = new InferenceSession(modelPath_400);
        }

        /// <summary>
        /// The DetectorConfidence
        /// </summary>
        /// <param name="frame">The frame<see cref="OpenCvSharp.Mat"/></param>
        /// <param name="personRect">The personRect<see cref="Rect"/></param>
        /// <returns>The <see cref="float"/></returns>
        public bool DetectorConfidence(OpenCvSharp.Mat frame, Rect personRect)
        {
            // 最大容忍
            if (personRect.Width * 2 > frame.Width || personRect.Height * 2 > frame.Height) return false;
            // pre
            Rect first_rect_27 = CalculateBox(personRect, frame.Cols, frame.Rows, 2.7f);
            Rect second_rect_40 = CalculateBox(personRect, frame.Cols, frame.Rows, 4.0f);

            Mat mm = new Mat(frame, first_rect_27);

            Mat first_frame_27 = Crop(frame, first_rect_27, 2.7f, 80, 80);
            Mat second_frame_40 = Crop(frame, second_rect_40, 4.0f, 80, 80);

            Tensor<float> first_tensor = ToTensor(first_frame_27);
            Tensor<float> second_tensor = ToTensor(second_frame_40);

            // 准备ONNX模型的输入
            string first_inputName = first_session_27.InputMetadata.Keys.First();
            var first_input = new List<NamedOnnxValue> { NamedOnnxValue.CreateFromTensor(first_inputName, first_tensor) };
            var first_out = first_session_27.Run(first_input);

            string second_inputName = second_session_400.InputMetadata.Keys.First();
            var second_input = new List<NamedOnnxValue> { NamedOnnxValue.CreateFromTensor(second_inputName, second_tensor) };
            var second_out = second_session_400.Run(first_input);


            DenseTensor<float> first_result = first_out.First().Value as DenseTensor<float> ?? first_out.First().AsTensor<float>().ToDenseTensor();
            DenseTensor<float> second_result = second_out.First().Value as DenseTensor<float> ?? second_out.First().AsTensor<float>().ToDenseTensor();

            var result = MergeTensors(first_result, second_result);

            (int a, float b) = Softmax(result.ToArray());
            if (a == 1) return true;
            return false;
        }

        /// <summary>
        /// The ToTensor
        /// </summary>
        /// <param name="frame">The frame<see cref="OpenCvSharp.Mat"/></param>
        /// <returns>The <see cref="Tensor{float}"/></returns>
        public static Tensor<float> ToTensor(OpenCvSharp.Mat frame)
        {
            // 将 Bitmap 转换为浮点张量
            var width = frame.Width;
            var height = frame.Height;
            var channels = frame.Channels(); // 假设为 RGB

            // 创建一个新的张量用于图像
            var tensor = new DenseTensor<float>(new[] { 1, channels, height, width });

            // 遍历图像像素并填充张量
            for (int y = 0; y < height; y++)
            {
                for (int x = 0; x < width; x++)
                {
                    Vec3b pixelValue = frame.At<Vec3b>(y, x);
                    tensor[0, 0, y, x] = pixelValue.Item0; // 蓝色通道
                    tensor[0, 1, y, x] = pixelValue.Item1; // 绿色通道
                    tensor[0, 2, y, x] = pixelValue.Item2; // 红色通道
                }
            }

            return tensor;
        }

        /// <summary>
        /// The Softmax
        /// </summary>
        /// <param name="prob">The prob<see cref="float[]"/></param>
        /// <returns>The <see cref="bool"/></returns>
        public (int, float) Softmax(float[] prob)
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
            int _maxPos = Array.IndexOf(prob, prob.Max());
            float _prob = result[_maxPos];
            return (_maxPos, _prob);
        }


        /// <summary>
        /// The CalculateBox
        /// </summary>
        /// <param name="faceBbox">The faceBbox<see cref="Rect"/></param>
        /// <param name="width">The width<see cref="int"/></param>
        /// <param name="height">The height<see cref="int"/></param>
        /// <param name="_scale">The _scale<see cref="float"/></param>
        /// <returns>The <see cref="Rect"/></returns>
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

        /// <summary>
        /// The Crop
        /// </summary>
        /// <param name="orgImg">The orgImg<see cref="Mat"/></param>
        /// <param name="bbox">The bbox<see cref="Rect"/></param>
        /// <param name="scale">The scale<see cref="float"/></param>
        /// <param name="outW">The outW<see cref="int"/></param>
        /// <param name="outH">The outH<see cref="int"/></param>
        /// <returns>The <see cref="Mat"/></returns>
        private static Mat Crop(Mat orgImg, Rect bbox, float scale, int outW, int outH)
        {
            int srcW = orgImg.Width;
            int srcH = orgImg.Height;

            // 获取新的裁剪框
            (int leftTopX, int leftTopY, int rightBottomX, int rightBottomY) = GetScaleBox(srcW, srcH, bbox, scale);

            // 确保裁剪框在图像范围内
            if (leftTopX < 0 || leftTopY < 0 || rightBottomX > srcW - 1 || rightBottomY > srcH - 1)
            {
                throw new ArgumentOutOfRangeException("裁剪区域超出图像边界！");
            }

            // 裁剪图像
            Mat croppedImg = new Mat(orgImg, new Rect(leftTopX, leftTopY, rightBottomX - leftTopX + 1, rightBottomY - leftTopY + 1));
            // 调整大小
            Mat dstImg = new Mat();
            Cv2.Resize(croppedImg, dstImg, new OpenCvSharp.Size(outW, outH));
            return dstImg;
        }

        /// <summary>
        /// The GetScaleBox
        /// </summary>
        /// <param name="srcW">The srcW<see cref="int"/></param>
        /// <param name="srcH">The srcH<see cref="int"/></param>
        /// <param name="bbox">The bbox<see cref="Rect"/></param>
        /// <param name="scale">The scale<see cref="float"/></param>
        /// <returns>The <see cref="(int, int, int, int)"/></returns>
        private static (int, int, int, int) GetScaleBox(int srcW, int srcH, Rect bbox, float scale)
        {
            int x = bbox.X;
            int y = bbox.Y;
            int boxW = bbox.Width;
            int boxH = bbox.Height;

            // 计算新的缩放比例
            scale = Math.Min((srcH - 1) / (float)boxH, Math.Min((srcW - 1) / (float)boxW, scale));

            int newWidth = (int)(boxW * scale);
            int newHeight = (int)(boxH * scale);
            float centerX = boxW / 2f + x;
            float centerY = boxH / 2f + y;

            int leftTopX = (int)(centerX - newWidth / 2f);
            int leftTopY = (int)(centerY - newHeight / 2f);
            int rightBottomX = (int)(centerX + newWidth / 2f);
            int rightBottomY = (int)(centerY + newHeight / 2f);

            // 边界处理
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
            if (rightBottomX > srcW - 1)
            {
                leftTopX -= rightBottomX - srcW + 1;
                rightBottomX = srcW - 1;
            }
            if (rightBottomY > srcH - 1)
            {
                leftTopY -= rightBottomY - srcH + 1;
                rightBottomY = srcH - 1;
            }

            return (leftTopX, leftTopY, rightBottomX, rightBottomY);
        }


        public static DenseTensor<float> MergeTensors(DenseTensor<float> tensorA, DenseTensor<float> tensorB)
        {
            // 检查两个张量的维度是否一致
            if (tensorA.Dimensions.Length != tensorB.Dimensions.Length)
            {
                throw new ArgumentException("两个张量的维度不一致");
            }

            for (int i = 0; i < tensorA.Dimensions.Length; i++)
            {
                if (tensorA.Dimensions[i] != tensorB.Dimensions[i])
                {
                    throw new ArgumentException("两个张量的形状不一致");
                }
            }

            // 创建一个新张量来存储结果
            DenseTensor<float> result = new DenseTensor<float>(tensorA.Dimensions);

            // 逐元素相加并求平均
            for (int i = 0; i < tensorA.Length; i++)
            {
                result.SetValue(i, (tensorA.GetValue(i) + tensorB.GetValue(i))/2);
            }

            return result;
        }
    }
}
