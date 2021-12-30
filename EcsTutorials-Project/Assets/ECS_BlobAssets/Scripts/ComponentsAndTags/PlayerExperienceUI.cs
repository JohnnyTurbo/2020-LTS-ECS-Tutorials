using TMPro;
using Unity.Entities;
using UnityEngine.UI;

namespace TMG.BlobAssets
{
    [GenerateAuthoringComponent]
    public class PlayerExperienceUI : IComponentData
    {
        public Slider PlayerExperienceSlider;
        public TMP_Text LevelText;
    }
}