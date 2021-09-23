using Unity.Entities;

namespace TMG.BatchedJobs
{
    [GenerateAuthoringComponent]
    public struct OscillateMagnitudeData : IComponentData
    {
        public float Value;
    }
}