using Unity.Entities;
using UnityEngine;

namespace TMG.PhysicsAddForces
{
    [GenerateAuthoringComponent]
    public struct MoveForceData : IComponentData
    {
        public float ForceAmount;
        public KeyCode ForwardInputKey;
    }
}