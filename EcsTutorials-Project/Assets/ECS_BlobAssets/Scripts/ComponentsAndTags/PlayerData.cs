using Unity.Entities;

namespace TMG.BlobAssets
{
    [GenerateAuthoringComponent]
    public struct PlayerData : IComponentData
    {
        public BlobAssetReference<LevelUpBlobAsset> LevelUpReference;
    }
}