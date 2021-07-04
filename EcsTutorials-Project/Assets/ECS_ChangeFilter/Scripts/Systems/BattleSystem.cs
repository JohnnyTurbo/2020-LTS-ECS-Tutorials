using Unity.Entities;
using UnityEngine;

namespace TMG.ChangeFilter
{
    public class BattleSystem : SystemBase
    {
        private BattleControlData _controlData;

        protected override void OnStartRunning()
        {
            _controlData = GetSingleton<BattleControlData>();

            #region EnemySetupRegion

            var enemy1Health = EntityManager.GetComponentData<EnemyHealthData>(_controlData.Enemy1);
            var enemy1HealthUI = EntityManager.GetComponentData<EnemyHealthUI>(_controlData.Enemy1);
            enemy1HealthUI.Slider.maxValue = enemy1Health.Max;
            enemy1Health.Value = enemy1Health.Max;
            enemy1HealthUI.Slider.value = enemy1Health.Value;
            EntityManager.SetComponentData(_controlData.Enemy1, enemy1Health);
            
            var enemy2Health = EntityManager.GetComponentData<EnemyHealthData>(_controlData.Enemy2);
            var enemy2HealthUI = EntityManager.GetComponentData<EnemyHealthUI>(_controlData.Enemy2);
            enemy2HealthUI.Slider.maxValue = enemy2Health.Max;
            enemy2Health.Value = enemy2Health.Max;
            enemy2HealthUI.Slider.value = enemy2Health.Value;
            EntityManager.SetComponentData(_controlData.Enemy2, enemy2Health);

            var enemy3Health = EntityManager.GetComponentData<EnemyHealthData>(_controlData.Enemy3);
            var enemy3HealthUI = EntityManager.GetComponentData<EnemyHealthUI>(_controlData.Enemy3);
            enemy3HealthUI.Slider.maxValue = enemy3Health.Max;
            enemy3Health.Value = enemy3Health.Max;
            enemy3HealthUI.Slider.value = enemy3Health.Value;
            EntityManager.SetComponentData(_controlData.Enemy3, enemy3Health);

            var enemy4Health = EntityManager.GetComponentData<EnemyHealthData>(_controlData.Enemy4);
            var enemy4HealthUI = EntityManager.GetComponentData<EnemyHealthUI>(_controlData.Enemy4);
            enemy4HealthUI.Slider.maxValue = enemy4Health.Max;
            enemy4Health.Value = enemy4Health.Max;
            enemy4HealthUI.Slider.value = enemy4Health.Value;
            EntityManager.SetComponentData(_controlData.Enemy4, enemy4Health);

            #endregion
        }

        protected override void OnUpdate()
        {
            var targetEntity = Entity.Null;

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

            var enemyHealth = EntityManager.GetComponentData<EnemyHealthData>(targetEntity);
            enemyHealth.Value -= _controlData.AttackPower;
            EntityManager.SetComponentData(targetEntity, enemyHealth);
        }
    }
}