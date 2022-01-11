using Unity.Collections;
using Unity.Entities;
using Unity.Jobs;
using Unity.Mathematics;
using Unity.Transforms;
using UnityEngine;

namespace TMG.JobDependencies
{
    [UpdateInGroup(typeof(SimulationSystemGroup), OrderFirst = true)]
    public class PlayerInputSystem : SystemBase
    {
        private PlayerControlData _controlData;
        
        protected override void OnStartRunning()
        {
            _controlData = GetSingleton<PlayerControlData>();
            
        }

        protected override void OnUpdate()
        {
            var newInputData = new PlayerInputData
            {
                MovementThisFrame = 2.5f,
                RotationThisFrame = 0f
            };
            
            if (Input.GetKey(_controlData.ForwardKey))
            {
                newInputData.MovementThisFrame = 5f;
            }

            if (Input.GetKey(_controlData.LeftRotationKey))
            {
                newInputData.RotationThisFrame = -50f;
            }
            else if (Input.GetKey(_controlData.RightRotationKey))
            {
                newInputData.RotationThisFrame = 50f;
            }
            
            Entities.ForEach((ref PlayerInputData inputData) =>
            {
                inputData = newInputData;
            }).Schedule();
        }
    }
    
    [UpdateInGroup(typeof(SimulationSystemGroup), OrderFirst = true)]
    [UpdateAfter(typeof(PlayerInputSystem))]
    public class RotationSystem : SystemBase
    {
        public JobHandle RotationHandle { get; private set; }
        
        private EntityQuery _shipQuery;
        private PlayerLTWData _ltwData;
        
        protected override void OnStartRunning()
        {
            var controlEntity = GetSingletonEntity<PlayerControlData>();
            _ltwData = EntityManager.GetComponentData<PlayerLTWData>(controlEntity);
            _ltwData.Value = new NativeArray<LocalToWorld>(_shipQuery.CalculateEntityCount(), Allocator.Persistent);
        }

        protected override void OnUpdate()
        {
            var deltaTime = Time.DeltaTime;
            var shipLocalToWorld = _ltwData.Value;
            
            RotationHandle = Entities
                .WithStoreEntityQueryInField(ref _shipQuery)
                .ForEach((Entity e, int entityInQueryIndex, ref Rotation rotation, in PlayerInputData inputData) =>
                {
                    rotation.Value = math.mul(rotation.Value,
                        quaternion.RotateY(math.radians(inputData.RotationThisFrame * deltaTime)));
                    
                    shipLocalToWorld[entityInQueryIndex] = new LocalToWorld
                    {
                        Value = float4x4.TRS(float3.zero, rotation.Value, Vector3.one)
                    };
                }).Schedule(Dependency);
            Dependency = JobHandle.CombineDependencies(Dependency, RotationHandle);
        }
    }
    
    [UpdateInGroup(typeof(SimulationSystemGroup), OrderFirst = true)]
    [UpdateAfter(typeof(RotationSystem))]
    public class MovementSystem : SystemBase
    {
        private PlayerLTWData _ltwData;
        
        protected override void OnStartRunning()
        {
            var controlEntity = GetSingletonEntity<PlayerControlData>();
            _ltwData = EntityManager.GetComponentData<PlayerLTWData>(controlEntity);
        }

        protected override void OnUpdate()
        {
            var deltaTime = Time.DeltaTime;
            var shipLocalToWorld = _ltwData.Value;
            var rotationHandle = World.GetOrCreateSystem<RotationSystem>().RotationHandle;

            Dependency = Entities.ForEach((Entity e, int entityInQueryIndex, ref Translation translation, 
                in PlayerInputData inputData) =>
            {
                translation.Value += (shipLocalToWorld[entityInQueryIndex].Forward * inputData.MovementThisFrame *
                                      deltaTime);
            }).Schedule(rotationHandle);
        }
    }

    [UpdateInGroup(typeof(SimulationSystemGroup), OrderLast = true)]
    [UpdateAfter(typeof(MovementSystem))]
    public class CheckDistanceToTargetSystem : SystemBase
    {
        private EndSimulationEntityCommandBufferSystem _endSimulationECBSystem;

        protected override void OnStartRunning()
        {
            _endSimulationECBSystem = World.GetOrCreateSystem<EndSimulationEntityCommandBufferSystem>();
        }
        
        protected override void OnUpdate()
        {
            var ecb = _endSimulationECBSystem.CreateCommandBuffer();
            
            var targetEntity = GetSingletonEntity<TargetSingletonTag>();
            var targetPosition = EntityManager.GetComponentData<Translation>(targetEntity);
            var targetRandomPosition = EntityManager.GetComponentData<RandomPosition>(targetEntity);

            Entities
                .WithAll<PlayerParentTag>()
                .ForEach((Entity e, in LocalToWorld localToWorld) =>
                {
                    if (math.distance(localToWorld.Position, targetPosition.Value) <= 1.25f)
                    {
                        targetPosition.Value = targetRandomPosition.NextPosition;
                        ecb.SetComponent(targetEntity, targetPosition);
                        ecb.SetComponent(targetEntity, targetRandomPosition);
                    }
                }).Schedule();
            //Dependency.Complete();
            _endSimulationECBSystem.AddJobHandleForProducer(Dependency);
        }
    }
    
    [UpdateInGroup(typeof(SimulationSystemGroup), OrderLast = true)]
    [UpdateAfter(typeof(MovementSystem))]
    public class ColorSystem : SystemBase
    {
        private Entity _targetEntity;

        protected override void OnStartRunning()
        {
            _targetEntity = GetSingletonEntity<TargetSingletonTag>();
        }

        protected override void OnUpdate()
        {
            var targetPosition = EntityManager.GetComponentData<Translation>(_targetEntity);
            Entities
                .WithAll<PlayerShipTag>()
                .ForEach((Entity e, ref ShipColorData coneColor, in LocalToWorld localToWorld) =>
                {
                    var shipForwardDirection = math.normalize(localToWorld.Up);
                    var playerToTargetDirection = math.normalize(targetPosition.Value - localToWorld.Position);
                    var dot = math.clamp(math.dot(shipForwardDirection, playerToTargetDirection), 0, 1);
                    var hue = math.lerp(0f, 120f, dot) / 360f;
                    var newCol = Color.HSVToRGB(hue, 1f, 1f);
                    coneColor.Value = new float4(newCol.r, newCol.g, newCol.b, newCol.a);
                }).Schedule();
        }
    }
}