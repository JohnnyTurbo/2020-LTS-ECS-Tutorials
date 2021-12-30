using Unity.Entities;

namespace TMG.ChangeFilter
{
    [GenerateAuthoringComponent]
    public struct EnemyHealthData : IComponentData
    {
        public int Max;
        public int Value;
    }
}