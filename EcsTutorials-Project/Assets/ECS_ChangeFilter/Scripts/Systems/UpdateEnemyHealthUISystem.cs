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
                    ui.Slider.value = healthData.Value;
                    Debug.Log($"Updating Health UI on: {e.Index}");
                }).WithoutBurst().Run();
        }
    }
}

















