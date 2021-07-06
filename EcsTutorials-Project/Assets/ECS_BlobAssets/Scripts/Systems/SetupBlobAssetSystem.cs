using Unity.Collections;
using Unity.Entities;

namespace TMG.BlobAssets
{
    public class SetupBlobAssetSystem : SystemBase
    {
        private BattleControlData _controlData;

        protected override void OnStartRunning()
        {
            var gameControllerEntity = GetSingletonEntity<BattleControlData>();
            _controlData = EntityManager.GetComponentData<BattleControlData>(gameControllerEntity);

            using var blobBuilder = new BlobBuilder(Allocator.Temp);
            ref var levelUpBlobAsset = ref blobBuilder.ConstructRoot<LevelUpBlobAsset>();
            var levelUpArray = blobBuilder.Allocate(ref levelUpBlobAsset.Array, 3);

            levelUpArray[0] = new LevelUpData
            {
                ExperiencePoints = 50,
                LevelName = "Gray Knight"
            };
            levelUpArray[1] = new LevelUpData
            {
                ExperiencePoints = 65,
                LevelName = "Black Knight",
                KnightPrefab = _controlData.BlackKnightPrefab
            };
            levelUpArray[2] = new LevelUpData
            {
                ExperiencePoints = 80,
                LevelName = "Red Knight",
                KnightPrefab = _controlData.RedKnightPrefab
            };

            var playerData = GetSingleton<PlayerData>();
            playerData.LevelUpReference = blobBuilder.CreateBlobAssetReference<LevelUpBlobAsset>(Allocator.Persistent);
            SetSingleton(playerData);

        }

        protected override void OnUpdate()
        {
            
        }
    }
}






