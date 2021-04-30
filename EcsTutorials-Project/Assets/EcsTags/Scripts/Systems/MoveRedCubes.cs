using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;

namespace TMG.Tags
{
    public class MoveRedCubes : SystemBase
    {
        private EndSimulationEntityCommandBufferSystem _endSimulationEntityCommandBufferSystem;

        protected override void OnCreate()
        {
            _endSimulationEntityCommandBufferSystem = World.GetOrCreateSystem<EndSimulationEntityCommandBufferSystem>();
        }

        protected override void OnUpdate()
        {
            var ecb = _endSimulationEntityCommandBufferSystem.CreateCommandBuffer().AsParallelWriter();
            var deltaTime = Time.DeltaTime;
            Entities
                .WithAll<RedSphereTag>()
                .ForEach((Entity e, int entityInQueryIndex, ref Translation translation) =>
                {
                    translation.Value.y += 0.25f * deltaTime;
                    ecb.SetComponent(entityInQueryIndex, e, translation);
                }).ScheduleParallel();
            _endSimulationEntityCommandBufferSystem.AddJobHandleForProducer(this.Dependency);
        }
    }
}