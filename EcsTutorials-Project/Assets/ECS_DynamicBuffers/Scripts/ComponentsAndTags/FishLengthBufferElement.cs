using Unity.Entities;

namespace TMG.DynamicBuffers
{
    [InternalBufferCapacity(8)]
    [GenerateAuthoringComponent]
    public struct FishLengthBufferElement : IBufferElementData
    {
        public int Value;

        public static implicit operator FishLengthBufferElement(int value)
        {
            return new FishLengthBufferElement {Value = value};
        }

        public static implicit operator int(FishLengthBufferElement element)
        {
            return element.Value;
        }
    }
}











