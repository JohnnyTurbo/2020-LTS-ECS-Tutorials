using Unity.Entities;
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
            _inputData = GetSingleton<PlayerInputData>();
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
            Entities.ForEach((ref Translation translation, in LocalToWorld localToWorld, in PlayerInputData inputData) =>
            {
                translation.Value += (localToWorld.Forward * inputData.MovementThisFrame * deltaTime);
            }).Schedule();

            Entities.ForEach((ref Rotation rotation, in LocalToWorld localToWorld, in PlayerInputData inputData) =>
            {
                rotation.Value = math.mul(rotation.Value, quaternion.RotateY(math.radians(inputData.RotationThisFrame * deltaTime)));
            }).Schedule();
        }
    }

    [UpdateInGroup(typeof(SimulationSystemGroup), OrderLast = true)]
    [UpdateAfter(typeof(MovementSystem))]
    public class ResetPlayerInputSystem : SystemBase
    {
        private PlayerInputData _defaultInputData;
        /*private JobHandle _movementJobHandle;
        private EntityQuery _playerQuery;*/
        
        protected override void OnStartRunning()
        {
            _defaultInputData.MovementThisFrame = 0f;
            _defaultInputData.RotationThisFrame = 0f;
            /*_playerQuery =
                EntityManager.CreateEntityQuery(typeof(Translation), typeof(Rotation), typeof(PlayerInputData));
            _movementJobHandle = _playerQuery.GetDependency();*/
        }

        protected override void OnUpdate()
        {
            //_movementJobHandle = _playerQuery.GetDependency();
            //_movementJobHandle.Complete();
            SetSingleton(_defaultInputData);
        }
    }
}