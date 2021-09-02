using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;

namespace TMG.ECS_GetComponentFromEntity
{
    public class FollowerMoveSystem : SystemBase
    {
        protected override void OnUpdate()
        {
            var allTranslations = GetComponentDataFromEntity<Translation>(true);
            
            var deltaTime = Time.DeltaTime;
            Entities.ForEach((Entity e, ref Translation translation, ref Rotation rotation, in FollowerMoveData followerMoveData, in MovementData movementData) =>
            {
                if(!allTranslations.HasComponent(followerMoveData.CurrentLeaderEntity)){return;}

                var targetPosition = allTranslations[followerMoveData.CurrentLeaderEntity].Value;
                
                #region MoveAndRotateRegion

                rotation.Value = quaternion.LookRotation(translation.Value - targetPosition, new float3(0, 1, 0));
                
                var toTarget = targetPosition - translation.Value;
                
                if(math.length(toTarget) <= followerMoveData.MinFollowDistance){return;}
                
                var moveStep = math.normalize(toTarget) * movementData.MoveSpeed * deltaTime;
                translation.Value += moveStep;

                #endregion
                
            }).Run();
        }
    }
}