using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;

namespace TMG.ECS_Transforms
{
    public class NonUniformScaleSystem : SystemBase
    {
        protected override void OnUpdate()
        {
            var curTime = Time.ElapsedTime;
            Entities.ForEach((ref NonUniformScale scale, in NonUniformScaleData scaleData) =>
            {
                var magnitude = scaleData.Magnitude;
                var frequency = scaleData.Frequency;
                scale.Value = new float3
                {
                    x = 1f,
                    y = magnitude * (float) math.sin(curTime * frequency) + magnitude + 1,
                    z = 1f
                };
            }).Run();
        }
    }
}