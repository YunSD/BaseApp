using BaseApp.Core.Domain;
using BaseApp.Core.UnitOfWork;
using BaseApp.Core.Utils;
using FaceAiSharp;
using HNSW.Net;
using SixLabors.ImageSharp.PixelFormats;
using System.Text.Json;

namespace BaseApp.App.Services
{
    public class FaceRecognitionService
    {
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

        private static bool DotEmbedding(float[] first, float[] second)
        {
            float dot = FaceAiSharp.Extensions.GeometryExtensions.Dot(first, second);
            if (dot >= 0.60) return true;
            return false;
        }


        /// <summary>
        /// KNN 矩阵
        /// </summary>
        private static SmallWorld<FaceFeatureVector, float>? world;

        public static void LoadData()
        {
            IUnitOfWork unitOfWork = ServiceProviderUtil.GetRequiredService<IUnitOfWork>();
            IRepository<SysUser> repository = unitOfWork.GetRepository<SysUser>();
            List<SysUser> all_user = repository.GetAll().ToList();
            if (all_user.Count == 0) return;

            var faceinfo = all_user.Where(u => !string.IsNullOrEmpty(u.InfoFace))
                .Select(u => new FaceFeatureVector(id: u.UserId, embedding: JsonSerializer.Deserialize<float[]>(u.InfoFace))).ToList();

            buildANN(faceinfo);
        }

        public static long? KNNSearch(float[] emb)
        {
            if (world == null) return null;

            List<FaceFeatureVector> faceFeatureVectors = new();
            lock (typeof(FaceRecognitionService))
            {
                IList<SmallWorld<FaceFeatureVector, float>.KNNSearchResult> faceFeatures = world.KNNSearch(new FaceFeatureVector(emb), 1);
                if (faceFeatures.Count > 0)
                {
                    faceFeatureVectors = faceFeatures.Select((u) => u.Item).ToList();
                }
            }

            foreach (var item in faceFeatureVectors)
            {
                if (DotEmbedding(emb, item.embedding))
                {
                    return item.id;
                }
            }

            return null;
        }




        private static void buildANN(IEnumerable<FaceFeatureVector> database)
        {
            lock (typeof(FaceRecognitionService))
            {
                IReadOnlyList<FaceFeatureVector> vectors = database.ToList().AsReadOnly();
                world = new SmallWorld<FaceFeatureVector, float>(distance: (to, from) => CosineDistance.SIMDForUnits(from.embedding, to.embedding), DefaultRandomGenerator.Instance, new() { EnableDistanceCacheForConstruction = true, InitialDistanceCacheSize = 512, NeighbourHeuristic = NeighbourSelectionHeuristic.SelectHeuristic, KeepPrunedConnections = true, ExpandBestSelection = true });
                world.AddItems(vectors);
            }
        }
    }


    internal class FaceFeatureVector
    {
        public long? id { get; }
        public float[] embedding { get; }

        public FaceFeatureVector(float[] embedding, long? id = null)
        {
            this.id = id;
            this.embedding = embedding;
        }
    }
}
