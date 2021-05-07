using Unity.Entities;
using Unity.Transforms;

namespace TMG.ECS_UI
{
    [UpdateInGroup(typeof(LateSimulationSystemGroup))]
    public class UpdateSliderUISystem : SystemBase
    {
        protected override void OnUpdate()
        {
            Entities.ForEach((HealthBarUIData healthBarUIData, in Translation translation,
                in TimeToLiveData timeToLiveData) =>
            {
                healthBarUIData.Slider.transform.position = translation.Value + healthBarUIData.Offset;
                healthBarUIData.Slider.value = (timeToLiveData.Value / timeToLiveData.InitialValue);
            }).WithoutBurst().Run();
        }
    }
}