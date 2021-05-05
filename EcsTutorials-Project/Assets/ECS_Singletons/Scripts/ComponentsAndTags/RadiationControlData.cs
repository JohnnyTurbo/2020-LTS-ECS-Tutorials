using Unity.Entities;
using UnityEngine;

namespace TMG.ECS_Singletons
{
    [GenerateAuthoringComponent]
    public struct RadiationControlData : IComponentData
    {
        public KeyCode SpawnButton;
        public KeyCode DestroyButton;
        public Entity RadiationPrefab;
    }
}