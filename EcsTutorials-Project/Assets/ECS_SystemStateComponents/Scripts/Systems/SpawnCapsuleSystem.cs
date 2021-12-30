using Unity.Entities;
using UnityEngine;

namespace TMG.SystemStateComponents
{
    public class SpawnCapsuleSystem : SystemBase
    {
        protected override void OnUpdate()
        {
            if (Input.GetKeyDown(KeyCode.Alpha1))
            {
                var capsulePrefab = GetSingleton<PrefabData>().CapsulePrefab;
                EntityManager.Instantiate(capsulePrefab);
            }
        }
    }
}