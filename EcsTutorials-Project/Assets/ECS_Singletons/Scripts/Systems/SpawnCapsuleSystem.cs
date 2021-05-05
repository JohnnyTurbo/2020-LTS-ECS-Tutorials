using Unity.Entities;
using Unity.Transforms;

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
                
                Translation newTranslation = new Translation()
                {
                    Value = _spawnCapsuleData.Random.NextFloat3(_spawnCapsuleData.MinSpawnPosition,
                        _spawnCapsuleData.MaxSpawnPosition)
                };
                
                EntityManager.SetComponentData(newEntity, newTranslation);
                _spawnCapsuleData.SpawnTimer = _spawnCapsuleData.SpawnInterval;
            }
            
            SetSingleton(_spawnCapsuleData);
        }
    }
}