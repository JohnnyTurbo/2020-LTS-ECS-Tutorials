using Unity.Entities;
using Unity.Mathematics;
using UnityEngine;
using Random = Unity.Mathematics.Random;

namespace TMG.ECS_Singletons
{
    public struct SpawnCapsuleData : IComponentData
    {
        public float SpawnInterval;
        public float SpawnTimer;
        public Entity EntityPrefab;
        public KeyCode FunKey;
        public float3 MinSpawnPosition;
        public float3 MaxSpawnPosition;
        public Random Random;
        public float3 RandomSpawnPos => Random.NextFloat3(MinSpawnPosition, MaxSpawnPosition);
    }
}