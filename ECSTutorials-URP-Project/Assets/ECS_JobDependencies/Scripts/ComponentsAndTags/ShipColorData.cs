using Unity.Entities;
using Unity.Mathematics;
using Unity.Rendering;

namespace TMG.JobDependencies
{
    [GenerateAuthoringComponent]
    [MaterialProperty("_Color", MaterialPropertyFormat.Float4)]
    public struct ShipColorData : IComponentData
    {
        public float4 Value;
    }
}