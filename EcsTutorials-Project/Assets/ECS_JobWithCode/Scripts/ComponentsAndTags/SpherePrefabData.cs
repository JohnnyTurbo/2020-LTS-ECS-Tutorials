using Unity.Entities;

namespace ECS_JobWithCode.Scripts.ComponentsAndTags
{
    [GenerateAuthoringComponent]
    public struct SpherePrefabData : IComponentData
    {
        public Entity Prefab;
        public int SpawnCount;
    }
}