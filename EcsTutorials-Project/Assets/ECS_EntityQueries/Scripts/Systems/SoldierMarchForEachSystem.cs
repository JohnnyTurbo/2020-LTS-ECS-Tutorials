using Unity.Entities;
using Unity.Transforms;

namespace TMG.EntityQueries
{
    [DisableAutoCreation]
    public class SoldierMarchForEachSystem : SystemBase
    {
        protected override void OnUpdate()
        {
            var deltaTime = Time.DeltaTime;
            
            Entities.ForEach((ref Translation position, in SoldierMoveData soldierMoveData) =>
            {
                position.Value.x += soldierMoveData.Value * deltaTime;
            }).ScheduleParallel();
        }
    }
}