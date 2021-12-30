using Unity.Collections;
using Unity.Entities;

namespace TMG.BlobAssets
{
    public struct LevelUpData
    {
        public int ExperiencePoints;
        public FixedString32 LevelName;
        public Entity KnightPrefab;
    }
}