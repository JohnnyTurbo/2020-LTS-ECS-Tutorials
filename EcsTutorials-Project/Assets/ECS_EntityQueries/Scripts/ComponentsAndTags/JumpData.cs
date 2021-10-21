using Unity.Entities;

namespace TMG.EntityQueries
{
    [GenerateAuthoringComponent]
    public struct JumpData : IComponentData
    {
        public float Value;
    }
}