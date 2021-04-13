using Unity.Entities;
using Unity.Mathematics;
using Unity.Physics;
using Unity.Physics.Extensions;
using UnityEngine;

namespace TMG.PhysicsAddForces
{
    [UpdateInGroup(typeof(FixedStepSimulationSystemGroup))]
    public class AddForceSystem : SystemBase
    {
        protected override void OnUpdate()
        {
            var deltaTime = Time.DeltaTime;
            Entities.ForEach((Entity sphere, ref PhysicsVelocity physicsVelocity, ref PhysicsMass physicsMass,
                in MoveForceData moveForceData) =>
            {
                if (Input.GetKey(moveForceData.ForwardInputKey))
                {
                    var forceVector = (float3) Vector3.forward * moveForceData.ForceAmount * deltaTime;
                    physicsVelocity.ApplyLinearImpulse(physicsMass, forceVector);
                }
            }).Run();
        }
    }
}