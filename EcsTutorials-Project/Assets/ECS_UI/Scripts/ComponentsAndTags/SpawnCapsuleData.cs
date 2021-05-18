using Unity.Entities;
using Unity.Mathematics;
using Random = Unity.Mathematics.Random;

namespace TMG.ECS_UI
{
    public struct SpawnCapsuleData : IComponentData
    {
        public float SpawnInterval;
        public float SpawnTimer;
        public Entity EntityPrefab;
        
        #region Spawn Position Stuff - Ignore For Now
        
        public float3 MinSpawnPosition;
        public float3 MaxSpawnPosition;
        public Unity.Mathematics.Random Random;
        public float3 RandomSpawnPos => Random.NextFloat3(MinSpawnPosition, MaxSpawnPosition);
        
        #endregion
    }
}