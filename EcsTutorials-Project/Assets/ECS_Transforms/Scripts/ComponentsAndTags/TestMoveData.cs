using Unity.Entities;
using UnityEngine;

namespace TMG.ECS_Transforms
{
    [GenerateAuthoringComponent]
    public struct TestMoveData : IComponentData
    {
        public Entity EntityToMove;
        public float MoveSpeed;
        public KeyCode MoveKey;

        public float RoationSpeed;
        public KeyCode RotationKey;
    }
}