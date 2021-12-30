using Unity.Entities;
using Unity.Transforms;
using UnityEngine;

namespace TMG.EntityQueries
{
    [DisableAutoCreation]
    public class SoldierMarchForEachSystem : SystemBase
    {
        private EntityQuery _soldierQuery;

        protected override void OnStartRunning()
        {
            Debug.Log($"There are {_soldierQuery.CalculateEntityCount()} soldiers in the parade!");
        }

        protected override void OnUpdate()
        {
            var deltaTime = Time.DeltaTime;
            
            Entities
                .WithStoreEntityQueryInField(ref _soldierQuery)
                .ForEach((Entity e, ref Translation position, in SoldierMoveData soldierMoveData) =>
            {
                position.Value.x += soldierMoveData.Value * deltaTime;
            }).ScheduleParallel();
        }
    }
}