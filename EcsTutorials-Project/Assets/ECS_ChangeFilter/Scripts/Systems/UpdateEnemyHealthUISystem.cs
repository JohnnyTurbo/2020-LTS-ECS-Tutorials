using Unity.Entities;
using Unity.Transforms;
using UnityEngine;

namespace TMG.ChangeFilter
{
    public class UpdateEnemyHealthUISystem : SystemBase
    {
        protected override void OnUpdate()
        {
            Entities
                .WithChangeFilter<EnemyHealthData, Translation>()
                .ForEach((Entity e, EnemyHealthUI ui, in EnemyHealthData healthData) =>
            {
                Debug.Log($"Updating Health UI on: {e.Index}");
                ui.Slider.value = healthData.Value;
            }).WithoutBurst().Run();
        }
    }
}