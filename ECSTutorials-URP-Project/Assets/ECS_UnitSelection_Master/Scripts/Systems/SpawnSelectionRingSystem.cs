using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;

namespace TMG.UnitSelection_Master
{
    public struct SelectionRingStateData : ISystemStateComponentData
    {
        public Entity SelectionUI;
    }
    
    //[UpdateAfter(typeof(UnitSelectionSystem))]
    public class SpawnSelectionRingSystem : SystemBase
    {
        private SelectionUIPrefab _selectionUIPrefab;
        private EndSimulationEntityCommandBufferSystem _endSimulationEntityCommandBufferSystem;

        protected override void OnStartRunning()
        {
            _selectionUIPrefab = GetSingleton<SelectionUIPrefab>();
            _endSimulationEntityCommandBufferSystem = World.GetOrCreateSystem<EndSimulationEntityCommandBufferSystem>();
        }

        protected override void OnUpdate()
        {
            var ecb = _endSimulationEntityCommandBufferSystem.CreateCommandBuffer();
            var selectionPrefab = _selectionUIPrefab.Value;
            Entities
                .WithAll<SelectedEntityTag>()
                .WithNone<SelectionRingStateData>()
                .ForEach((Entity selectedEntity) =>
                {
                    var selectionUI = ecb.Instantiate(selectionPrefab);
                    var newSelectionStateData = new SelectionRingStateData()
                    {
                        SelectionUI = selectionUI
                    };
                    ecb.AddComponent<SelectionRingStateData>(selectedEntity);
                    ecb.SetComponent(selectedEntity, newSelectionStateData);
                    ecb.AddComponent<Parent>(selectionUI);
                    ecb.SetComponent(selectionUI, new Parent{Value = selectedEntity});
                    ecb.AddComponent<LocalToParent>(selectionUI);
                    ecb.SetComponent(selectionUI, new LocalToParent{Value = float4x4.zero});
                }).Run();
        }
    }
    
    [UpdateAfter(typeof(SpawnSelectionRingSystem))]
    public class CleanupSelectionRingSystem : SystemBase
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
                .WithNone<SelectedEntityTag>()
                .ForEach((Entity e, in SelectionRingStateData selectionStateData) =>
                {
                    ecb.DestroyEntity(selectionStateData.SelectionUI);
                    ecb.RemoveComponent<SelectionRingStateData>(e);
                }).Run();
        }
    }
}