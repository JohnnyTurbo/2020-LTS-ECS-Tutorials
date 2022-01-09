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
        private PlayerInputData _inputData;
        private PlayerControlData _controlData;
        
        protected override void OnStartRunning()
        {
            _controlData = GetSingleton<PlayerControlData>();
        }

        protected override void OnUpdate()
        {
            _inputData = GetSingleton<PlayerInputData>();
            if (Input.GetKey(_controlData.ForwardKey))
            {
                _inputData.MovementThisFrame = 5f;
            }

            if(Input.GetKey(_controlData.LeftRotationKey))
            {
                _inputData.RotationThisFrame = -50f;
            }
            else if(Input.GetKey(_controlData.RightRotationKey))
            {
                _inputData.RotationThisFrame = 50f;
            }
            SetSingleton(_inputData);
        }
    }

    [UpdateInGroup(typeof(SimulationSystemGroup), OrderFirst = true)]
    [UpdateAfter(typeof(PlayerInputSystem))]
    public class MovementSystem : SystemBase
    {
        protected override void OnUpdate()
        {
            var deltaTime = Time.DeltaTime;
            var shipLocalToWorld = new NativeArray<LocalToWorld>(1, Allocator.TempJob);
            
            var rotationHandle = Entities.ForEach((ref Rotation rotation, in PlayerInputData inputData) =>
            {
                rotation.Value = 
                    math.mul(rotation.Value, quaternion.RotateY(math.radians(inputData.RotationThisFrame * deltaTime)));
                shipLocalToWorld[0] = new LocalToWorld {Value = float4x4.TRS(float3.zero, rotation.Value, Vector3.one)};
            }).Schedule(Dependency);

            var translationDependencies = JobHandle.CombineDependencies(Dependency, rotationHandle);
            
            var translationHandle = Entities.ForEach((ref Translation translation, in PlayerInputData inputData) =>
            {
                translation.Value += (shipLocalToWorld[0].Forward * inputData.MovementThisFrame * deltaTime);
            }).Schedule(translationDependencies);
            
            translationHandle.Complete();
        }
    }

    [UpdateInGroup(typeof(SimulationSystemGroup), OrderLast = true)]
    [UpdateAfter(typeof(MovementSystem))]
    public class CheckDistanceToTargetSystem : SystemBase
    {
        protected override void OnUpdate()
        {
            var playerEntity = GetSingletonEntity<PlayerInputData>();
            var playerPosition = EntityManager.GetComponentData<Translation>(playerEntity);
            
            Entities
                .WithAll<TargetSingletonTag>()
                .ForEach((ref Translation translation, ref RandomPosition randomPosition) =>
                {
                    if (math.distance(translation.Value, playerPosition.Value) <= 1.25f)
                    {
                        translation.Value = randomPosition.NextPosition;
                    }
                }).Schedule();
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
                }).Run();
        }
    }

    [UpdateInGroup(typeof(SimulationSystemGroup), OrderLast = true)]
    [UpdateAfter(typeof(ColorSystem))]
    public class ResetPlayerInputSystem : SystemBase
    {
        private PlayerInputData _defaultInputData;

        protected override void OnStartRunning()
        {
            _defaultInputData.MovementThisFrame = 2.5f;
            _defaultInputData.RotationThisFrame = 0f;
        }

        protected override void OnUpdate()
        {
            SetSingleton(_defaultInputData);
        }
    }
}