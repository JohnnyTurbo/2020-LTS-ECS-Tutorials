using Unity.Entities;
using Unity.Mathematics;
using UnityEngine.UI;

namespace TMG.ECS_UI
{
    [GenerateAuthoringComponent]
    public class HealthBarUIData : IComponentData
    {
        public Slider Slider;
        public float3 Offset;
    }
}