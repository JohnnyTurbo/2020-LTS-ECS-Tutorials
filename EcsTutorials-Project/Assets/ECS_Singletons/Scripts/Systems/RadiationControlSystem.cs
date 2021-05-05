using Unity.Entities;
using UnityEngine;

namespace TMG.ECS_Singletons
{
    public class RadiationControlSystem : SystemBase
    {
        private RadiationControlData _radiationControlData;
        
        protected override void OnStartRunning()
        {
            _radiationControlData = GetSingleton<RadiationControlData>();
        }

        protected override void OnUpdate()
        {
            if (Input.GetKeyDown(_radiationControlData.SpawnButton))
            {
                if (HasSingleton<RadiationTag>())
                {
                    Debug.LogWarning("Warning, RadiationTag Singleton already exists in scene!");
                }
                else
                {
                    EntityManager.Instantiate(_radiationControlData.RadiationPrefab);
                }
            }

            if (Input.GetKeyDown(_radiationControlData.DestroyButton))
            {
                if (!HasSingleton<RadiationTag>())
                {
                    Debug.LogWarning("Warning, no RadiationTag Singleton exists to destroy!");
                }
                else
                {
                    EntityManager.DestroyEntity(GetSingletonEntity<RadiationTag>());
                }
            }
        }
    }
}