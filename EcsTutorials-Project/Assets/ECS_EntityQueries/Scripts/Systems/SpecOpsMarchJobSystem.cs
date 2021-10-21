using Unity.Burst;
using Unity.Collections;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;

namespace TMG.EntityQueries
{
    //[DisableAutoCreation]
    public class SpecOpsMarchJobSystem : SystemBase
    {
        private EntityQueryDesc _entityQueryDesc;
        
        protected override void OnCreate()
        {
            _entityQueryDesc = new EntityQueryDesc
            {
                All = new[] {ComponentType.ReadOnly<SoldierMoveData>()},
                Any = new []{ComponentType.ReadOnly<SpinSpeedData>(), ComponentType.ReadOnly<JumpData>()},
                None = new[] {ComponentType.ReadOnly<CommanderTag>()}
            };
        }

        protected override void OnUpdate()
        {
            var soldierQuery = GetEntityQuery(_entityQueryDesc);

            var soldierMarchJob = new ComplexSoldierMarchJob()
            {
                TranslationTypeHandle = GetComponentTypeHandle<Translation>(false),
                RotationTypeHandle = GetComponentTypeHandle<Rotation>(false),
                SpinSpeedTypeHandle = GetComponentTypeHandle<SpinSpeedData>(true),
                JumpDataTypeHandle = GetComponentTypeHandle<JumpData>(true),
                DeltaTime = Time.DeltaTime,
                CurrentTime = (float) Time.ElapsedTime
            };

            Dependency = soldierMarchJob.ScheduleParallel(soldierQuery, 1, Dependency);
        }
    }
    
    public struct ComplexSoldierMarchJob : IJobEntityBatch
    {
        public ComponentTypeHandle<Translation> TranslationTypeHandle;
        public ComponentTypeHandle<Rotation> RotationTypeHandle;
        
        [ReadOnly] public ComponentTypeHandle<SpinSpeedData> SpinSpeedTypeHandle;
        [ReadOnly] public ComponentTypeHandle<JumpData> JumpDataTypeHandle;
        
        public float DeltaTime;
        public float CurrentTime;
        
        [BurstCompile]
        public void Execute(ArchetypeChunk batchInChunk, int batchIndex)
        {
            if (batchInChunk.Has(SpinSpeedTypeHandle))
            {
                var spinSpeedDataArray = batchInChunk.GetNativeArray(SpinSpeedTypeHandle);
                var rotationDataArray = batchInChunk.GetNativeArray(RotationTypeHandle);
                for (var i = 0; i < batchInChunk.Count; i++)
                {
                    var rotation = rotationDataArray[i];
                    rotation.Value = math.mul(rotation.Value,
                        quaternion.RotateZ(math.radians(spinSpeedDataArray[i].Value * DeltaTime)));
                    rotationDataArray[i] = rotation;

                }
            }
            else if(batchInChunk.Has(JumpDataTypeHandle))
            {
                var translationArray = batchInChunk.GetNativeArray(TranslationTypeHandle);
                var jumpDataArray = batchInChunk.GetNativeArray(JumpDataTypeHandle);
                
                for (var i = 0; i < batchInChunk.Count; i++)
                {
                    var translation = translationArray[i];
                    var magnitude = jumpDataArray[i].Value;
                    translation.Value.y = magnitude * math.cos(CurrentTime * 4 + 1f) + 2f;
                    translationArray[i] = translation;
                }
            }
        }
    }
}