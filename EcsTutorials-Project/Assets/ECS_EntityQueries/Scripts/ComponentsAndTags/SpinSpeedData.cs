using Unity.Entities;

namespace TMG.EntityQueries
{
    [GenerateAuthoringComponent]
    public struct SpinSpeedData : IComponentData
    {
        public float Value;
    }
}