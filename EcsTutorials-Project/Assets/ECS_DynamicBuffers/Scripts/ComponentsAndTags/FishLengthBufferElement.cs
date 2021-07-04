using Unity.Entities;

namespace TMG.DynamicBuffers
{
    [GenerateAuthoringComponent]
    public struct FishLengthBufferElement : IBufferElementData
    {
        public int Value;
        
        public static implicit operator int(FishLengthBufferElement element)
        {
            return element.Value;
        }

        public static implicit operator FishLengthBufferElement(int value)
        {
            return new FishLengthBufferElement {Value = value};
        }
    }
}