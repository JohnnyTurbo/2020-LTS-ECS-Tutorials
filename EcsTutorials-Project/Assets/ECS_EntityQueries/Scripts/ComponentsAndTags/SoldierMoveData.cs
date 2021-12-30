using Unity.Entities;

namespace TMG.EntityQueries 
{
    [GenerateAuthoringComponent]
    public struct SoldierMoveData : IComponentData
    {
        public float Value;
    }
}