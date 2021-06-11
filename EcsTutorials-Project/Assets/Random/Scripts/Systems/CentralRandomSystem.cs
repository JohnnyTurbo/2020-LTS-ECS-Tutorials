using Unity.Entities;
using Unity.Transforms;
using UnityEngine;

namespace TMG.ECS_Random
{
    public class CentralRandomSystem : SystemBase
    {
        private CentralRandomData _centralRandom;
        
        protected override void OnStartRunning()
        {
            _centralRandom = GetSingleton<CentralRandomData>();
            _centralRandom.Value.InitState(10);
            Debug.Log($"Initial State: {_centralRandom.Value.state}");
        }

        protected override void OnUpdate()
        {
            if (Input.GetKeyDown(KeyCode.R))
            {
                var capturedRandom = _centralRandom;
                Debug.Log($"Captured State: {capturedRandom.Value.state}");
                Entities
                    .WithAll<CentralRandomTag>()
                    .ForEach((ref Translation translation) =>
                    {
                        translation.Value.x = capturedRandom.Value.NextFloat(0, 25f);
                    }).ScheduleParallel();
                SetSingleton(capturedRandom);
            }
        }
    }
}