using Unity.Entities;
using UnityEngine;

namespace TMG.ECS_ReadRelationships
{
    [GenerateAuthoringComponent]
    public struct MovementData : IComponentData
    {
        public float MoveSpeed;
    }
}