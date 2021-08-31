using Unity.Entities;
using Unity.Mathematics;
using UnityEngine;
using Random = Unity.Mathematics.Random;

namespace TMG.ECS_GetComponentFromEntity
{
    [GenerateAuthoringComponent]
    public struct LeaderMoveData : IComponentData
    {
        public float3x2 MovementBounds;
        public float3 CurrentDestination;
        public Random Random;
        
        public void SetRandomDestination()
        {
            CurrentDestination = Random.NextFloat3(MovementBounds[0], MovementBounds[1]);
        }
    }
}