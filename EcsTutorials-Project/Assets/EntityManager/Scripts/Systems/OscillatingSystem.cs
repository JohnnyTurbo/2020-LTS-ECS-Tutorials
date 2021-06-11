using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;

namespace TMG.ECS_EntityManager
{
    public class OscillatingSystem : SystemBase
    {
        protected override void OnUpdate()
        {
            var time = Time.ElapsedTime;
            var deltaTime = Time.DeltaTime;
            Entities.WithAll<OscillatingTag>().ForEach((ref LocalToWorld localToWorld) =>
            {
                var newPosition = localToWorld.Position;
                newPosition.y += math.sin((float) time * 2f) * deltaTime;
                localToWorld.Value = float4x4.Translate(newPosition);
            }).Run();
        }
    }
}