using Unity.Entities;

namespace TMG.BlobAssets
{
    public struct Number : IComponentData
    {
        public int Value;
    }

    public struct NumberBlobAsset
    {
        public BlobArray<Number> NumberArray;
    }
}