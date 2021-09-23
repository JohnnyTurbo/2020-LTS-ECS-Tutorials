using Unity.Burst;
using Unity.Collections;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;

namespace TMG.BatchedJobs
{
    public class MovementSystem : SystemBase
    {
        private EntityQuery _entityQuery;
        
        protected override void OnCreate()
        {
            _entityQuery = GetEntityQuery(ComponentType.ReadWrite<Translation>(),
                ComponentType.ReadOnly<OscillateMagnitudeData>());
        }

        protected override void OnUpdate()
        {
            var movementJob = new MovementJob
            {
                CurrentTime = (float) Time.ElapsedTime,
                TranslationTypeHandle = GetComponentTypeHandle<Translation>(false),
                MagnitudeTypeHandle = GetComponentTypeHandle<OscillateMagnitudeData>(true),
                DoubleSpeedTypeHandle = GetComponentTypeHandle<DoubleSpeedTag>(true)
            };

            Dependency = movementJob.ScheduleParallel(_entityQuery, 1, Dependency);
        }
    }

    public struct MovementJob : IJobEntityBatch
    {
        public ComponentTypeHandle<Translation> TranslationTypeHandle;
        
        [ReadOnly] public ComponentTypeHandle<OscillateMagnitudeData> MagnitudeTypeHandle;
        [ReadOnly] public ComponentTypeHandle<DoubleSpeedTag> DoubleSpeedTypeHandle;
        
        public float CurrentTime;
        
        [BurstCompile]
        public void Execute(ArchetypeChunk batchInChunk, int batchIndex)
        {
            var frequency = batchInChunk.Has(DoubleSpeedTypeHandle) ? 2f : 1f;
            
            var moveSpeedArray = batchInChunk.GetNativeArray(MagnitudeTypeHandle);
            var translationArray = batchInChunk.GetNativeArray(TranslationTypeHandle);
            
            for (var i = 0; i < batchInChunk.Count; i++)
            {
                var magnitude = moveSpeedArray[i].Value;
                var curTranslation = translationArray[i];
                curTranslation.Value.y = magnitude * math.sin(CurrentTime * frequency + magnitude + 1f);
                translationArray[i] = curTranslation;
            }
        }
    }
}