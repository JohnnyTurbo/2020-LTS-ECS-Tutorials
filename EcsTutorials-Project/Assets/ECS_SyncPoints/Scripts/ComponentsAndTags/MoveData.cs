using Unity.Entities;

namespace TMG.SyncPoints
{
    [GenerateAuthoringComponent]
    public struct MoveData : IComponentData
    {
        public float MoveSpeed;
    }
}