using Unity.Entities;
using Unity.Transforms;

namespace TMG.ECS_Transforms
{
    public class MoveBattleStage : SystemBase
    {
        protected override void OnUpdate()
        {
            Entities.ForEach((ref Translation translation, ref Rotation rotation, in BattleStageManagedData battleStageManagedData) =>
            {
                translation.Value = battleStageManagedData.BattleStageFollower.transform.position;
                rotation.Value = battleStageManagedData.BattleStageFollower.transform.rotation;
            }).WithoutBurst().Run();
        }
    }
}