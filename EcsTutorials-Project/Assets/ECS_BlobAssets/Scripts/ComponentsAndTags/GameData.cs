using Unity.Entities;

namespace TMG.BlobAssets
{
    [GenerateAuthoringComponent]
    public struct GameData : IComponentData
    {
        public BlobAssetReference<LevelUpBlobAsset> LevelUpReference;
    }
}