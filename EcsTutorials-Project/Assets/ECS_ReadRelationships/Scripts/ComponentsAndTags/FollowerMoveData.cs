using Unity.Entities;

namespace TMG.ECS_ReadRelationships
{
    [GenerateAuthoringComponent]
    public struct FollowerMoveData : IComponentData
    {
        public Entity EntityToFollow;
        public float MinFollowDistance;
    }
}