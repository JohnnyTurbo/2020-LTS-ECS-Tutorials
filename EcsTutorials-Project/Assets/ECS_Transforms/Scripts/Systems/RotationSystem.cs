using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;

namespace TMG.ECS_Transforms
{
    public class RotationSystem : SystemBase
    {
        protected override void OnUpdate()
        {
            var time = Time.ElapsedTime;
            Entities.ForEach((ref Rotation rotation, in RotationData rotationData) =>
            {
                var magnitude = rotationData.Magnitude;
                var frequency = rotationData.Frequency;
                rotation.Value = quaternion.Euler(magnitude * (float) math.sin(time * frequency) + magnitude + 1, 0, 0);
            }).Run();
        }
    }
}