using Unity.Entities;

namespace TMG.ECS_UI
{
    [GenerateAuthoringComponent]
    public struct TimeToLiveData : IComponentData
    {
        public float InitialValue;
        public float Value;
    }
}