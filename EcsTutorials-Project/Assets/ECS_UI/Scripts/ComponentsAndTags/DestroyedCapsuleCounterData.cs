using Unity.Entities;

namespace TMG.ECS_UI
{
    [GenerateAuthoringComponent]
    public struct DestroyedCapsuleCounterData : IComponentData
    {
        public int Value;
    }
}