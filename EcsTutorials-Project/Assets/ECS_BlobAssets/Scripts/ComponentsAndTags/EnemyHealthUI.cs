using Unity.Entities;
using UnityEngine.UI;

namespace TMG.BlobAssets
{
    [GenerateAuthoringComponent]
    public class EnemyHealthUI : IComponentData
    {
        public Slider Slider;
    }
}