using TMG.ECS_ReadRelationships;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;
using UnityEngine;

namespace ECS_ReadRelationships.Scripts.Systems
{
    public class LeaderMoveSystem : SystemBase
    {
        protected override void OnUpdate()
        {
            var deltaTime = Time.DeltaTime;
            
            Entities.ForEach((Entity e, ref Translation translation, in LeaderMoveData leaderMoveData, in MovementData movementData) =>
            {
                var moveSpeed = movementData.MoveSpeed * deltaTime; 
                if (Input.GetKey(leaderMoveData.ForwardKey))
                {
                    translation.Value += new float3(0, 0, moveSpeed);
                }
                if (Input.GetKey(leaderMoveData.LeftKey))
                {
                    translation.Value += new float3(-moveSpeed, 0, 0);
                }
                if (Input.GetKey(leaderMoveData.RightKey))
                {
                    translation.Value += new float3(moveSpeed, 0, 0);
                }
                if (Input.GetKey(leaderMoveData.BackKey))
                {
                    translation.Value += new float3(0, 0, -moveSpeed);
                }
            }).Run();
        }
    }
}