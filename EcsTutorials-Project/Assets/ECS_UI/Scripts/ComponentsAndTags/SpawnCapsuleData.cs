using Unity.Entities;
using Unity.Mathematics;

namespace TMG.ECS_UI
{
    public struct SpawnCapsuleData : IComponentData
    {
        public float SpawnInterval;
        public float SpawnTimer;
        public Entity EntityPrefab;
        public Random Random;
        public float3 MinSpawnPosition;
        public float3 MaxSpawnPosition;
    }
}