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
            Entities
                .WithAll<ShadowTag>()
                .WithNone<ShadowStateData>()
                .ForEach((Entity e) =>
                {
                    var newShadowStateData = new ShadowStateData
                    {
                        ShadowEntity = ecb.Instantiate(_prefabDataSingleton.ShadowPrefab)
                    };
                    ecb.AddComponent<ShadowStateData>(e);
                    ecb.SetComponent(e, newShadowStateData);
                }).WithoutBurst().Run();
        }
    }
}