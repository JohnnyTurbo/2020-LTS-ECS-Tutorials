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
        
        protected override void OnCreate()
        {
            RequireSingletonForUpdate<SelectionColliderTag>();
            
            _stepPhysicsWorld = World.GetOrCreateSystem<StepPhysicsWorld>();
            _buildPhysicsWorld = World.GetOrCreateSystem<BuildPhysicsWorld>();
            _endFixedECBSystem = World.GetOrCreateSystem<EndFixedStepSimulationEntityCommandBufferSystem>();
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

            var selectionEntity = GetSingletonEntity<SelectionColliderTag>();

            if (HasComponent<StepsToLiveData>(selectionEntity))
            {
                var stepsToLive = GetComponent<StepsToLiveData>(selectionEntity);
                stepsToLive.Value--;
                ecb.SetComponent(selectionEntity, stepsToLive);
                if (stepsToLive.Value <= 0)
                {
                    ecb.DestroyEntity(selectionEntity);
                }
            }
            else
            {
                ecb.AddComponent<StepsToLiveData>(selectionEntity);
                ecb.SetComponent(selectionEntity, new StepsToLiveData{Value = 1});
            }
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

            var isBodyAUnit = Units.HasComponent(entityA);
            var isBodyBUnit = Units.HasComponent(entityB);

            if ((isBodyASelection && !isBodyBUnit) || (isBodyBSelection && !isBodyAUnit))
            {
                return;
            }

            var selectedUnit = isBodyASelection ? entityB : entityA;
            ECB.AddComponent<SelectedEntityTag>(selectedUnit);
        }
    }
}