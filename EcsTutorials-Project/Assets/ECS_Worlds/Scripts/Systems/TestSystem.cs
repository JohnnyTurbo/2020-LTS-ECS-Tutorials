using Unity.Entities;
using Unity.Transforms;
using Unity.Mathematics;

namespace TMG.ECS_Worlds
{
    public class TestSystem : SystemBase
    {
        protected override void OnUpdate()
        {
            var time = Time.ElapsedTime;
            var deltaTime = Time.DeltaTime;
            Entities.WithAll<AlphaTag>().ForEach((ref LocalToWorld localToWorld) =>
            {
                var newPosition = localToWorld.Position;
                newPosition.y += math.sin((float) time * 2f) * deltaTime;
                localToWorld.Value = float4x4.Translate(newPosition);
            }).Run();
        }
    }
}