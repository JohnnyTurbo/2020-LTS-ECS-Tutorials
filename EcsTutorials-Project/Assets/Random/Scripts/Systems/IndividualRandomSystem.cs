using Unity.Entities;
using Unity.Transforms;
using UnityEngine;
using Random = Unity.Mathematics.Random;

namespace TMG.ECS_Random
{
    public class IndividualRandomSystem : SystemBase
    {
        // Initialize Random Seed
        protected override void OnStartRunning()
        {
            Entities.ForEach((Entity e, int entityInQueryIndex, ref IndividualRandomData randomData) =>
            {
                randomData.Value = Random.CreateFromIndex((uint)entityInQueryIndex);
            }).ScheduleParallel();
        }

        
        protected override void OnUpdate()
        {
            if (Input.GetKeyDown(KeyCode.I))
            {
                Entities
                    .ForEach((ref Translation translation, ref IndividualRandomData randomData, ref Rotation rot) =>
                    {
                        translation.Value = randomData.NextPosition;
                        rot.Value = randomData.Value.NextQuaternionRotation();
                    }).ScheduleParallel();
            }
        }
    }
}