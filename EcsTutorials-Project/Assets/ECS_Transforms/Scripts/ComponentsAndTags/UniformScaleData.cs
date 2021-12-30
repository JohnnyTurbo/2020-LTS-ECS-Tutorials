using Unity.Entities;

namespace TMG.ECS_Transforms
{
    [GenerateAuthoringComponent]
    public struct UniformScaleData : IComponentData
    {
        public float Magnitude;
        public float Frequency;
    }
}