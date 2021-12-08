using Unity.Entities;

namespace TMG.UnitSelection
{
    [GenerateAuthoringComponent]
    public struct SelectionUIData : IComponentData
    {
        public Entity SelectionUIPrefab;
    }
}