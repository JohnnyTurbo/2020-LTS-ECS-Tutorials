using Unity.Entities;
using Unity.Transforms;

namespace TMG.ECS_UI
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

                var newHealthBarUI = EntityManager.GetComponentData<HealthBarUIData>(newEntity);
                newHealthBarUI.Slider = HealthBarPooling.Instance.GetNextSlider();
                
                _spawnCapsuleData.SpawnTimer = _spawnCapsuleData.SpawnInterval;
            }
        }
    }
}