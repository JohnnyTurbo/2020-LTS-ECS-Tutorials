using Unity.Entities;

namespace TMG.ConnectFour
{
    [GenerateAuthoringComponent]
    public struct VerticalPosition : IComponentData
    {
        public int Value;
    }
}