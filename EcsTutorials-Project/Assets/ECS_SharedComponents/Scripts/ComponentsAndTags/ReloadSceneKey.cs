using Unity.Entities;
using UnityEngine;

namespace TMG.ConnectFour
{
    [GenerateAuthoringComponent]
    public struct ReloadSceneKey : IComponentData
    {
        public KeyCode Value;
    }
}