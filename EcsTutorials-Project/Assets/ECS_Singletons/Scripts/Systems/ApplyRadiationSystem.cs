using Unity.Entities;

namespace TMG.ECS_Singletons
{
    public class ApplyRadiationSystem : SystemBase
    {
        private EndSimulationEntityCommandBufferSystem _endSimulationEntityCommandBufferSystem;

        protected override void OnCreate()
        {
            RequireSingletonForUpdate<RadiationTag>();
            _endSimulationEntityCommandBufferSystem = World.GetOrCreateSystem<EndSimulationEntityCommandBufferSystem>();
        }
        
        protected override void OnUpdate()
        {
            var ecb = _endSimulationEntityCommandBufferSystem.CreateCommandBuffer().AsParallelWriter();
            var deltaTime = Time.DeltaTime;
            Entities.ForEach((Entity e, int entityInQueryIndex, ref TimeToLiveData timeToLive) =>
            {
                timeToLive.Value -= deltaTime;

                if (timeToLive.Value <= 0)
                {
                    ecb.DestroyEntity(entityInQueryIndex, e);
                }
                
            }).ScheduleParallel();
            
            _endSimulationEntityCommandBufferSystem.AddJobHandleForProducer(Dependency);
        }
    }
}