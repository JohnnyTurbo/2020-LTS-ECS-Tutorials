using Unity.Burst;
using Unity.Collections;
using Unity.Entities;
using Unity.Transforms;

namespace TMG.EntityQueries
{
    //[DisableAutoCreation]
    public class SoldierMarchJobSystem : SystemBase
    {
        protected override void OnUpdate()
        {
            var soldierQuery = GetEntityQuery(typeof(Translation), ComponentType.ReadOnly<SoldierMoveData>());
            
            var soldierMarchJob = new SoldierMarchJob
            {
                TranslationTypeHandle = GetComponentTypeHandle<Translation>(false),
                SoldierMoveTypeHandle = GetComponentTypeHandle<SoldierMoveData>(true),
                DeltaTime = Time.DeltaTime
            };

            Dependency = soldierMarchJob.ScheduleParallel(soldierQuery, 1, Dependency);
            RequireForUpdate(soldierQuery);
        }
    }
    public struct SoldierMarchJob : IJobEntityBatch
    {
        public ComponentTypeHandle<Translation> TranslationTypeHandle;

        [ReadOnly] public ComponentTypeHandle<SoldierMoveData> SoldierMoveTypeHandle;

        public float DeltaTime;
        
        [BurstCompile]
        public void Execute(ArchetypeChunk batchInChunk, int batchIndex)
        {
            var translationArray = batchInChunk.GetNativeArray(TranslationTypeHandle);
            var soldierMoveDataArray = batchInChunk.GetNativeArray(SoldierMoveTypeHandle);

            for (var i = 0; i < batchInChunk.Count; i++)
            {
                var translation = translationArray[i];
                var soldierMoveData = soldierMoveDataArray[i];
                translation.Value.x += soldierMoveData.Value * DeltaTime;
                translationArray[i] = translation;
            }
        }
    }
}