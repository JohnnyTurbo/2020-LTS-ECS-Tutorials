using Unity.Entities;

namespace TMG.ECS_Transforms
{
    [GenerateAuthoringComponent]
    public struct RotationData : IComponentData
    {
        public float Magnitude;
        public float Frequency;
    }
}