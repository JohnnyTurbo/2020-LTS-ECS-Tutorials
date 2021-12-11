using Unity.Entities;

namespace TMG.UnitSelection
{
    [GenerateAuthoringComponent]
    public struct SelectionUIPrefab : IComponentData
    {
        public Entity Value;
    }
}