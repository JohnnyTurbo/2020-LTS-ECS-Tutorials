using Unity.Collections;
using Unity.Entities;

namespace TMG.ECS_UI
{
    [UpdateAfter(typeof(BeginSimulationEntityCommandBufferSystem))]
    public class ApplyRadiationSystem : SystemBase
    {
        private EndSimulationEntityCommandBufferSystem _endSimulationEntityCommandBufferSystem;
        
        protected override void OnStartRunning()
        {
            RequireSingletonForUpdate<RadiationTag>();
            _endSimulationEntityCommandBufferSystem = World.GetOrCreateSystem<EndSimulationEntityCommandBufferSystem>();
        }
        
        protected override void OnUpdate()
        {
            var ecb = _endSimulationEntityCommandBufferSystem.CreateCommandBuffer().AsParallelWriter();
            var deltaTime = Time.DeltaTime;
            
            Entities.ForEach((Entity e, int entityInQueryIndex, ref TimeToLiveData timeToLiveData) =>
            {
                timeToLiveData.Value -= deltaTime;

                if (timeToLiveData.Value <= 0)
                {
                    ecb.AddComponent(entityInQueryIndex, e, new DestroyCapsuleTag());
                }
            }).ScheduleParallel();
            
            Dependency.Complete();
        }
    }
}