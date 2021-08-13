using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;

namespace TMG.ECS_Transforms
{
    public class UniformScaleSystem : SystemBase
    {
        protected override void OnStartRunning()
        {
            var endSimulationEntityCommandBufferSystem = World.GetOrCreateSystem<EndSimulationEntityCommandBufferSystem>();
            
            var ecb = endSimulationEntityCommandBufferSystem.CreateCommandBuffer();
            Entities.ForEach((Entity e, in UniformScaleData scaleData) =>
            {
                var newScale = new Scale {Value = 1};
                ecb.AddComponent<Scale>(e);
                ecb.SetComponent(e, newScale);
            }).Run();
        }

        protected override void OnUpdate()
        {
            var time = Time.ElapsedTime;
            Entities.ForEach((ref Scale scale, in UniformScaleData scaleData) =>
            {
                var magnitude = scaleData.Magnitude;
                var frequency = scaleData.Frequency;
                scale.Value = magnitude * (float) math.sin(time * frequency) + magnitude + 1;
            }).Run();
        }
    }
}