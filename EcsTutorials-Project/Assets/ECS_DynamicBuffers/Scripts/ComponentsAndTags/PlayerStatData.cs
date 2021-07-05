using Unity.Entities;

namespace TMG.DynamicBuffers
{
    [GenerateAuthoringComponent]
    public struct PlayerStatData : IComponentData
    {
        public int Health;
        public int MaxHealth;
        public int Mana;
        public int MaxMana;
        public int Strength;
        public int MaxStrength;
    }
}