using Unity.Entities;

namespace TMG.BlobAssets
{
    [GenerateAuthoringComponent]
    public struct LevelUpBlob : IComponentData
    {
        public BlobAssetReference<LevelUpBlobAsset> Reference;
    }
}