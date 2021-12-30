using Unity.Entities;

namespace TMG.ECS_GetComponentFromEntity
{
    public struct LeaderEntityBlobAsset : IComponentData
    {
        public BlobArray<LeaderEntity> Array;
    }
}