using Unity.Entities;

namespace TMG.BatchedJobs
{
    [GenerateAuthoringComponent]
    public struct MoveSpeedData : IComponentData
    {
        public float Value;
    }
}