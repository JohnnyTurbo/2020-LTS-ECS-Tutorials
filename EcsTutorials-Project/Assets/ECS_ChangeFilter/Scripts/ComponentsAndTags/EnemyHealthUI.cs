using Unity.Entities;
using UnityEngine.UI;

namespace TMG.ChangeFilter
{
    [GenerateAuthoringComponent]
    public class EnemyHealthUI : IComponentData
    {
        public Slider Slider;
    }
}