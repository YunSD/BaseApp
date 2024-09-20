using BaseApp.App.Utils;
using BaseApp.Core.Domain;
using BaseApp.Core.UnitOfWork;
using BaseApp.Core.Utils;
using HNSW.Net;
using System.Text.Json;

namespace BaseApp.App.Services
{
    public class FaceRecognitionService
    {

        private static SmallWorld<FaceFeatureVector, float>? world;

        public static long? KNNSearch(float[] emb) { 
            if(world == null) return null;

            List<FaceFeatureVector> faceFeatureVectors = new ();
            lock (typeof(FaceRecognitionService))
            {
                IList<SmallWorld<FaceFeatureVector, float>.KNNSearchResult> faceFeatures = world.KNNSearch(new FaceFeatureVector(emb), 1);
                if (faceFeatures.Count > 0) {
                    faceFeatureVectors = faceFeatures.Select((u)=>u.Item).ToList();
                }
            }

            foreach (var item in faceFeatureVectors)
            {
                if (FaceUtil.DotEmbedding(emb, item.embedding)) {
                    return item.id;
                }
            }

            return null;
        }

        public static void LoadFaceInfo() {
            IUnitOfWork unitOfWork = ServiceProviderUtil.GetRequiredService<IUnitOfWork>();
            IRepository<SysUser> repository = unitOfWork.GetRepository<SysUser>();
            List<SysUser> all_user = repository.GetAll().ToList();
            if (all_user.Count == 0) return;
            
            var faceinfo = all_user.Where(u=> !string.IsNullOrEmpty(u.InfoFace))
                .Select(u => new FaceFeatureVector(id: u.UserId, embedding: JsonSerializer.Deserialize<float[]>(u.InfoFace))).ToList();
            
            buildANN(faceinfo);
        }


        private static void buildANN(IEnumerable<FaceFeatureVector> database)
        {
            lock (typeof(FaceRecognitionService)) {
                IReadOnlyList<FaceFeatureVector> vectors = database.ToList().AsReadOnly();
                world = new SmallWorld<FaceFeatureVector, float>(distance: (to, from) => CosineDistance.NonOptimized(from.embedding, to.embedding), DefaultRandomGenerator.Instance, new() { M = 16, LevelLambda = 1 / Math.Log(16) });
                world.AddItems(vectors);
            }
        }
    }


    public class FaceFeatureVector { 
        public long? id { get; }
        public float[] embedding { get; }

        public FaceFeatureVector(float[] embedding, long? id = null) { 
            this.id = id;
            this.embedding = embedding;
        }
    }
}
