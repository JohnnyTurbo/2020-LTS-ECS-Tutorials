using Unity.Entities;
using Random = Unity.Mathematics.Random;

namespace TMG.ECS_Random
{
    [GenerateAuthoringComponent]
    public struct CentralRandomData : IComponentData
    {
        public Random Value;
    }
}