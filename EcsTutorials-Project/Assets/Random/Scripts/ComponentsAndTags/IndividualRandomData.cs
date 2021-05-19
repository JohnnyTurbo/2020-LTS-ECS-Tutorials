using Unity.Entities;
using Random = Unity.Mathematics.Random;

namespace TMG.ECS_Random
{
    [GenerateAuthoringComponent]
    public struct IndividualRandomData : IComponentData
    {
        public Random Value;
    }
}