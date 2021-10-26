using Unity.Entities;

namespace TMG.SystemStateComponents
{
    public class SpawnShadowSystem : SystemBase
    {
        private PrefabData _prefabDataSingleton;
        private EndSimulationEntityCommandBufferSystem _endSimulationEntityCommandBufferSystem;

        protected override void OnStartRunning()
        {
            _prefabDataSingleton = GetSingleton<PrefabData>();
            _endSimulationEntityCommandBufferSystem = World.GetOrCreateSystem<EndSimulationEntityCommandBufferSystem>();
        }

        protected override void OnUpdate()
        {
            var ecb = _endSimulationEntityCommandBufferSystem.CreateCommandBuffer();
            var shadowPrefab = _prefabDataSingleton.ShadowPrefab;
            Entities
                .WithAll<ShadowTag>()
                .WithNone<ShadowStateData>()
                .ForEach((Entity e) =>
                {
                    var newShadowStateData = new ShadowStateData
                    {
                        ShadowEntity = ecb.Instantiate(shadowPrefab)
                    };
                    ecb.AddComponent<ShadowStateData>(e);
                    ecb.SetComponent(e, newShadowStateData);
                }).Run();
        }
    }
}