using Unity.Entities;
using Unity.Mathematics;

namespace TMG.DynamicBuffers
{
    [GenerateAuthoringComponent]
    public struct FishingSimData : IComponentData
    {
        public bool CaughtFish;
        public bool CanFish;
        public Random Random;
        public int MinFishLength;
        public int MaxFishLength;
        public int NextFishLength => Random.NextInt(MinFishLength, MaxFishLength);
    }
}