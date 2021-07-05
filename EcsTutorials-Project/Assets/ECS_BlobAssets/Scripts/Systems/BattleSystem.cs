using Unity.Entities;
using UnityEngine;

namespace TMG.BlobAssets
{
    [UpdateAfter(typeof(SetupBlobAssetSystem))]
    public class BattleSystem : SystemBase
    {
        private BattleControlData _controlData;
        private BlobAssetReference<LevelUpBlobAsset> _levelUpBlobAssetReference;
        private Entity _knightEntity;
        private PlayerExperienceData _playerExperienceData;
        private PlayerExperienceUI _playerExperienceUI;
        
        protected override void OnStartRunning()
        {
            #region GeneralSetupRegion

            _controlData = GetSingleton<BattleControlData>();
            _knightEntity = _controlData.GrayKnight;
            
            var playerEntity = GetSingletonEntity<PlayerExperienceData>();
            _playerExperienceData = EntityManager.GetComponentData<PlayerExperienceData>(playerEntity);
            _playerExperienceUI = EntityManager.GetComponentData<PlayerExperienceUI>(playerEntity);
            
            _playerExperienceUI.PlayerExperienceSlider.value = 0;
            _playerExperienceUI.LevelText.text = $"Level 1\n<size=95>Gray Knight</size>";
            
            #endregion

            _levelUpBlobAssetReference = GetSingleton<LevelUpBlob>().Reference;
            
            _playerExperienceUI.PlayerExperienceSlider.maxValue =
                _levelUpBlobAssetReference.Value.Array[0].ExperiencePoints;

            #region EnemySetupRegion

            var enemy1Health = EntityManager.GetComponentData<EnemyData>(_controlData.Enemy1);
            var enemy1HealthUI = EntityManager.GetComponentData<EnemyHealthUI>(_controlData.Enemy1);
            enemy1HealthUI.Slider.maxValue = enemy1Health.MaxHealth;
            enemy1Health.CurHealth = enemy1Health.MaxHealth;
            enemy1HealthUI.Slider.value = enemy1Health.CurHealth;
            EntityManager.SetComponentData(_controlData.Enemy1, enemy1Health);
            
            var enemy2Health = EntityManager.GetComponentData<EnemyData>(_controlData.Enemy2);
            var enemy2HealthUI = EntityManager.GetComponentData<EnemyHealthUI>(_controlData.Enemy2);
            enemy2HealthUI.Slider.maxValue = enemy2Health.MaxHealth;
            enemy2Health.CurHealth = enemy2Health.MaxHealth;
            enemy2HealthUI.Slider.value = enemy2Health.CurHealth;
            EntityManager.SetComponentData(_controlData.Enemy2, enemy2Health);

            var enemy3Health = EntityManager.GetComponentData<EnemyData>(_controlData.Enemy3);
            var enemy3HealthUI = EntityManager.GetComponentData<EnemyHealthUI>(_controlData.Enemy3);
            enemy3HealthUI.Slider.maxValue = enemy3Health.MaxHealth;
            enemy3Health.CurHealth = enemy3Health.MaxHealth;
            enemy3HealthUI.Slider.value = enemy3Health.CurHealth;
            EntityManager.SetComponentData(_controlData.Enemy3, enemy3Health);

            var enemy4Health = EntityManager.GetComponentData<EnemyData>(_controlData.Enemy4);
            var enemy4HealthUI = EntityManager.GetComponentData<EnemyHealthUI>(_controlData.Enemy4);
            enemy4HealthUI.Slider.maxValue = enemy4Health.MaxHealth;
            enemy4Health.CurHealth = enemy4Health.MaxHealth;
            enemy4HealthUI.Slider.value = enemy4Health.CurHealth;
            EntityManager.SetComponentData(_controlData.Enemy4, enemy4Health);

            #endregion
        }

        protected override void OnUpdate()
        {
            Entity targetEntity;

            #region InputRegion

            if (Input.GetKeyDown(KeyCode.Alpha1))
            {
                targetEntity = _controlData.Enemy1;
            }
            else if (Input.GetKeyDown(KeyCode.Alpha2))
            {
                targetEntity = _controlData.Enemy2;
            }
            else if (Input.GetKeyDown(KeyCode.Alpha3))
            {
                targetEntity = _controlData.Enemy3;
            }
            else if (Input.GetKeyDown(KeyCode.Alpha4))
            {
                targetEntity = _controlData.Enemy4;
            }
            else
            {
                return;
            }

            #endregion

            var enemyData = EntityManager.GetComponentData<EnemyData>(targetEntity);
            enemyData.CurHealth -= _controlData.AttackPower;
            
            if (enemyData.CurHealth <= 0)
            {
                enemyData.CurHealth = enemyData.MaxHealth;
                
                var expGained = enemyData.ExpValue;
                var curExp = _playerExperienceData.CurrentExperience;
                
                curExp += expGained;
                
                var curLevel = _playerExperienceData.CurrentLevel;
                var levelUpExp = _levelUpBlobAssetReference.Value.Array[curLevel].ExperiencePoints;

                if (curExp >= levelUpExp && curLevel < 2)
                {
                    // Increment level and carryover Experience
                    curLevel++;
                    curExp %= levelUpExp;
                    _playerExperienceData.CurrentLevel = curLevel;
                    
                    EntityManager.DestroyEntity(_knightEntity);
                    
                    var nextExperienceData = _levelUpBlobAssetReference.Value.Array[curLevel];
                    
                    _knightEntity = EntityManager.Instantiate(nextExperienceData.KnightPrefab);
                    var nextLevelExp = nextExperienceData.ExperiencePoints;
                    var nextLevelTitle = nextExperienceData.LevelName;
                    
                    _playerExperienceUI.PlayerExperienceSlider.maxValue = nextLevelExp;
                    _playerExperienceUI.LevelText.text = $"Level: {curLevel + 1}\n<size=95>{nextLevelTitle}</size>";
                }

                _playerExperienceUI.PlayerExperienceSlider.value = curExp;
                _playerExperienceData.CurrentExperience = curExp;
                SetSingleton(_playerExperienceData);
            }
            EntityManager.SetComponentData(targetEntity, enemyData);
        }
    }
}