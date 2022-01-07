using Unity.Entities;

namespace ECS_SyncPoints.Scripts.AuthoringAndMono
{
    [GenerateAuthoringComponent]
    public struct CubeMoveData : IComponentData
    {
        public float RotationSpeed;
        public float MoveSpeed;
    }
}