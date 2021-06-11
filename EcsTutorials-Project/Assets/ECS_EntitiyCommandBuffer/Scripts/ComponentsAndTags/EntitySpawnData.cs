using Unity.Entities;

namespace TMG.ECS_CommandBuffer
{
    [GenerateAuthoringComponent]
    public struct EntitySpawnData : IComponentData
    {
        public Entity EntityToSpawn;
        public float SpawnDelay;
        public float Timer;
    }
}