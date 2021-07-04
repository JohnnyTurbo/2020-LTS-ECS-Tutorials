using Unity.Entities;

namespace TMG.ChangeFilter
{
    [GenerateAuthoringComponent]
    public struct BattleControlData : IComponentData
    {
        public Entity Enemy1;
        public Entity Enemy2;
        public Entity Enemy3;
        public Entity Enemy4;
        public int AttackPower;
    }
}