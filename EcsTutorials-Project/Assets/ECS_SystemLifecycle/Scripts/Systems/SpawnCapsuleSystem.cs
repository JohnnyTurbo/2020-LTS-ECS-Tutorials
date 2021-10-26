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
        }

        protected override void OnUpdate()
        {
            if (Input.GetKeyDown(KeyCode.Alpha1))
            {
                _spawnedCapsule = EntityManager.Instantiate(_spawnCapsuleData.CapsulePrefab);
            }

            if(Input.GetKeyDown(KeyCode.Alpha2))
            {
                EntityManager.DestroyEntity(_spawnedCapsule);
            }
        }
    }
}