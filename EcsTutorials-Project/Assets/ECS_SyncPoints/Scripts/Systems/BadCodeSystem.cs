using Unity.Burst;
using Unity.Collections;
using Unity.Entities;
using Unity.Jobs;
using Unity.Mathematics;
using Unity.Transforms;

namespace TMG.SyncPoints
{
    public class BadCodeSystem : SystemBase
    {
        private EntityQuery _movableEntities;

        protected override void OnCreate()
        {
            _movableEntities = GetEntityQuery(typeof(Translation), ComponentType.ReadOnly<MoveData>());
        }

        protected override void OnUpdate()
        {
            var simpleMoveJob = new SimpleMoveJob
            {
                TranslationTypeHandle = GetComponentTypeHandle<Translation>(),
                MoveDataTypeHandle = GetComponentTypeHandle<MoveData>(true),
                DeltaTime = Time.DeltaTime
            };
            //Dependency = simpleMoveJob.ScheduleParallel(_movableEntities, 1, Dependency);
            JobHandle badJob = simpleMoveJob.ScheduleParallel(_movableEntities, 1, Dependency);
            /*Dependency.Complete();
            var movableEntities = _movableEntities.ToEntityArray(Allocator.Temp);
            var firstEntity = movableEntities[0];
            var position = GetComponent<Translation>(firstEntity);
            position.Value = float3.zero;
            SetComponent(firstEntity, position);*/
            
            //Dependency = simpleMoveJob.ScheduleParallel(_movableEntities, 1, Dependency);
            JobHandle badJob2 = simpleMoveJob.ScheduleParallel(_movableEntities, 1, Dependency);
        }
    }

    public struct SimpleMoveJob : IJobEntityBatch
    {
        public ComponentTypeHandle<Translation> TranslationTypeHandle;
        [ReadOnly] public ComponentTypeHandle<MoveData> MoveDataTypeHandle;
        public float DeltaTime;
        
        [BurstCompile]
        public void Execute(ArchetypeChunk batchInChunk, int batchIndex)
        {
            var translationArray = batchInChunk.GetNativeArray(TranslationTypeHandle);
            var moveDataArray = batchInChunk.GetNativeArray(MoveDataTypeHandle);
            for (var i = 0; i < batchInChunk.Count; i++)
            {
                var translation = translationArray[i];
                translation.Value.x += moveDataArray[i].MoveSpeed * DeltaTime;
                translationArray[i] = translation;
            }
        }
    }
}