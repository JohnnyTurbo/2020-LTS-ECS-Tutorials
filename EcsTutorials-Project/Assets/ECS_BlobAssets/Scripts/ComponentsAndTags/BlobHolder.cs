using Unity.Entities;

namespace TMG.BlobAssets
{
    public struct BlobHolder : IComponentData
    {
        public BlobAssetReference<int> NumberBlob;
    }
}