using TMPro;
using Unity.Entities;
using UnityEngine;

namespace TMG.DynamicBuffers
{
    [GenerateAuthoringComponent]
    public class FishingSimUI : IComponentData
    {
        public GameObject Fish;
        public GameObject LowerFishingLine;
        public GameObject ScreenDimmer;
        public TMP_Text FishCaughtText;
        public TMP_Text FinalText;
    }
}