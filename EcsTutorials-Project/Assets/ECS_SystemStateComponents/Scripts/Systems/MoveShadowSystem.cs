using Unity.Entities;
using Unity.Transforms;

namespace TMG.SystemStateComponents
{
    public class MoveShadowSystem : SystemBase
    {
        private EndSimulationEntityCommandBufferSystem _endSimulationEntityCommandBufferSystem;

        protected override void OnCreate()
        {
            _endSimulationEntityCommandBufferSystem = World.GetOrCreateSystem<EndSimulationEntityCommandBufferSystem>();
        }

        protected override void OnUpdate()
        {
            var ecb = _endSimulationEntityCommandBufferSystem.CreateCommandBuffer();
            Entities.WithAll<ShadowTag>().ForEach((in ShadowStateData shadowStateData, in Translation translation) =>
            {
                var shadowEntity = shadowStateData.ShadowEntity;
                var shadowTranslation = GetComponent<Translation>(shadowEntity);
                shadowTranslation.Value = translation.Value;
                shadowTranslation.Value.y = 0.0001f;
                ecb.SetComponent(shadowEntity, shadowTranslation);
            }).Run();
        }
    }
}