using Unity.Entities;

namespace TMG.ECS_Transforms
{
    [GenerateAuthoringComponent]
    public struct NonUniformScaleData : IComponentData
    {
        public float Magnitude;
        public float Frequency;
    }
}