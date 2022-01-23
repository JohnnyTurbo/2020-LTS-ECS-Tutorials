using Unity.Entities;
using UnityEngine;

namespace TMG.JobDependencies
{
    [GenerateAuthoringComponent]
    public struct PlayerControlData : IComponentData
    {
        public KeyCode ForwardKey;
        public KeyCode LeftRotationKey;
        public KeyCode RightRotationKey;
    }
}