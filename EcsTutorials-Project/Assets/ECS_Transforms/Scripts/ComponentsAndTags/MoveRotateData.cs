using Unity.Entities;
using Unity.Mathematics;

namespace TMG.ECS_Transforms
{
    [GenerateAuthoringComponent]
    public struct MoveRotateData : IComponentData
    {
        public float Magnitude;
        public float Frequency;
        public float3 OriginPosition;
    }
}