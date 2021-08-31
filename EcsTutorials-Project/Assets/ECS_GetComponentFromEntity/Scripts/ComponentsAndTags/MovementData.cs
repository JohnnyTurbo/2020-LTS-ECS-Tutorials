using Unity.Entities;
using UnityEngine;

namespace TMG.ECS_GetComponentFromEntity
{
    [GenerateAuthoringComponent]
    public struct MovementData : IComponentData
    {
        public float MoveSpeed;
    }
}