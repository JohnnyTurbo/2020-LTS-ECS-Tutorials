using Unity.Entities;

namespace TMG.BlobAssets
{
    [GenerateAuthoringComponent]
    public struct PlayerExperienceData : IComponentData
    {
        public int CurrentLevel;
        public int CurrentExperience;
    }
}