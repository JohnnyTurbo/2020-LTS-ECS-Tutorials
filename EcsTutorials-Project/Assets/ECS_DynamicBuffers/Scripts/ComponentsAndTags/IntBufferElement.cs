using Unity.Entities;

namespace TMG.DynamicBuffers
{
    [GenerateAuthoringComponent]
    public struct IntBufferElement : IBufferElementData
    {
        public int Value;

        public static implicit operator int(IntBufferElement element)
        {
            return element.Value;
        }

        public static implicit operator IntBufferElement(int value)
        {
            return new IntBufferElement {Value = value};
        }
    }
}