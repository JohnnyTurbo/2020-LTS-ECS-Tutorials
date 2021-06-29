using TMG.BlobAssets;
using Unity.Entities;

namespace TMG.ChangeFilter
{
    [GenerateAuthoringComponent]
    public struct BlobReferenceHolder : IComponentData
    {
        public BlobAssetReference<NumberBlobAsset> BlobRef;
    }
}