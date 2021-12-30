using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;

namespace TMG.ECS_GetComponentFromEntity
{
    public class LeaderMoveSystem : SystemBase
    {
        protected override void OnStartRunning()
        {
            Entities.ForEach((ref LeaderMoveData leaderMoveData) =>
            {
                leaderMoveData.SetRandomDestination();
            }).Run();
        }

        protected override void OnUpdate()
        {
            var deltaTime = Time.DeltaTime;
            
            Entities.ForEach((Entity e, ref Translation translation, ref Rotation rotation, ref LeaderMoveData leaderMoveData, in MovementData movementData) =>
            {
                var currentDestination = leaderMoveData.CurrentDestination;
                
                rotation.Value = quaternion.LookRotation(translation.Value - currentDestination, new float3(0, 1, 0));
                
                var toDestination = currentDestination - translation.Value;
                
                if (math.length(toDestination) <= 0.01f)
                {
                    leaderMoveData.SetRandomDestination();
                }

                var moveStep = math.normalize(toDestination) * movementData.MoveSpeed * deltaTime;
                translation.Value += moveStep;
                
            }).Run();
        }
    }
}