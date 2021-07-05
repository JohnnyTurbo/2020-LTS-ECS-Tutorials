using Unity.Entities;
using UnityEngine;

namespace TMG.BlobAssets
{
    public class UpdateEnemyHealthUISystem : SystemBase
    {
        protected override void OnUpdate()
        {
            Entities
                .WithChangeFilter<EnemyData>()
                .ForEach((Entity e, EnemyHealthUI ui, ref EnemyData healthData) =>
                {
                    ui.Slider.value = healthData.CurHealth;
                }).WithoutBurst().Run();
        }
    }
}