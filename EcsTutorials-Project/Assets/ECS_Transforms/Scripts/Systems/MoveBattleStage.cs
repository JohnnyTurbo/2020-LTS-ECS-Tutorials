using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;

namespace TMG.ECS_Transforms
{
    public class MoveBattleStage : SystemBase
    {
        protected override void OnUpdate()
        {
            Entities.ForEach((ref LocalToWorld localToWorld, in BattleStageManagedData battleStageManagedData) =>
            {
                var position = battleStageManagedData.TrackFollower.position;
                var rotation = battleStageManagedData.TrackFollower.rotation;

                localToWorld.Value = new float4x4(rotation, position);
            }).WithoutBurst().Run();
        }
    }
}