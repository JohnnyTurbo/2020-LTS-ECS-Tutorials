using Unity.Entities;
using Unity.Mathematics;

namespace TMG.ECS_EntityManager
{
    [GenerateAuthoringComponent]
    public struct EntitySpawnData : IComponentData
    {
        public int2 SpawnGrid;
        public int2 EntitySpacing;
        public Entity EntityPrefab;
    }
}