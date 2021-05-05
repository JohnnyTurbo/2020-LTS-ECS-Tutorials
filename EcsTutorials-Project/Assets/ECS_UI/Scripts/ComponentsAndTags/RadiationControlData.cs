using Unity.Entities;
using UnityEngine;

namespace TMG.ECS_UI
{
    [GenerateAuthoringComponent]
    public struct RadiationControlData : IComponentData
    {
        public KeyCode SpawnButton;
        public KeyCode DestroyButton;
        public Entity RadiationPrefab;
    }
}