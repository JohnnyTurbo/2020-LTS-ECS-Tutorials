using Unity.Entities;

namespace TMG.SyncPoints
{
    [GenerateAuthoringComponent]
    public struct CubeMoveData : IComponentData
    {
        public float RotationSpeed;
        public float MoveSpeed;
    }
}