using Unity.Entities;

namespace TMG.ECS_Singletons
{
    [GenerateAuthoringComponent]
    public struct TimeToLiveData : IComponentData
    {
        public float Value;
    }
}