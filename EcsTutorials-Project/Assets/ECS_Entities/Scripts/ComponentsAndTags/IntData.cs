using Unity.Entities;

namespace TMG.ECS_Entities
{
    [GenerateAuthoringComponent]
    public struct IntData : IComponentData
    {
        public int Value;
    }
    
    public struct IntData2 : IComponentData
    {
        public int Value;
    }

    public struct IntData3 : IComponentData
    {
        public bool Value1;
        public bool Value2;
    }
}