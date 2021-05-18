using Unity.Mathematics;
using Unity.Entities;

namespace TMG.ECS_Random
{
    [GenerateAuthoringComponent]
    public struct IndividualRandomData : IComponentData
    {
        public Random Value;
        public float3 MinimumPosition;
        public float3 MaximumPosition;
        public float3 NextPosition => Value.NextFloat3(MinimumPosition, MaximumPosition);
    }
}