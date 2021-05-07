using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;
using UnityEngine;
using Random = Unity.Mathematics.Random;

namespace TMG.ECS_Singletons
{
    public class SpawnCapsuleSystem : SystemBase
    {
        private SpawnCapsuleData _spawnCapsuleData;

        protected override void OnStartRunning()
        {
            _spawnCapsuleData = GetSingleton<SpawnCapsuleData>();
        }

        protected override void OnUpdate()
        {
            var spawnTimer = _spawnCapsuleData.SpawnTimer -= Time.DeltaTime;
            
            if (spawnTimer <= 0)
            {
                var newEntity = EntityManager.Instantiate(_spawnCapsuleData.EntityPrefab);
                
                var newTranslation = new Translation()
                {
                    Value = _spawnCapsuleData.RandomSpawnPos
                };
                
                EntityManager.SetComponentData(newEntity, newTranslation);
                _spawnCapsuleData.SpawnTimer = _spawnCapsuleData.SpawnInterval;
            }

            if (Input.GetKeyDown(_spawnCapsuleData.FunKey))
            {
                var newSpawnData = new SpawnCapsuleData()
                {
                    EntityPrefab = _spawnCapsuleData.EntityPrefab,
                    FunKey = _spawnCapsuleData.FunKey,
                    MinSpawnPosition = new float3(12.5f, 0.8f, 12.5f),
                    MaxSpawnPosition = new float3(12.5f, 0.8f, 12.5f),
                    Random = Random.CreateFromIndex(1),
                    SpawnInterval = 0.01f,
                    SpawnTimer = 0.01f,
                };
                
                SetSingleton(newSpawnData);
                _spawnCapsuleData = GetSingleton<SpawnCapsuleData>();
            }
        }
    }
}