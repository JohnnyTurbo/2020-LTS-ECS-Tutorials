using Unity.Entities;
using Unity.Rendering;
using UnityEngine;

namespace TMG.ECS_GetComponentFromEntity
{
    public class FollowerInputSystem : SystemBase
    {
        protected override void OnUpdate()
        {
            var followerEntity = GetSingletonEntity<FollowerMoveData>();
            var followerMoveData = EntityManager.GetComponentData<FollowerMoveData>(followerEntity);
            var followerRenderMesh = EntityManager.GetSharedComponentData<RenderMesh>(followerEntity);

            #region InputRegion

            var entityIndex = -1;

            if (Input.GetKeyDown(KeyCode.Alpha1))
            {
                entityIndex = 0;
            }
            else if (Input.GetKeyDown(KeyCode.Alpha2))
            {
                entityIndex = 1;
            }
            else if (Input.GetKeyDown(KeyCode.Alpha3))
            {
                entityIndex = 2;
            }
            else if (Input.GetKeyDown(KeyCode.Alpha4))
            {
                entityIndex = 3;
            }
            else if (Input.GetKeyDown(KeyCode.Alpha5))
            {
                entityIndex = 4;
            }

            if (entityIndex < 0)
            {
                return;
            }

            #endregion

            var leaderEntity = followerMoveData.LeaderReference.Value.Array[entityIndex].Value;
            followerMoveData.CurrentLeaderEntity = leaderEntity;

            var leaderMaterial = EntityManager.GetSharedComponentData<RenderMesh>(leaderEntity).material;
            followerRenderMesh.material = leaderMaterial;
            
            EntityManager.SetComponentData(followerEntity, followerMoveData);
            EntityManager.SetSharedComponentData(followerEntity, followerRenderMesh);
        }
    }
}