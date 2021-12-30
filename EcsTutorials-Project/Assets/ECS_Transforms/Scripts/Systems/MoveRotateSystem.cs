using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;

namespace TMG.ECS_Transforms
{
    public class MoveRotateSystem : SystemBase
    {
        protected override void OnUpdate()
        {
            var curTime = Time.ElapsedTime;
            Entities.ForEach((ref LocalToParent localToParent, in MoveRotateData moveRotateData) =>
            {
                var magnitude = moveRotateData.Magnitude;
                var frequency = moveRotateData.Frequency;
                var position = new float3
                {
                    x = magnitude * (float) math.sin(curTime * frequency) + magnitude + 1f,
                    y = 0f,
                    z = magnitude * (float) math.cos(curTime * frequency) + magnitude + 1f
                };
                position += moveRotateData.OriginPosition;
                var rotation = quaternion.Euler(magnitude * (float) math.sin(curTime * frequency) + magnitude + 1, 0, 0);
                
                localToParent.Value = new float4x4(rotation, position);
            }).Run();
        }
    }
}