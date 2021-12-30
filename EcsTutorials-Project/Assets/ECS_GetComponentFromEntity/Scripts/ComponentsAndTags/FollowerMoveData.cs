using Unity.Collections;
using Unity.Entities;
using Unity.Transforms;

namespace TMG.ECS_GetComponentFromEntity
{
    [GenerateAuthoringComponent]
    public struct FollowerMoveData : IComponentData
    {
        public Entity CurrentLeaderEntity;
        public float MinFollowDistance;
        public BlobAssetReference<LeaderEntityBlobAsset> LeaderReference;
    }
}