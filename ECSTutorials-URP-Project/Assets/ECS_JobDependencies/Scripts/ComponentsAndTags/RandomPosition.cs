using Unity.Entities;
using Unity.Mathematics;

namespace TMG.JobDependencies
{
    [GenerateAuthoringComponent]
    public struct RandomPosition : IComponentData
    {
        public Random Value;
        
        public float3 MinimumPosition;
        public float3 MaximumPosition;
        public float3 NextPosition => Value.NextFloat3(MinimumPosition, MaximumPosition);
    }
}