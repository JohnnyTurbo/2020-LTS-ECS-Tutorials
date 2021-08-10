using Unity.Entities;
using UnityEngine;

namespace TMG.ECS_ReadRelationships
{
    [GenerateAuthoringComponent]
    public struct LeaderMoveData : IComponentData
    {
        public KeyCode ForwardKey;
        public KeyCode LeftKey;
        public KeyCode RightKey;
        public KeyCode BackKey;
    }
}