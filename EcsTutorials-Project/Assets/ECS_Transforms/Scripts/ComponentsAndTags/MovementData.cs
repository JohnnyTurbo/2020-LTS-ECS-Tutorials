using Unity.Entities;
using Unity.Mathematics;

namespace TMG.ECS_Transforms
{
    [GenerateAuthoringComponent]
    public struct MovementData : IComponentData
    {
        public float Magnitude;
        public float Frequency;
        public float3 OriginPosition;
    }
}