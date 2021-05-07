using Unity.Entities;
using UnityEngine.UI;

namespace TMG.ECS_UI
{
    [GenerateAuthoringComponent]
    public struct TimeToLiveData : IComponentData
    {
        public float InitialValue;
        public float Value;
    }
}