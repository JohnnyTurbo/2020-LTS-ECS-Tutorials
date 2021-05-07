using Unity.Entities;

namespace TMG.ECS_UI
{
    [UpdateInGroup(typeof(LateSimulationSystemGroup))]
    public class DestroyCapsuleSystem : SystemBase
    {
        private EndSimulationEntityCommandBufferSystem _endSimulationEntityCommandBufferSystem;
        private Entity _counterEntity;
        private DestroyedCapsuleCounterData _counterData;
        private DestroyedCapsuleUIData _counterUI;
        
        protected override void OnStartRunning()
        {
            RequireSingletonForUpdate<DestroyedCapsuleCounterData>();
            _counterEntity = GetSingletonEntity<DestroyedCapsuleCounterData>();
            _counterData = EntityManager.GetComponentData<DestroyedCapsuleCounterData>(_counterEntity);
            _counterUI = EntityManager.GetComponentData<DestroyedCapsuleUIData>(_counterEntity);
            _counterUI.CounterText.text = $"Capsules Destroyed: {_counterData.Value}";
            
            _endSimulationEntityCommandBufferSystem = World.GetOrCreateSystem<EndSimulationEntityCommandBufferSystem>();
        }

        protected override void OnUpdate()
        {
            var ecb = _endSimulationEntityCommandBufferSystem.CreateCommandBuffer();
            Entities.WithAll<DestroyCapsuleTag>().ForEach((Entity e) =>
            {
                var slider = EntityManager.GetComponentData<HealthBarUIData>(e).Slider;
                HealthBarPooling.Instance.ReturnSlider(slider);

                ecb.DestroyEntity(e);

                _counterData.Value++;
                _counterUI.CounterText.text = $"Capsules Destroyed: {_counterData.Value}";

            }).WithoutBurst().Run();
        }
    }
}