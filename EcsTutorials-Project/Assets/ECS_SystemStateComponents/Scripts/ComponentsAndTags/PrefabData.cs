using Unity.Entities;

namespace TMG.SystemStateComponents
{
    [GenerateAuthoringComponent]
    public struct PrefabData : IComponentData
    {
        public Entity CapsulePrefab;
        public Entity ShadowPrefab;
    }
}