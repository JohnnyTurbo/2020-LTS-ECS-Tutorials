using Unity.Entities;
using UnityEngine;

namespace TMG.ECS_Transforms
{
    [GenerateAuthoringComponent]
    public class BattleStageManagedData : IComponentData
    {
        public Transform BattleStageFollower;
    }
}