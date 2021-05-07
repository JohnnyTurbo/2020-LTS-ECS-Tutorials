using TMPro;
using Unity.Entities;

namespace TMG.ECS_UI
{
    [GenerateAuthoringComponent]
    public class DestroyedCapsuleUIData : IComponentData
    {
        public TMP_Text CounterText;
    }
}