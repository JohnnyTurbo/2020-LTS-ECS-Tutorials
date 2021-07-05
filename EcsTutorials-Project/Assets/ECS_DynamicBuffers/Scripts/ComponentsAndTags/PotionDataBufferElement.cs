using Unity.Entities;

namespace TMG.DynamicBuffers
{
    public enum PotionType { Health, Mana, Strength }
    
    public struct PotionDataBufferElement : IBufferElementData
    {
        public PotionType Type;
        public int Value;

        public static implicit operator PotionType(PotionDataBufferElement element)
        {
            return element.Type;
        }

        public static implicit operator int(PotionDataBufferElement element)
        {
            return element.Value;
        }
    }
}