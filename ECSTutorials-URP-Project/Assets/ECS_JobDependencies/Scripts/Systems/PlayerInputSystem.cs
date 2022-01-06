using Unity.Entities;
using Unity.Jobs;
using Unity.Mathematics;
using Unity.Rendering;
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
        public JobHandle CurrentJobHandle;
        
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
    public class ColorSystem : SystemBase
    {
        protected override void OnUpdate()
        {
            Entities
                .WithAll<PlayerConeTag>()
                .WithStructuralChanges()
                .ForEach((Entity e, RenderMesh renderMesh, in LocalToWorld localToWorld) =>
                {
                    var forward = localToWorld.Up;
                    forward = math.normalize(forward);
                    var toOrigin = float3.zero - localToWorld.Position;
                    toOrigin = math.normalize(toOrigin);
                    var dot = math.dot(forward, toOrigin);
                    dot = (dot + 1f) / 2f;
                    var hue = math.lerp(0f, 120f, dot);
                    hue /= 360f;
                    renderMesh.material.color = Color.HSVToRGB(hue, 1f, 1f);
                    EntityManager.SetSharedComponentData(e, renderMesh);
                }).WithoutBurst().Run();
        }
    }
    
    [UpdateInGroup(typeof(SimulationSystemGroup), OrderLast = true)]
    [UpdateAfter(typeof(ColorSystem))]
    public class ResetPlayerInputSystem : SystemBase
    {
        private PlayerInputData _defaultInputData;

        protected override void OnStartRunning()
        {
            _defaultInputData.MovementThisFrame = 0f;
            _defaultInputData.RotationThisFrame = 0f;
        }

        protected override void OnUpdate()
        {
            SetSingleton(_defaultInputData);
        }
    }
}