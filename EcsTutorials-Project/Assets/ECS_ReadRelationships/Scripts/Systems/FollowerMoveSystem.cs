using TMG.ECS_ReadRelationships;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;
using UnityEngine;

namespace ECS_ReadRelationships.Scripts.Systems
{
    public class FollowerMoveSystem : SystemBase
    {
        protected override void OnUpdate()
        {
            var leaderTransforms = GetComponentDataFromEntity<Translation>(true);
            var deltaTime = Time.DeltaTime;
            Entities.ForEach((Entity e, ref Translation translation, ref Rotation rotation, in FollowerMoveData followerMoveData, in MovementData movementData) =>
            {
                var otherTransform = leaderTransforms[followerMoveData.EntityToFollow].Value;
                rotation.Value = quaternion.LookRotation(translation.Value - otherTransform, new float3(0, 1, 0));
                var toLeader = otherTransform - translation.Value;
                if(math.length(toLeader) <= followerMoveData.MinFollowDistance){return;}
                toLeader = math.normalize(toLeader);
                var moveSpeed = movementData.MoveSpeed * deltaTime;
                toLeader *= moveSpeed;
                translation.Value += toLeader;
                //translation.Value = leaderTransforms[followerMoveData.EntityToFollow].Value;
            }).Run();
        }
    }
}