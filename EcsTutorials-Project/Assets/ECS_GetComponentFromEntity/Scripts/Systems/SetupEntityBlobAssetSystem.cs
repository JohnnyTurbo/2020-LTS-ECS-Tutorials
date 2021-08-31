using Unity.Collections;
using Unity.Entities;

namespace TMG.ECS_GetComponentFromEntity
{
    public class SetupEntityBlobAssetSystem : SystemBase
    {
        protected override void OnStartRunning()
        {
            var gameControllerEntity = GetSingletonEntity<GameControllerTag>();
            var blobAssetManagedData = EntityManager.GetComponentData<BlobAssetManagedData>(gameControllerEntity);

            using var blobBuilder = new BlobBuilder(Allocator.Temp);
            ref var leaderEntityBlobAsset = ref blobBuilder.ConstructRoot<LeaderEntityBlobAsset>();
            var leaderEntityArray =
                blobBuilder.Allocate(ref leaderEntityBlobAsset.Array, blobAssetManagedData.Leaders.Length);
            for (var i = 0; i < blobAssetManagedData.Leaders.Length; i++)
            {
                var curLeader = blobAssetManagedData.Leaders[i];
                leaderEntityArray[i] = new LeaderEntity
                {
                    Value = curLeader
                };
            }

            var followerData = GetSingleton<FollowerMoveData>();
            followerData.LeaderReference =
                blobBuilder.CreateBlobAssetReference<LeaderEntityBlobAsset>(Allocator.Persistent);
            SetSingleton(followerData);
        }

        protected override void OnUpdate()
        {
            
        }
    }
}