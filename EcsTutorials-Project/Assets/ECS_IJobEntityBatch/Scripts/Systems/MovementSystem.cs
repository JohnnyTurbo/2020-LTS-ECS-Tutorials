using Unity.Burst;
using Unity.Collections;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;
using UnityEngine;

namespace TMG.BatchedJobs
{
    public class MovementSystem : SystemBase
    {
        private EntityQuery _entityQuery;
        
        protected override void OnCreate()
        {
            _entityQuery = GetEntityQuery(ComponentType.ReadOnly<MoveSpeedData>(),
                ComponentType.ReadWrite<Translation>());
        }

        protected override void OnUpdate()
        {
            var movementJob = new MovementJob();

            movementJob.CurrentTime = (float)Time.ElapsedTime;
            movementJob.TranslationTypeHandle = GetComponentTypeHandle<Translation>(false);
            movementJob.DoubleSpeedTypeHandle = GetComponentTypeHandle<DoubleSpeedTag>(true);
            movementJob.MoveSpeedTypeHandle = GetComponentTypeHandle<MoveSpeedData>(true);

            Dependency = movementJob.ScheduleParallel(_entityQuery, 1, Dependency);
        }
    }

    public struct MovementJob : IJobEntityBatch
    {
        [ReadOnly]
        public ComponentTypeHandle<MoveSpeedData> MoveSpeedTypeHandle;
        public ComponentTypeHandle<Translation> TranslationTypeHandle;
        [ReadOnly]
        public ComponentTypeHandle<DoubleSpeedTag> DoubleSpeedTypeHandle;
        public float CurrentTime;
        
        [BurstCompile]
        public void Execute(ArchetypeChunk batchInChunk, int batchIndex)
        {
            var frequency = batchInChunk.Has(DoubleSpeedTypeHandle) ? 2 : 1;

            var moveSpeedArray = batchInChunk.GetNativeArray(MoveSpeedTypeHandle);
            var translationArray = batchInChunk.GetNativeArray(TranslationTypeHandle);
            
            for (var i = 0; i < batchInChunk.Count; i++)
            {
                var magnitude = moveSpeedArray[i].Value;
                var curTranslation = translationArray[i];
                curTranslation.Value.y = magnitude * math.sin(CurrentTime * frequency + magnitude + 1f);
                translationArray[i] = curTranslation;
                /*var newPosition = new float3
                {
                    x = batchIndex,
                    y = magnitude * math.sin(CurrentTime * frequency + magnitude + 1f),
                    z = 0f
                };*/
                //cannot set translationArray[i].Value
                //translationArray[i] = new Translation {Value = newPosition};
            }
        }
    }
}