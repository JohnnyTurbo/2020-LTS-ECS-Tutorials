using Unity.Entities;

namespace TMG.JobDependencies
{
    [GenerateAuthoringComponent]
    public struct PlayerInputData : IComponentData
    {
        public float MovementThisFrame;
        public float RotationThisFrame;
    }
}