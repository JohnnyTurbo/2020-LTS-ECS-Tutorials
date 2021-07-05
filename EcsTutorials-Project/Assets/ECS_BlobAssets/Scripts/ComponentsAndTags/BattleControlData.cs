using Unity.Entities;

namespace TMG.BlobAssets
{
    [GenerateAuthoringComponent]
    public struct BattleControlData : IComponentData
    {
        public Entity GrayKnight;
        public Entity BlackKnightPrefab;
        public Entity RedKnightPrefab;
        
        public Entity Enemy1;
        public Entity Enemy2;
        public Entity Enemy3;
        public Entity Enemy4;
        
        public int AttackPower;
    }
}