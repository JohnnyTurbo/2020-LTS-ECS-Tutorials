using Unity.Entities;

namespace TMG.UnitSelection_Master
{
    public class DeleteEntitiesSystem : SystemBase
    {
        private EndSimulationEntityCommandBufferSystem _endSimulationECBSystem;
        
        protected override void OnCreate()
        {
            _endSimulationECBSystem = World.GetOrCreateSystem<EndSimulationEntityCommandBufferSystem>();
        }

        protected override void OnUpdate()
        {
            var ecb = _endSimulationECBSystem.CreateCommandBuffer();
            Entities
                .WithAll<DeleteEntityTag>()
                .ForEach((Entity e) =>
            {
                ecb.DestroyEntity(e);
            }).Run();
        }
    }
}