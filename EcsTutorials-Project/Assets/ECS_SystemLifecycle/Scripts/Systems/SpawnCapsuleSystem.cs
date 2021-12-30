using Unity.Entities;
using UnityEngine;

namespace TMG.SystemLifecycle
{
    public class SpawnCapsuleSystem : SystemBase
    {
        private SpawnCapsuleData _spawnCapsuleData;
        private Entity _spawnedCapsule;
        
        protected override void OnStartRunning()
        {
            _spawnCapsuleData = GetSingleton<SpawnCapsuleData>();
            var eq = new EntityQuery();
            eq.CalculateEntityCount();
        }

        protected override void OnUpdate()
        {
            if (Input.GetKeyDown(KeyCode.Alpha1))
            {
                _spawnedCapsule = EntityManager.Instantiate(_spawnCapsuleData.CapsulePrefab);
                var newEntity = EntityManager.CreateEntity(typeof(TestDC));
                EntityManager.Instantiate(newEntity);
            }

            if(Input.GetKeyDown(KeyCode.Alpha2))
            {
                EntityManager.DestroyEntity(_spawnedCapsule);
            }
        }
    }
}