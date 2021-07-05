using Unity.Entities;

namespace TMG.BlobAssets
{
    [GenerateAuthoringComponent]
    public struct EnemyData : IComponentData
    {
        public int MaxHealth;
        public int CurHealth;
        public int ExpValue;
    }
}