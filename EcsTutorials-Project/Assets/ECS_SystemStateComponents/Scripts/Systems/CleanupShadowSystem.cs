using Unity.Entities;

namespace TMG.SystemStateComponents
{
    public class CleanupShadowSystem : SystemBase
    {
        private EndSimulationEntityCommandBufferSystem _endSimulationEntityCommandBufferSystem;

        protected override void OnCreate()
        {
            _endSimulationEntityCommandBufferSystem = World.GetOrCreateSystem<EndSimulationEntityCommandBufferSystem>();
        }

        protected override void OnUpdate()
        {
            var ecb = _endSimulationEntityCommandBufferSystem.CreateCommandBuffer();
            Entities
                .WithNone<ShadowTag>()
                .ForEach((Entity e, in ShadowStateData shadowStateData) =>
            {
                ecb.DestroyEntity(shadowStateData.ShadowEntity);
                ecb.RemoveComponent<ShadowStateData>(e);
            }).Run();
        }
    }
}