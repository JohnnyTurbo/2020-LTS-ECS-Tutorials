using Unity.Entities;

namespace TMG.UnitSelection_Master
{
    [GenerateAuthoringComponent]
    public struct SelectionUIPrefab : IComponentData
    {
        public Entity Value;
    }
}