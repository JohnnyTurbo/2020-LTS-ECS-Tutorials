using Unity.Entities;
using Unity.Transforms;
using UnityEngine;

namespace TMG.ECS_Random
{
    public class CentralRandomSystem : SystemBase
    {
        
        protected override void OnStartRunning()
        {
            var centralRandom = GetSingleton<CentralRandomData>();
            centralRandom.Value.InitState(10);
            Debug.Log($"Initial State: {centralRandom.Value.state}");
        }

        protected override void OnUpdate()
        {
            if (Input.GetKeyDown(KeyCode.R))
            {
                var centralRandom = GetSingleton<CentralRandomData>();
                Debug.Log($"Captured State: {centralRandom.Value.state}");
                Entities
                    .WithAll<CentralRandomTag>()
                    .ForEach((ref Translation translation) =>
                    {
                        translation.Value.x = centralRandom.Value.NextFloat(0, 25f);
                    }).ScheduleParallel();
                SetSingleton(centralRandom);
            }
        }
    }
}