using Unity.Entities;
using Unity.Mathematics;

namespace TMG.ECS_EntityManager
{
    [GenerateAuthoringComponent]
    public struct EntitySpawnData : IComponentData
    {
        public Entity EntityPrefab;
        public int2 SpawnGrid;
        public int2 EntitySpacing;
    }
}