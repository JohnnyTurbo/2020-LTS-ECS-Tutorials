using Unity.Collections;
using Unity.Entities;
using Unity.Physics;
using Unity.Physics.Systems;

namespace TMG.UnitSelection
{
    [UpdateInGroup(typeof(FixedStepSimulationSystemGroup))]
    public class SelectMultipleUnitsJobSystem : SystemBase
    {
        private StepPhysicsWorld _stepPhysicsWorld;
        private BuildPhysicsWorld _buildPhysicsWorld;
        private EndFixedStepSimulationEntityCommandBufferSystem _endFixedECBSystem;
        private EntityQuery _activeSelectionQuery;
        
        protected override void OnCreate()
        {
            _stepPhysicsWorld = World.GetOrCreateSystem<StepPhysicsWorld>();
            _buildPhysicsWorld = World.GetOrCreateSystem<BuildPhysicsWorld>();
            _endFixedECBSystem = World.GetOrCreateSystem<EndFixedStepSimulationEntityCommandBufferSystem>();
            
            var activeSelectionQueryDesc = new EntityQueryDesc
            {
                All = new[] {ComponentType.ReadOnly<SelectionColliderTag>()},
                None = new[] {ComponentType.ReadOnly<DeleteEntityTag>()}
            };
            _activeSelectionQuery = GetEntityQuery(activeSelectionQueryDesc);
            
            RequireForUpdate(_activeSelectionQuery);
        }

        protected override void OnUpdate()
        {
            var ecb = _endFixedECBSystem.CreateCommandBuffer();

            var jobHandle = new SelectionJob
            {
                SelectionVolumes = GetComponentDataFromEntity<SelectionColliderTag>(),
                Units = GetComponentDataFromEntity<SelectableUnitTag>(),
                ECB = ecb
            }.Schedule(_stepPhysicsWorld.Simulation, ref _buildPhysicsWorld.PhysicsWorld, Dependency);
            jobHandle.Complete();

            var selectionEntities = _activeSelectionQuery.ToEntityArray(Allocator.Temp);

            foreach (var selectionEntity in selectionEntities)
            {
                if (HasComponent<FramesToLiveData>(selectionEntity))
                {
                    var deletionData = GetComponent<FramesToLiveData>(selectionEntity);
                    deletionData.Value--;
                    ecb.SetComponent(selectionEntity, deletionData);
                    if (deletionData.Value <= 0)
                    {
                        ecb.AddComponent<DeleteEntityTag>(selectionEntity);
                    }
                }
                else
                {
                    ecb.AddComponent<FramesToLiveData>(selectionEntity);
                    ecb.SetComponent(selectionEntity, new FramesToLiveData{Value = 1});
                }
            }

            selectionEntities.Dispose();
        }
    }

    public struct SelectionJob : ITriggerEventsJob
    {
        public ComponentDataFromEntity<SelectionColliderTag> SelectionVolumes;
        public ComponentDataFromEntity<SelectableUnitTag> Units;
        public EntityCommandBuffer ECB;
        
        public void Execute(TriggerEvent triggerEvent)
        {
            var entityA = triggerEvent.EntityA;
            var entityB = triggerEvent.EntityB;

            var isBodyASelection = SelectionVolumes.HasComponent(entityA);
            var isBodyBSelection = SelectionVolumes.HasComponent(entityB);

            if (isBodyASelection && isBodyBSelection)
            {
                return;
            }

            var isBodyATrash = Units.HasComponent(entityA);
            var isBodyBTrash = Units.HasComponent(entityB);

            if ((isBodyASelection && !isBodyBTrash) || (isBodyBSelection && !isBodyATrash))
            {
                return;
            }

            var selectedUnit = isBodyASelection ? entityB : entityA;
            ECB.AddComponent<SelectedEntityTag>(selectedUnit);
        }
    }
}