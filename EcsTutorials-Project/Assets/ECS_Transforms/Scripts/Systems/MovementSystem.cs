using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;

namespace TMG.ECS_Transforms
{
    public class MovementSystem : SystemBase
    {
        protected override void OnUpdate()
        {
            var curTime = Time.ElapsedTime;
            Entities.ForEach((Entity e, ref Translation translation, in MovementData movementData) =>
            {
                var magnitude = movementData.Magnitude;
                var frequency = movementData.Frequency;
                var newPosition = new float3
                {
                    x = magnitude * (float) math.sin(curTime * frequency) + magnitude + 1f,
                    y = 0f,
                    z = magnitude * (float) math.cos(curTime * frequency) + magnitude + 1f
                };
                translation.Value = newPosition + movementData.OriginPosition;
            }).Run();
        }
    }
}