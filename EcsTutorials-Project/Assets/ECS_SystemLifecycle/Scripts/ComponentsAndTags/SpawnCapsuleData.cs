using Unity.Entities;

namespace TMG.SystemLifecycle
{
    [GenerateAuthoringComponent]
    public struct SpawnCapsuleData : IComponentData
    {
        public Entity CapsulePrefab;
    }
}