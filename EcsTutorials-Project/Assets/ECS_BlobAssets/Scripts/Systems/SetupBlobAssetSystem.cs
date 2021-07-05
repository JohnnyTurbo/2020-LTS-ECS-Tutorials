using Unity.Collections;
using Unity.Entities;

namespace TMG.BlobAssets
{
    public class SetupBlobAssetSystem : SystemBase
    {
        private BattleControlData _controlData;
        //private LevelUpBlob _levelUpBlob;
        //private BlobAssetReference<LevelUpBlobAsset> _levelUpBlobAssetReference;
        
        protected override void OnStartRunning()
        {
            var gameControllerEntity = GetSingletonEntity<BattleControlData>();
            _controlData = EntityManager.GetComponentData<BattleControlData>(gameControllerEntity);

            using var blobBuilder = new BlobBuilder(Allocator.Temp);
            ref var experienceBlobAsset = ref blobBuilder.ConstructRoot<LevelUpBlobAsset>();
            var experienceArray = blobBuilder.Allocate(ref experienceBlobAsset.Array, 3);
            
            experienceArray[0] = new LevelUpData
            {
                ExperiencePoints = 50, 
                LevelName = "Gray Knight"
            };
            experienceArray[1] = new LevelUpData
            {
                ExperiencePoints = 65, 
                LevelName = "Black Knight", 
                KnightPrefab = _controlData.BlackKnightPrefab
                
            };
            experienceArray[2] = new LevelUpData
            {
                ExperiencePoints = 80, 
                LevelName = "Red Knight", 
                KnightPrefab = _controlData.RedKnightPrefab
            };

            //_levelUpBlobAssetReference = blobBuilder.CreateBlobAssetReference<LevelUpBlobAsset>(Allocator.Persistent);
            
        }

        protected override void OnUpdate()
        {
            
        }
    }
}